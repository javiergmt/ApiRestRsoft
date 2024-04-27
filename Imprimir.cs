using System;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using ApiRestRs.Models;
using System.Linq;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;

namespace ApiRestRs

{
    public class Imprimir
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
                       uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
                                  FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);
            
        public static void ImprimirComanda(EnMesaDetMult? m , string? con)
        {
            SqlConnection connection = new(con);
            SqlDataReader dataReader;
            string sql = "Select idSectorExped,S.Descripcion,combinacomandas, I.* from sectores_exped S inner join Impresoras I on S.idImpresora = I.idImpresora";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                // Obtengo todos los sectores los recorro y los aplico al filtro
                // Select idLugarEsprd, idSectorExped,S.Descripcion,combinacomandas, I.* from sectores_expedicion S inner join Impresora I on S.idImpresora = I.idImpresora
                int idSectorExped = Convert.ToInt32(dataReader["idSectorExped"]);
                string iP = dataReader["iP"].ToString();
                int cantSaltos = Convert.ToInt32(dataReader["cantSaltos"]);
                int cantSaltosCut = Convert.ToInt32(dataReader["cantSaltosCut"]);
                int margen = Convert.ToInt32(dataReader["margen"]);
                string margin = "".PadLeft(margen);
                int anchoHoja = Convert.ToInt32(dataReader["anchoHoja"]);

                string impresora = ""; // Nombre de la impresora
                string hostName = ""; // Nombre del equipo donde esta la impresora

                //string hostName = System.Net.Dns.GetHostName();
                //string hostName = System.Environment.MachineName;

                //string ubicacion = string.Format("\\\\{0}\\{1}", hostName, impresora);

                // Uso este filtro para obtnener los platos de un mismo lugar y sector , mas los combos
                //var detalle = m.MesaDetM.Where(md => md.idSectorExped = idSectorExped).Where(md => md.idSectorExped = 0 );
                // if (detalle.Count() > 0)
               
                #region Linq Deffered Query  
                var detalle = from md in m.MesaDetM
                             where md.idSectorExped == idSectorExped || md.idTipoConsumo == "CB"
                           
                             select md;
                #endregion


                if ( detalle.Count() > 0 )
                    {
                    var titulo = false; 
                    
                    SafeFileHandle fileHandle = CreateFile(iP, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                    if (fileHandle.IsInvalid)
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }
                    using (FileStream fs = new(fileHandle, FileAccess.Write))
                    {
                        

                        foreach (var reng in detalle)
                        {
                            if (reng.Combos != null)
                            {
                                foreach (var comb in reng.Combos)
                                {
                                    if( comb.idSectorExped == idSectorExped)
                                    {
                                        if (!titulo)
                                        {
                                            titulo = true;
                                            Inicializar(fs);
                                            Letra_Doble_Alto(fs);
                                            ImprimirTexto(fs,margin + "########## INICIO ##########");

                                            ImprimirTexto(fs, margin + "Sector: " + dataReader["Descripcion"].ToString());
                                            ImprimirTexto(fs, margin + "Mesa: " + reng.nroMesa.ToString());
                                            ImprimirTexto(fs, margin + "Mozo: (" + reng.idMozo.ToString()+") "+reng.nombreMozo);
                                            ImprimirTexto(fs, margin + reng.fechaHora.ToShortDateString() + ' ' + reng.fechaHora.ToShortTimeString());
                                            ImprimirTexto(fs, margin + "----------------------------");
                                        }

                                        
                                        ImprimirTexto(fs, margin + comb.cant.ToString() + " EN COMBO: " + comb.descripcion);
                                        if (comb.CombosGustos != null)
                                        {
                                            foreach (var gusto in comb.CombosGustos)
                                            {
                                                ImprimirTexto(fs, margin + "   " + gusto.descripcion);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!titulo)
                                {
                                    titulo = true;
                                    Inicializar(fs);
                                    Letra_Doble_Alto(fs);
                                    ImprimirTexto(fs, margin + "########## INICIO ##########");
                              
                                    ImprimirTexto(fs, margin + "Sector: " + dataReader["Descripcion"].ToString());
                                    ImprimirTexto(fs, margin + "Mesa: " + reng.nroMesa.ToString());
                                    ImprimirTexto(fs, margin + "Mozo: (" + reng.idMozo.ToString() + ") " + reng.nombreMozo);
                                    ImprimirTexto(fs, margin + reng.fechaHora.ToShortDateString() + ' ' + reng.fechaHora.ToShortTimeString());
                                    ImprimirTexto(fs, margin + "----------------------------");
                                }
                              
                                ImprimirTexto(fs, margin + reng.cant.ToString() + ' ' + reng.descripcion);
                                if (reng.idTamanio != 0)
                                {
                                    ImprimirTexto(fs, margin + "   " + reng.tamanio);
                                }
                                if (reng.Gustos != null)
                                {
                                    foreach (var gusto in reng.Gustos)
                                    {
                                        ImprimirTexto(fs, margin + "   " + gusto.descripcion);
                                    }
                                }
                                
                            }
                            
                        }
                        if (titulo)
                        {
                            ImprimirTexto(fs, margin + "----------------------------");
                            SaltarLinea(fs, "1");
                            ImprimirTexto(fs, margin + "########## FIN ##########");
                            SaltarLinea(fs, cantSaltosCut.ToString());
                            Corte_Papel(fs);


                        }
                        fs.Dispose();

                    }
                }
            }
            connection.Close();
        }

        public static void Inicializar(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(0x40); // @
        }
        public static void SaltarLinea(FileStream fs, string n)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte('d')); // feed n lines
            fs.WriteByte(Convert.ToByte(n)); // n lines
           
        }

        public static void ImprimirTexto(FileStream fs, string texto)
        {
            fs.Write(Encoding.ASCII.GetBytes(texto), 0, texto.Length);
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte('d')); // feed n lines
            fs.WriteByte(Convert.ToByte(1)); // n lines

        }

        public static void Letra_Normal_Normal(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(33)); //
            fs.WriteByte(Convert.ToByte(1)); // 

        }

        public static void Letra_Negrita(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(33)); // 
            fs.WriteByte(Convert.ToByte(8)); // 

        }

        public static void Letra_Doble_Alto(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(33)); 
            fs.WriteByte(Convert.ToByte(16)); 

        }

        public static void Letra_Doble_Ancho(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(33)); 
            fs.WriteByte(Convert.ToByte(32)); 

        }

        public static void Letra_Negrita_Doble_Ancho(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(33)); // 
            fs.WriteByte(Convert.ToByte(40)); // 

        }
        public static void Letra_Negrita_Doble_Alto(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(33)); // 
            fs.WriteByte(Convert.ToByte(24)); // 

        }
        public static void Corte_Papel(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte(105)); // 
           
        }

    }
  
}
