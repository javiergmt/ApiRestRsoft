//using System;
//using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using ApiRestRs.Models;
using System.Data.SqlClient;
using System.Globalization;


namespace ApiRestRs

{
    
    public class Imprimir
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
                       uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
                                  FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

        public static void ImprimirComanda(EnMesaDetMult? m, string? con)
        {
            if (m == null)
            {
                return;
            }

            SqlConnection conPar = new(con);
            SqlDataReader dataParamCom;
            string sqlParamCom = "Select imprimeComandas,isnull(idImpresoraComandaCentral,0) as idImpresoraComandaCentral," +
                "DelimitadorEntrada1,DelimitadorEntrada2,DelimitadorEntrada3,concatenarGustos," +
                "detallarPlatosEnCombina From Parametros_Comandas";
            conPar.Open();
            SqlCommand cmdParamCom = new SqlCommand(sqlParamCom, conPar);
            dataParamCom = cmdParamCom.ExecuteReader();
            dataParamCom.Read();
            if (dataParamCom["imprimeComandas"].ToString() == "N")
            {
                return;
            }

            string? Delim1 = "";
            string? Delim2 = "";
            string? Delim3 = "";
            int idImpComandaCentral = 0;
            int detallarPlatos = 0;
            int concatenarGustos = 0;


            try
            {
                idImpComandaCentral = Convert.ToInt32(dataParamCom["idImpresoraComandaCentral"]);
                detallarPlatos = Convert.ToInt32(dataParamCom["detallarPlatosEnCombina"]);
                concatenarGustos = Convert.ToInt32(dataParamCom["concatenarGustos"]);

                Delim1 = dataParamCom["DelimitadorEntrada1"].ToString();
                Delim2 = dataParamCom["DelimitadorEntrada2"].ToString();
                Delim3 = dataParamCom["DelimitadorEntrada3"].ToString();
            } catch (Exception e)
            {
                Delim1 = "";
                Delim2 = "";
                Delim3 = "";
                idImpComandaCentral = 0;
                detallarPlatos = 0;
                concatenarGustos = 0;
            }
            conPar.Close();

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
                string? iP = dataReader["iP"].ToString();
                int cantSaltos = Convert.ToInt32(dataReader["cantSaltos"]);
                int cantSaltosCut = Convert.ToInt32(dataReader["cantSaltosCut"]);
                int margen = Convert.ToInt32(dataReader["margen"]);
                string margin = "".PadLeft(margen);
                int anchoHoja = Convert.ToInt32(dataReader["anchoHoja"]);

                //string? impresora = ""; // Nombre de la impresora
                //string? hostName = ""; // Nombre del equipo donde esta la impresora

                //string hostName = System.Net.Dns.GetHostName();
                //string hostName = System.Environment.MachineName;

                //string ubicacion = string.Format("\\\\{0}\\{1}", hostName, impresora);

                // Uso este filtro para obtnener los platos de un mismo lugar y sector , mas los combos

                #region Linq Deffered Query  
                var entradas = from md in m.MesaDetM
                               where (md.idSectorExped == idSectorExped && md.esEntrada )
                               select md;
                #endregion

                #region Linq Deffered Query  
                var detalle = from md in m.MesaDetM
                              where (md.idSectorExped == idSectorExped || md.idTipoConsumo == "CB")
                              && (!md.esEntrada)
                              select md;
                #endregion


                if (detalle.Count() > 0 || entradas.Count() > 0)
                {

                    var titulo = false;
                    var hayEntradas = entradas.Count() > 0;
                    if ( iP != null && iP != "") 
                    {
                    SafeFileHandle fileHandle = CreateFile(iP, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                    if (fileHandle.IsInvalid)
                    {
                        connection.Close(); 
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

                        if (hayEntradas)
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
                                    if (comb.idSectorExped == idSectorExped)
                                    {
                                        if (!titulo)
                                        {
                                            titulo = true;
                                            Inicializar(fs);
                                            ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), reng.nroMesa.ToString(),
                                                            reng.idMozo.ToString(), reng.nombreMozo, 1, reng.fechaHora);

                                        }


                                        ImprimirTexto(fs, margin + comb.cant.ToString() + " EN COMBO: " + comb.descripcion +' '+ comb.tamanio);
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
                                       reng.idMozo.ToString(), reng.nombreMozo, 1, reng.fechaHora);

                                }
                                if (reng.Combos != null)
                                {
                                    // Impresion de Combos
                                    foreach (var comb in reng.Combos)
                                    {
                                        if (comb.idSectorExped == idSectorExped)
                                        {
                                            if (!titulo)
                                            {
                                                titulo = true;
                                                Inicializar(fs);
                                                ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), reng.nroMesa.ToString(),
                                                                reng.idMozo.ToString(), reng.nombreMozo, 1, reng.fechaHora);

                                            }


                                            ImprimirTexto(fs, margin + comb.cant.ToString() + " EN COMBO: " + comb.descripcion);
                                            if (comb.obs != null && comb.obs != "")
                                            {
                                                ImprimirTexto(fs, margin + "   " + comb.obs);
                                            }
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
                                              where ( (cb.idSectorExped != idSectorExped) && (cb.idSectorExped !=0 ))
                                              select cb;
                                #endregion
                                string[] sectComb = [];
                                List <string> platosComb = [];
                                    if (combina != null)
                                    {
                                        foreach (var comb in combina)
                                        {
                                            if (comb != null)
                                            {
                                                if ((comb.idTipoConsumo == "CB") & (comb.Combos != null))
                                                {
                                                    foreach (var combos in comb.Combos)
                                                    {
                                                        var sect = combos.idSectorExped;
                                                        if (sect != idSectorExped)
                                                        {
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
                                                                if (indice == -1)
                                                                {
                                                                    Array.Resize(ref sectComb, sectComb.Length + 1);
                                                                    sectComb[sectComb.Length - 1] = descripcion;
                                                                }
                                                                platosComb.Add(combos.descripcion);

                                                            }
                                                            conSec.Close();
                                                        }
                                                    }
                                                }
                                                else
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
                                                        if (indice == -1)
                                                        {
                                                            Array.Resize(ref sectComb, sectComb.Length + 1);
                                                            sectComb[sectComb.Length - 1] = descripcion;
                                                        }
                                                        platosComb.Add(comb.descripcion);

                                                    }
                                                    conSec.Close();
                                                }
                                            }
                                        };

                                        for (int i = 0; i < sectComb.Length; i++)
                                        {
                                            Letra_Normal_Normal(fs);
                                            ImprimirTexto(fs, margin + "Combina con: " + sectComb[i]);
                                        }

                                        if (detallarPlatos == 1)
                                        {
                                            for (int i = 0; i < platosComb.Count; i++)
                                            {
                                                Letra_Normal_Normal(fs);
                                                ImprimirTexto(fs, margin + platosComb[i]);
                                            }
                                        }

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
                    
                    } // fin del if de impresora
                }
            } // Fin del While
            connection.Close();

            // Comanda Centralizada
            if (idImpComandaCentral != 0)
            {

                Boolean titulo = false;
                SqlConnection conCent = new(con);
                SqlDataReader dataCent;
                string sqlCent = "Select * from Impresoras where idImpresora = " + idImpComandaCentral;
                conCent.Open();
                SqlCommand cmdCent = new SqlCommand(sqlCent, conCent);
                dataCent = cmdCent.ExecuteReader();
                dataCent.Read();
                string? iP = dataCent["iP"].ToString();
                int cantSaltos = Convert.ToInt32(dataCent["cantSaltos"]);
                int cantSaltosCut = Convert.ToInt32(dataCent["cantSaltosCut"]);
                int margen = Convert.ToInt32(dataCent["margen"]);
                string? margin = "".PadLeft(margen);
                int anchoHoja = Convert.ToInt32(dataCent["anchoHoja"]);
                conCent.Close();

                if (iP != null && iP != "")
                {
                    SafeFileHandle fileHandle = CreateFile(iP, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                    if (fileHandle.IsInvalid)
                    {
                        connection.Close();
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }

                    using (FileStream fs = new(fileHandle, FileAccess.Write))
                    {
                        Inicializar(fs);
                        SqlConnection conSect = new(con);
                        SqlDataReader dataSect;
                        string sqlSect = "Select * From Sectores_Exped Order by idSectorExped ";
                        conSect.Open();
                        SqlCommand cmdSect = new SqlCommand(sqlSect, conSect);
                        dataSect = cmdSect.ExecuteReader();
                        while (dataSect.Read())
                        {
                            int idSectorExped = Convert.ToInt32(dataSect["idSectorExped"]);
                            string? descSector = dataSect["Descripcion"].ToString();
                            #region Linq Deffered Query  
                            var centralizada = from md in m.MesaDetM
                                               where (md.idSectorExped == idSectorExped || md.idTipoConsumo == "CB")
                                               select md;
                            #endregion
                            Boolean titulosector = false;
                            foreach (var ct in centralizada)
                            {
                                if (!titulo)
                                {
                                    titulo = true;

                                    Inicializar(fs);
                                    ImprimirTitulo(fs, margin, "", ct.nroMesa.ToString(),
                                                    ct.idMozo.ToString(), ct.nombreMozo, 1, ct.fechaHora);
                                    SaltarLinea(fs, "1");
                                    Letra_Normal_Normal(fs);
                                }


                                if (ct.Combos != null)
                                {
                                    // Impresion de Combos
                                    foreach (var comb in ct.Combos)
                                    {
                                        if (comb.idSectorExped == idSectorExped)
                                        {
                                            if (!titulosector)
                                            {
                                                titulosector = true;
                                                SaltarLinea(fs, "1");
                                                Letra_Normal_Normal(fs);
                                                ImprimirTexto(fs, margin + "--------------------------------------------------");
                                                Letra_Doble_Alto(fs);
                                                ImprimirTexto(fs, margin + "          " + "Sector: " + descSector);
                                                Letra_Normal_Normal(fs);
                                                ImprimirTexto(fs, margin + "--------------------------------------------------");

                                            }
                                            Letra_Doble_Alto(fs);
                                            if (comb.impCentralizada == 1)
                                            {
                                                ImprimirTexto(fs, margin + comb.cant.ToString() + " EN COMBO: " + comb.descripcion + ' ' + comb.tamanio);
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
                                }
                                else
                                {
                                    if (!titulosector)
                                    {
                                        titulosector = true;
                                        SaltarLinea(fs, "1");
                                        Letra_Normal_Normal(fs);
                                        ImprimirTexto(fs, margin + "--------------------------------------------------");
                                        Letra_Doble_Alto(fs);
                                        ImprimirTexto(fs, margin + "          " + "Sector: " + descSector);
                                        Letra_Normal_Normal(fs);
                                        ImprimirTexto(fs, margin + "--------------------------------------------------");

                                    }
                                    Letra_Doble_Alto(fs);
                                    if ( ct.impCentralizada == 1)
                                    {
                                        ImprimirTexto(fs, margin + ct.cant.ToString() + ' ' + ct.descripcion);
                                        if (ct.obs != null && ct.obs != "")
                                        {
                                            ImprimirTexto(fs, margin + "   " + ct.obs);
                                        }
                                        if (ct.idTamanio != 0)
                                        {
                                            ImprimirTexto(fs, margin + "   " + ct.tamanio);
                                        }
                                        if (ct.Gustos != null)
                                        {
                                            foreach (var gusto in ct.Gustos)
                                            {
                                                ImprimirTexto(fs, margin + "   " + gusto.descripcion);
                                            }
                                        }
                                    }
                                }
                                //            
                            } // fin del foreach de centralizada
                        } // fin del while de sectores
                        if (titulo)
                        {
                            Letra_Normal_Normal(fs);
                            ImprimirTexto(fs, margin + "--------------------------------------------------");
                            SaltarLinea(fs, "1");
                            ImprimirTexto(fs, margin + "     " + "####################### FIN ####################");
                            SaltarLinea(fs, cantSaltosCut.ToString());
                            Corte_Papel(fs);
                        }
                        fs.Dispose();
                        conSect.Close();

                    } // fin del using de fs
                }
                
            } // fin del if de impresora centralizada

        }

        public static string? retNombreSector(int idSector,string con)           
        {
            SqlConnection conCent = new(con);
            SqlDataReader dataCent;
            string sqlCent = "Select * From Sectores_Exped where idSectorExped = " + idSector;
            conCent.Open();
            SqlCommand cmdCent = new SqlCommand(sqlCent, conCent);
            dataCent = cmdCent.ExecuteReader();
            dataCent.Read();
            string? descRet = "";
            try
            {
                descRet = dataCent["Descripcion"].ToString();
            } catch (Exception e)
            {
                descRet = "";
            }
            conCent.Close();
            return descRet;
        }
        public static void ImprimirTitulo(FileStream? fs,string? margin,string? sector, string? nroMesa,
            string? nroMozo, string? nombMozo, int comensales, DateTime fechaHora)

        {
            if (fs == null) {
                return;
            }
            Letra_Normal_Normal(fs);
            ImprimirTexto(fs, margin + "     " + "################### INICIO ###################");
            Letra_Doble_Alto(fs);
            if (sector != "")
            {
                ImprimirTexto(fs, margin + "          " + "Sector: " + sector);
            }
            ImprimirTexto(fs,"");
            ImprimirTexto(fs, margin + "Mesa: " + nroMesa);
            ImprimirTexto(fs, margin + "Mozo: (" + nroMozo + ") " + nombMozo);
            ImprimirTexto(fs, margin + fechaHora.ToShortDateString() + ' ' + fechaHora.ToShortTimeString());
            Letra_Normal_Normal(fs);
            ImprimirTexto(fs, margin + "--------------------------------------------------");
            Letra_Doble_Alto(fs);
        }

        public static void ImprimirMensaje(int idSectorExp,int idImpresora, string? descripcion,string? nombre, string? con)
        {
            SqlConnection conSect = new(con);
            SqlDataReader dataSect;
            string sqlSect = "Select idSectorExped,Descripcion from sectores_exped  " +
                " Where idSectorExped = "+idSectorExp;
            conSect.Open();
            SqlCommand cmdSect = new SqlCommand(sqlSect, conSect);
            dataSect = cmdSect.ExecuteReader();
            dataSect.Read();
            string? descSector = dataSect["Descripcion"].ToString();
            conSect.Close();

            SqlConnection connection = new(con);
            SqlDataReader dataReader;
            string sql = "Select * from Impresoras where idImpresora = " + idImpresora;
            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            string? iP = dataReader["iP"].ToString();
            int cantSaltos = Convert.ToInt32(dataReader["cantSaltos"]);
            int cantSaltosCut = Convert.ToInt32(dataReader["cantSaltosCut"]);
            int margen = Convert.ToInt32(dataReader["margen"]);
            string margin = "".PadLeft(margen);
            int anchoHoja = Convert.ToInt32(dataReader["anchoHoja"]);
            if (iP != null && iP != "")
            {
                SafeFileHandle fileHandle = CreateFile(iP, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                if (fileHandle.IsInvalid)
                {
                    connection.Close();
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
                using (FileStream fs = new(fileHandle, FileAccess.Write))
                {
                    Inicializar(fs);

                    Letra_Normal_Normal(fs);
                    ImprimirTexto(fs, margin + "######################################################");
                    Letra_Doble_Alto(fs);
                    ImprimirTexto(fs, margin + "                 MENSAJE");
                    Letra_Normal_Normal(fs);
                    ImprimirTexto(fs, margin + "######################################################");
                    ImprimirTexto(fs, margin + "Sector: " + descSector);
                    ImprimirTexto(fs, margin + nombre);
                    ImprimirTexto(fs, margin + "Fecha:  " + DateTime.Now);
                    ImprimirTexto(fs, margin + "--------------------------------------------------");
                    Letra_Doble_Alto(fs);
                    ImprimirTexto(fs, margin + descripcion);
                    ImprimirTexto(fs, margin);
                    Letra_Normal_Normal(fs);
                    ImprimirTexto(fs, margin + "######################################################");
                    Letra_Doble_Alto(fs);
                    ImprimirTexto(fs, margin + "             FIN MENSAJE");
                    Letra_Normal_Normal(fs);
                    ImprimirTexto(fs, margin + "######################################################");
                    SaltarLinea(fs, cantSaltosCut.ToString());
                    Corte_Papel(fs);
                    fs.Dispose();
                }
            }
            connection.Close(); 
        }


        public static void Inicializar(FileStream fs)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(0x40); // @
        }

        public static void SaltarLinea(FileStream fs, string? n)
        {
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte('d')); // feed n lines
            fs.WriteByte(Convert.ToByte(n)); // n lines
           
        }

        public static void ImprimirTexto(FileStream fs, string? texto)
        {
            if (texto == null)
            {
                return;
            }
            fs.Write(Encoding.ASCII.GetBytes(texto), 0, texto.Length);
            fs.WriteByte(0x1b); // ESC
            fs.WriteByte(Convert.ToByte('d')); // feed n lines
            fs.WriteByte(Convert.ToByte(1)); // n lines

        }

        public static void ImprimirCentrado(FileStream fs, string? margin, string? cadena, int ancho)
        {
           
            int cantEspacios = 0;
            if (cadena != null)
            {
                cantEspacios = (int)(ancho - cadena.Length) / 2;

            }
            string espacios = string.Concat(System.Linq.Enumerable.Repeat(' ', (int)cantEspacios));
            ImprimirTexto(fs, margin + espacios + cadena);
        }

        public static string EspaciosIzq(string? cadena, int ancho)
        {
            int cantEspacios = 0;
            if (cadena != null )
            {
                cantEspacios = (int)(ancho - cadena.Length);

            }
            string espacios = string.Concat(System.Linq.Enumerable.Repeat(' ', (int)cantEspacios));
            return espacios + cadena;
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

        public static void ImprimirAceptacion(MesaCerrar? m, string? con)
        {
            if (m == null)
            {
                return;
            }
            CultureInfo culture = new("es-AR");
            SqlConnection conPar = new(con);
            SqlDataReader dataParamCom;
            string sqlParamCom = "Select * From Parametros";
            conPar.Open();
            SqlCommand cmdParamCom = new SqlCommand(sqlParamCom, conPar);
            dataParamCom = cmdParamCom.ExecuteReader();
            dataParamCom.Read();
            string? Leyenda1 = dataParamCom["LeyendaAcep1"].ToString();
            string? Leyenda2 = dataParamCom["LeyendaAcep2"].ToString();
            string? Leyenda3 = dataParamCom["LeyendaAcep3"].ToString();
            string? Leyenda4 = dataParamCom["LeyendaAcep4"].ToString();
            string? LeyendaPie1 = dataParamCom["LeyendaPie1"].ToString();
            string? LeyendaPie2 = dataParamCom["LeyendaPie1"].ToString();
            string? LeyendaPie3 = dataParamCom["LeyendaPie1"].ToString();
            string? LeyendaPie4 = dataParamCom["LeyendaPie1"].ToString();
            int idImpresora = Convert.ToInt32(dataParamCom["idImpresoraTicket"]);

            SqlConnection connection = new(con);
            SqlDataReader dataReader;
            string sql = "Select * from Impresoras where idImpresora = " + idImpresora;
            connection.Open();
            SqlCommand cmd = new SqlCommand(sql, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            string? iP = dataReader["iP"].ToString();
            int cantSaltos = Convert.ToInt32(dataReader["cantSaltos"]);
            int cantSaltosCut = Convert.ToInt32(dataReader["cantSaltosCut"]);
            int margen = Convert.ToInt32(dataReader["margen"]);
            string margin = "".PadLeft(margen);
            int anchoHoja = Convert.ToInt32(dataReader["anchoHoja"]);
            if (iP != null && iP != "")
            {
                SafeFileHandle fileHandle = CreateFile(iP, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
                if (fileHandle.IsInvalid)
                {
                    connection.Close();
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                }
                using (FileStream fs = new(fileHandle, FileAccess.Write))
                {
                    Inicializar(fs);
                    Letra_Normal_Normal(fs);

                    ImprimirCentrado(fs, margin, "<< Aceptacion de Consumo >>", 60);
                    if (Leyenda1 != "")
                    {
                        ImprimirCentrado(fs, margin, Leyenda1, 60);
                    }
                    if (Leyenda2 != "")
                    {
                        ImprimirCentrado(fs, margin, Leyenda2, 60);
                    }
                    if (Leyenda3 != "")
                    {
                        ImprimirCentrado(fs, margin, Leyenda3, 60);
                    }
                    if (Leyenda4 != "")
                    {
                        ImprimirCentrado(fs, margin, Leyenda4, 60);
                    }

                    decimal pagos = 0;
                    SqlConnection conPag = new(con);
                    SqlDataReader dataPag;
                    // Pagos en mesa
                    string sqlPag = "select SUM( case when F.tipo = 'NC' then - F.Total else F.total end ) as Pagos " +
                    " from EN_MESA_PAGOS P "+
                    " INNER JOIN FACENC F ON(P.TipoComp = F.TIPO and P.NroComp = F.Nro) "+
                    " Where(P.NroMesa = "+ m.nroMesa + ")";
                    conPag.Open();
                    SqlCommand cmdPag = new SqlCommand(sqlPag, conPag);
                    dataPag = cmdPag.ExecuteReader();
                    dataPag.Read();
                    try { 
                        pagos = Convert.ToDecimal(dataPag["Pagos"]);
                    } catch (Exception e)
                    {
                        pagos = 0;
                    }

                    conPag.Close();

                    Boolean titulo = true;
                    SqlConnection conDet = new(con);
                    SqlDataReader dataDetCon;
                    // Aca deberia hacer inner join con En_Mesa para obtener el nombre del mozo
                    string sqlDetCon = "Select R.*, M.Nombre From (Select D.Cant, D.Descripcion, " +
                        "D.Importe, E.idMozo, E.Fecha, E.NroMesa From EN_MESADET D INNER JOIN EN_MESA E ON E.NroMesa = D.NroMesa " +
                        " Where D.NroMesa = "+ m.nroMesa + ") R " +
                        "INNER JOIN MOZOS M ON M.idMozo = R.idMozo";
        
                    conDet.Open();
                    SqlCommand cmdDetCon = new SqlCommand(sqlDetCon, conDet);
                    dataDetCon = cmdDetCon.ExecuteReader();
                    decimal total = 0;

                    while (dataDetCon.Read())
                    {
                        int cant = Convert.ToInt32(dataDetCon["Cant"]);
                        string? descripcion = dataDetCon["Descripcion"].ToString();
                        decimal importe = Convert.ToDecimal(dataDetCon["Importe"]);
                        int idMozo = Convert.ToInt32(dataDetCon["idMozo"]);
                        string? nombre = dataDetCon["Nombre"].ToString();
                        total += importe;

                        if (titulo)
                        {
                            titulo = false;
                            ImprimirTexto(fs, margin + "Fecha:  " + DateTime.Now);
                            Letra_Doble_Alto(fs);
                            ImprimirTexto(fs, margin + "Mesa: " + m.nroMesa.ToString());
                            ImprimirTexto(fs, margin + "Mozo: (" + idMozo.ToString() + ") " + nombre);
                            ImprimirTexto(fs, margin + "Cliente: CONSUMIDOR FINAL");
                            ImprimirTexto(fs, margin + "");
                            Letra_Normal_Normal(fs);
                            ImprimirTexto(fs, margin + "Cant Descripcion                          Importe ");
                            ImprimirTexto(fs, margin + "--------------------------------------------------");
                        }

                        ImprimirTexto(fs, margin + cant.ToString("0.00") + " " + descripcion);
                        ImprimirTexto(fs, margin + ".................................... $ " + EspaciosIzq(importe.ToString("N", culture), 10));
                        
                    }
                    ImprimirTexto(fs, margin + "--------------------------------------------------");
                    ImprimirTexto(fs, margin + "                           Subtotal: $ " + EspaciosIzq(total.ToString("N", culture), 10));
                    if (pagos != 0)
                    {
                        ImprimirTexto(fs, margin + "                              Pagos: $ " + EspaciosIzq(pagos.ToString("N", culture), 10));
                        total = total - pagos;
                    }
                    else
                    {
                        ImprimirTexto(fs, margin + "                            Descuento:       0.00");
                    }
                    Letra_Doble_Alto(fs);
                    ImprimirTexto(fs, margin + "                 TOTAL: $ " + EspaciosIzq(total.ToString("N", culture), 10));
                    Letra_Normal_Normal(fs);
                    ImprimirTexto(fs, margin + "");
                    if (LeyendaPie1 != "")
                    {
                        ImprimirCentrado(fs, margin, LeyendaPie1, 60);
                    }
                    if (LeyendaPie2 != "")
                    {
                        ImprimirCentrado(fs, margin, LeyendaPie2, 60);
                    }
                    if (LeyendaPie3 != "")
                    {
                        ImprimirCentrado(fs, margin, LeyendaPie3, 60);
                    }
                    if (LeyendaPie4 != "")
                    {
                        ImprimirCentrado(fs, margin, LeyendaPie4, 60);
                    }

                    SaltarLinea(fs, cantSaltosCut.ToString());
                    Corte_Papel(fs);
                    fs.Dispose();
                    conDet.Close();
                }
                connection.Close();
            }
        }

    }

 }
