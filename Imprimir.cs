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
using System.Drawing.Printing;

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
            
            SqlConnection conPar = new(con);
            SqlDataReader dataParamCom;
            string sqlParamCom = "Select * From Parametros_Comandas";
            conPar.Open();
            SqlCommand cmdParamCom = new SqlCommand(sqlParamCom, conPar);
            dataParamCom = cmdParamCom.ExecuteReader();
            dataParamCom.Read();    
            if (dataParamCom["imprimeComandas"].ToString() == "N")
            {
               return;
            }

            var Delim1 = dataParamCom["DelimitadorEntrada1"].ToString();
            var Delim2 = dataParamCom["DelimitadorEntrada2"].ToString();
            var Delim3 = dataParamCom["DelimitadorEntrada3"].ToString();

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
                int combinaComandas = Convert.ToInt32(dataReader["combinacomandas"]);
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

                #region Linq Deffered Query  
                var entradas = from md in m.MesaDetM
                              where (md.idSectorExped == idSectorExped && md.esEntrada)
                              select md;
                #endregion

                #region Linq Deffered Query  
                var detalle = from md in m.MesaDetM
                              where ( md.idSectorExped == idSectorExped || md.idTipoConsumo == "CB" )
                              && ( !md.esEntrada )                                                         
                              select md;
                #endregion


                if ( detalle.Count() > 0 || entradas.Count() > 0)
                    {

                    var titulo = false;
                    var hayEntradas = entradas.Count() > 0;
                    SafeFileHandle fileHandle = CreateFile(iP, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                    if (fileHandle.IsInvalid)
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }
                    using (FileStream fs = new(fileHandle, FileAccess.Write))
                    {
                        // Recorro las entradas
                        foreach (var ent in entradas)
                        {
                            if (!titulo)
                            {
                                titulo = true;
                                Inicializar(fs);
                                ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), ent.nroMesa.ToString(),
                                               ent.idMozo.ToString(), ent.nombreMozo, 1, ent.fechaHora);

                            }
                            ImprimirTexto(fs, margin + ent.cant.ToString() + ' ' + ent.descripcion);
                            if (ent.obs != null && ent.obs != "")
                            {
                                ImprimirTexto(fs, margin + "   " + ent.obs);
                            }
                            if (ent.idTamanio != 0)
                            {
                                ImprimirTexto(fs, margin + "   " + ent.tamanio);
                            }
                            if (ent.Gustos != null)
                            {
                                foreach (var gusto in ent.Gustos)
                                {
                                    ImprimirTexto(fs, margin + "   " + gusto.descripcion);
                                }
                            }   
                        }

                        if( hayEntradas)
                        {
                            Letra_Normal_Normal(fs);
                            ImprimirTexto(fs, margin + Delim1);
                            ImprimirTexto(fs, margin + Delim2);
                            ImprimirTexto(fs, margin + Delim3);
                            Letra_Doble_Alto(fs);
                        }

                        // Recorro los platos

                        foreach (var reng in detalle)
                        {
                            if (reng.Combos != null)
                            {
                                // Impresion de Combos
                                foreach (var comb in reng.Combos)
                                {
                                    if( comb.idSectorExped == idSectorExped)
                                    {
                                        if (!titulo)
                                        {
                                            titulo = true;
                                            Inicializar(fs);
                                            ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), reng.nroMesa.ToString(),
                                                            reng.idMozo.ToString(), reng.nombreMozo, 1, reng.fechaHora);

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
                                // Impresion de Platos
                                if (!titulo)
                                {
                                    titulo = true;
                                    Inicializar(fs);
                                    ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), reng.nroMesa.ToString(),
                                       reng.idMozo.ToString(), reng.nombreMozo, 1 , reng.fechaHora);
                                    
                                }
                              
                                ImprimirTexto(fs, margin + reng.cant.ToString() + ' ' + reng.descripcion);
                                if (reng.obs != null && reng.obs != "") 
                                {
                                    ImprimirTexto(fs, margin + "   " + reng.obs);
                                }
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
                            Letra_Normal_Normal(fs);
                            ImprimirTexto(fs, margin + "--------------------------------------------------");


                            // Impresion de Combinaciones para el sector idSectorExped
                            // Busco los platos de otros sectores que tienen Combinacion y son distintos a idSectorExped
                            if (combinaComandas == 1)
                            {

                                #region Linq Deffered Query  
                                var combina = from cb in m.MesaDetM
                                              where (cb.idSectorExped != idSectorExped)
                                              select cb;
                                #endregion
                                string[] sectComb = [];
                                
                                foreach (var comb in combina)
                                {
                                   
                                    var sect = comb.idSectorExped;
                                    SqlConnection conSec = new(con);
                                    SqlDataReader dataSec;
                                    string sqlSec = "Select * From Sectores_Exped where idSectorExped = " + sect;
                                    conSec.Open();
                                    SqlCommand cmdSec = new SqlCommand(sqlSec, conSec);
                                    dataSec = cmdSec.ExecuteReader();
                                    dataSec.Read();
                                    if (Convert.ToInt32(dataSec["combinacomandas"]) == 1)
                                    {
                                        var descripcion = dataSec["Descripcion"].ToString();
                                        int indice = Array.IndexOf(sectComb, descripcion);
                                        if ( indice == -1 )
                                        {
                                            Array.Resize(ref sectComb, sectComb.Length + 1);
                                            sectComb[sectComb.Length - 1] = descripcion;
                                        }
                                        
                                    }
                                };
                                
                                for (int i = 0; i < sectComb.Length; i++)
                                {
                                    Letra_Normal_Normal(fs);
                                    ImprimirTexto(fs, margin + "Combina con: " + sectComb[i]);
                                }
                                    
                                
                            }
                            SaltarLinea(fs, "1");
                            Letra_Normal_Normal(fs);
                            ImprimirTexto(fs, margin + "     " + "####################### FIN ####################");
                            SaltarLinea(fs, cantSaltosCut.ToString());
                            Corte_Papel(fs);
                        }

                        fs.Dispose();

                    }
                }
            }
            connection.Close();
        }

        public static void ImprimirTitulo(FileStream fs,string margin,string sector, string nroMesa,
            string nroMozo, string nombMozo, int comensales, DateTime fechaHora)

        {
            Letra_Normal_Normal(fs);
            ImprimirTexto(fs, margin + "     " + "################### INICIO ###################");
            Letra_Doble_Alto(fs);
            ImprimirTexto(fs, margin + "          " + "Sector: " + sector);
            ImprimirTexto(fs,"");
            ImprimirTexto(fs, margin + "Mesa: " + nroMesa);
            ImprimirTexto(fs, margin + "Mozo: (" + nroMozo + ") " + nombMozo);
            ImprimirTexto(fs, margin + fechaHora.ToShortDateString() + ' ' + fechaHora.ToShortTimeString());
            Letra_Normal_Normal(fs);
            ImprimirTexto(fs, margin + "--------------------------------------------------");
            Letra_Doble_Alto(fs);
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
