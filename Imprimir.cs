//using System;
//using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using ApiRestRs.Models;
using System.Data.SqlClient;
using System.Globalization;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;


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
                int idSectorExped = dataReader["idSectorExped"] != DBNull.Value ? Convert.ToInt32(dataReader["idSectorExped"]) : 0;
                int combinaComandas = dataReader["combinacomandas"] != DBNull.Value ?  Convert.ToInt32(dataReader["combinacomandas"]) : 0;
                string? iP = dataReader["iP"].ToString();
                int cantSaltos = dataReader["cantSaltos"] != DBNull.Value ? Convert.ToInt32(dataReader["cantSaltos"]) : 0;
                int cantSaltosCut = dataReader["cantSaltosCut"] != DBNull.Value ? Convert.ToInt32(dataReader["cantSaltosCut"]) : 0;
                int margen = dataReader["margen"] != DBNull.Value ? Convert.ToInt32(dataReader["margen"]) : 0;
                string margin = "".PadLeft(margen);
                int anchoHoja = dataReader["anchoHoja"] != DBNull.Value ? Convert.ToInt32(dataReader["anchoHoja"]) : 0;

                //string? impresora = ""; // Nombre de la impresora
                //string? hostName = ""; // Nombre del equipo donde esta la impresora

                //string hostName = System.Net.Dns.GetHostName();
                //string hostName = System.Environment.MachineName;

                //string ubicacion = string.Format("\\\\{0}\\{1}", hostName, impresora);

                // Uso este filtro para obtnener los platos de un mismo lugar y sector , mas los combos

                #region Linq Deffered Query  
                var entradas = from md in m.MesaDetM
                               where (md.idSectorExped == idSectorExped && md.comanda && md.esEntrada )
                               select md;
                #endregion

                #region Linq Deffered Query  
                var detalle = from md in m.MesaDetM
                              where (( md.idSectorExped == idSectorExped || md.idTipoConsumo == "CB" ) && md.comanda )
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
                                               ent.idMozo.ToString(), ent.nombreMozo, getComensales(ent.nroMesa, con), ent.fechaHora);

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
                                    if ( (comb.idSectorExped == idSectorExped) && (comb.comanda) )
                                    {
                                        if (!titulo)
                                        {
                                            titulo = true;
                                            Inicializar(fs);
                                            ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), reng.nroMesa.ToString(),
                                                            reng.idMozo.ToString(), reng.nombreMozo, getComensales(reng.nroMesa, con), reng.fechaHora);

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
                                       reng.idMozo.ToString(), reng.nombreMozo, getComensales(reng.nroMesa, con), reng.fechaHora);

                                }
                                if (reng.Combos != null)
                                {
                                    // Impresion de Combos
                                    foreach (var comb in reng.Combos)
                                    {
                                        if ( (comb.idSectorExped == idSectorExped) && (comb.comanda) )
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

        public static void ImprimirComanda2(comanda? c, string? con)
        {
            
            if (c == null)
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
            }
            catch (Exception e)
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
                int idSectorExped = dataReader["idSectorExped"] != DBNull.Value ? Convert.ToInt32(dataReader["idSectorExped"]) : 0;
                int combinaComandas = dataReader["combinacomandas"] != DBNull.Value ? Convert.ToInt32(dataReader["combinacomandas"]) : 0;
                string? iP = dataReader["iP"].ToString();
                int cantSaltos = dataReader["cantSaltos"] != DBNull.Value ? Convert.ToInt32(dataReader["cantSaltos"]) : 0;
                int cantSaltosCut = dataReader["cantSaltosCut"] != DBNull.Value ? Convert.ToInt32(dataReader["cantSaltosCut"]) : 0;
                int margen = dataReader["margen"] != DBNull.Value ? Convert.ToInt32(dataReader["margen"]) : 0;
                string margin = "".PadLeft(margen);
                int anchoHoja = dataReader["anchoHoja"] != DBNull.Value ? Convert.ToInt32(dataReader["anchoHoja"]) : 0;


                #region Linq Deffered Query  
                var entradas = from p in c.platos
                               where (p.idSectorExped == idSectorExped && p.esEntrada)
                               select p;
                #endregion

                #region Linq Deffered Query  
                var detalle = from p in c.platos
                              where (p.idSectorExped == idSectorExped && !p.esEntrada)
                              select p;
                #endregion

                if (detalle.Count() > 0 || entradas.Count() > 0)
                {

                    var titulo = false;
                    var hayEntradas = entradas.Count() > 0;
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
                            // Recorro las entradas
                            foreach (var ent in entradas)
                            {
                                if (!titulo)
                                {
                                    titulo = true;
                                    Inicializar(fs);
                                    ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), c.nroMesa.ToString(),
                                                   c.idMozo.ToString(),c.nombreMozo,c.comensales, c.fechaHora);

                                }
                                if (ent.idTipoConsumo == "CB")
                                {
                                    ImprimirTexto(fs, margin + ent.cant.ToString() + " EN COMBO: " + ent.descripcion + ' ' + ent.tamanio);
                                }
                                else
                                {
                                    ImprimirTexto(fs, margin + ent.cant.ToString() + ' ' + ent.descripcion);
                                    if (ent.tamanio != "")
                                    {
                                        ImprimirTexto(fs, margin + "   " + ent.tamanio);
                                    }
                                }

                                if (ent.obs != null && ent.obs != "")
                                {
                                    ImprimirTexto(fs, margin + "   " + ent.obs);
                                }
                                
                                if (ent.gustos != null)
                                {
                                    foreach (var gusto in ent.gustos)
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

                            // Recorro el detalle
                            foreach (var det in detalle)
                            {
                                if (!titulo)
                                {
                                    titulo = true;
                                    Inicializar(fs);
                                    ImprimirTitulo(fs, margin, dataReader["Descripcion"].ToString(), c.nroMesa.ToString(),
                                                   c.idMozo.ToString(), c.nombreMozo, c.comensales, c.fechaHora);

                                }
                                if (det.idTipoConsumo == "CB")
                                {
                                    ImprimirTexto(fs, margin + det.cant.ToString() + " EN COMBO: " + det.descripcion + ' ' + det.tamanio);
                                }
                                else { 
                                ImprimirTexto(fs, margin + det.cant.ToString() + ' ' + det.descripcion);
                                if (det.tamanio != "")
                                {
                                    ImprimirTexto(fs, margin + "   " + det.tamanio);
                                }
                                }
                                if (det.obs != null && det.obs != "")
                                {
                                    ImprimirTexto(fs, margin + "   " + det.obs);
                                }
                                
                                if (det.gustos != null)
                                {
                                    foreach (var gusto in det.gustos)
                                    {
                                        ImprimirTexto(fs, margin + "   " + gusto.descripcion);
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
                                    var combina = from cb in c.platos
                                                  where ((cb.idSectorExped != idSectorExped) && (cb.idSectorExped != 0))
                                                  select cb;
                                    #endregion
                                    string[] sectComb = [];
                                    List<string> platosComb = [];
                                    if (combina != null)
                                    {
                                        foreach (var comb in combina)
                                        {
                                            if (comb != null)
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
                                        };

                                        for (int i = 0; i < sectComb.Length; i++)
                                        {
                                            Letra_Normal_Normal(fs);
                                            ImprimirTexto(fs, margin + "Combina con: " + sectComb[i]);
                                        }

                                        if (detallarPlatos == 1)
                                        {
                                            string platosconcat = "";
                                            for (int i = 0; i < platosComb.Count; i++)
                                            {
                                                Letra_Normal_Normal(fs);
                                                if (concatenarGustos == 1)
                                                {
                                                    platosconcat = platosconcat + platosComb[i]+",";
                                                }
                                                else
                                                {
                                                    ImprimirTexto(fs, margin + platosComb[i]);
                                                }
                                               
                                            }
                                            if (concatenarGustos == 1)
                                            {
                                                ImprimirTexto(fs, margin + platosconcat);
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

                        } // FIN DEL USING

                    } // Fin del if de impresora
                } // fin del if de impresora
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
                            var centralizada = from md in c.platos
                                               where (md.idSectorExped == idSectorExped)
                                               select md;
                            #endregion
                            Boolean titulosector = false;
                            foreach (var ct in centralizada)
                            {
                                if (!titulo)
                                {
                                    titulo = true;

                                    Inicializar(fs);
                                    ImprimirTitulo(fs, margin, "", c.nroMesa.ToString(),
                                                    c.idMozo.ToString(), c.nombreMozo, c.comensales, c.fechaHora);
                                    SaltarLinea(fs, "1");
                                    Letra_Normal_Normal(fs);
                                }


                                
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
                                    if (ct.impCentralizada == 1)
                                    {
                                        if (ct.idTipoConsumo == "CB")
                                        {
                                            ImprimirTexto(fs, margin + ct.cant.ToString() + " EN COMBO: " + ct.descripcion + ' ' + ct.tamanio);
                                        }
                                        else
                                        {
                                            ImprimirTexto(fs, margin + ct.cant.ToString() + ' ' + ct.descripcion);
                                            if (ct.tamanio != "")
                                            {
                                                ImprimirTexto(fs, margin + "   " + ct.tamanio);
                                            }
                                        }
                                        
                                        if (ct.obs != null && ct.obs != "")
                                        {
                                            ImprimirTexto(fs, margin + "   " + ct.obs);
                                        }
                                       
                                        if (ct.gustos != null)
                                        {
                                            foreach (var gusto in ct.gustos)
                                            {
                                                ImprimirTexto(fs, margin + "   " + gusto.descripcion);
                                            }
                                        }
                                    }
                                          
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

        public static int getComensales(int nroMesa, string? con)
        {
            int comensales = 0;
            SqlConnection conMesa = new(con);
            SqlDataReader dataMesa;
            string sqlMesa = "Select cantPersonas from En_Mesa where nroMesa = " + nroMesa;
            conMesa.Open();
            SqlCommand cmdMesa = new SqlCommand(sqlMesa, conMesa);
            dataMesa = cmdMesa.ExecuteReader();
            dataMesa.Read();
            comensales = (int)dataMesa["cantPersonas"];
            conMesa.Close();

            return comensales;
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
            ImprimirTexto(fs, margin + "Comensales: " + comensales.ToString());
            Letra_Normal_Normal(fs);
            ImprimirTexto(fs, margin + "--------------------------------------------------");
            Letra_Doble_Alto(fs);
        }

        public static void ImprimirMensaje(int idSectorExp,int idImpresora, string? descripcion,int nroMesa, string? nombre, string? con)
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
            int cantSaltos = dataReader["cantSaltos"] != DBNull.Value ? Convert.ToInt32(dataReader["cantSaltos"]) : 0;
            int cantSaltosCut = dataReader["cantSaltosCut"] != DBNull.Value ? Convert.ToInt32(dataReader["cantSaltosCut"]) : 0;
            int margen = dataReader["margen"] != DBNull.Value ? Convert.ToInt32(dataReader["margen"]) : 0;
            string margin = "".PadLeft(margen);
            int anchoHoja = dataReader["anchoHoja"] != DBNull.Value ? Convert.ToInt32(dataReader["anchoHoja"]) : 0;

            
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
                    ImprimirTexto(fs, margin + "Mesa "+nroMesa.ToString() );
                    ImprimirTexto(fs, margin + descripcion);
                    ImprimirTexto(fs, margin);
                    Letra_Normal_Normal(fs);
                    ImprimirTexto(fs, margin + "######################################################");
                    Letra_Doble_Alto(fs);
                    ImprimirTexto(fs, margin + "               FIN MENSAJE");
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

        public static void ImprimirAceptacion(MesaCerrarMozo? m, string? con)
        {
            if (m == null)
            {
                return;
            }
            CultureInfo culture = new("es-AR");
            NumberFormatInfo nfi = new CultureInfo("es-AR", false).NumberFormat;
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
            decimal Propina = dataParamCom["Propina"] != DBNull.Value ? Convert.ToDecimal(dataParamCom["Propina"]) : 0;
            string? PropinaTxt = dataParamCom["PropinaLeyenda"].ToString();
            string? PropinaTxt2 = dataParamCom["PropinaLeyenda2"].ToString();
            bool? PropinaAliasMozo = (bool?)dataParamCom["PropinaAliasMozo"];
            decimal PorcDesc = dataParamCom["PorcDescPagoEfectivo"] != DBNull.Value ? Convert.ToDecimal(dataParamCom["PorcDescPagoEfectivo"]) : 0;
            
            int idImpresora = dataParamCom["idImpresoraTicket"] != DBNull.Value ? Convert.ToInt32(dataParamCom["idImpresoraTicket"]) : 0;

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
            string? aliasMozo = "";

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
                    string sqlDetCon = "Select R.*, M.Nombre,M.NroPager From (Select D.Cant, D.Descripcion, " +
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
                        aliasMozo = dataDetCon["NroPager"].ToString();
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
                        ImprimirTexto(fs, margin + ".................................... $ " + EspaciosIzq(importe.ToString("#.00"), 10));
                        
                    }
                    ImprimirTexto(fs, margin + "--------------------------------------------------");
                    ImprimirTexto(fs, margin + "                           Subtotal: $ " + EspaciosIzq(total.ToString("#.00"), 10));
                    if (pagos != 0)
                    {
                        ImprimirTexto(fs, margin + "                              Pagos: $ " + EspaciosIzq(pagos.ToString("#.00"), 10));
                        total = total - pagos;
                    }
                    else
                    {
                        ImprimirTexto(fs, margin + "                            Descuento:       0.00");
                    }
                    Letra_Doble_Alto(fs);
                    ImprimirTexto(fs, margin + "                 TOTAL: $ " + EspaciosIzq(total.ToString("#.00"), 10));

                    if (PorcDesc != 0)
                    {
                        string porcDesc = EspaciosIzq(PorcDesc.ToString("#.00"),10);
                        ImprimirTexto(fs, margin + "TOTAL PAGO EFVO: ( -" +porcDesc.Trim() + "%): $ " + EspaciosIzq((total - (total * PorcDesc / 100)).ToString("#.00").Trim(), 10));
                    }    
                    
                    if (Propina != 0)
                    {
                        ImprimirTexto(fs, margin + PropinaTxt + ' '+EspaciosIzq(Propina.ToString("#.00"),10).Trim() + " %");
                        ImprimirTexto(fs, margin + "TOTAL con Propina $ "+ EspaciosIzq( (total + (total * Propina / 100)).ToString("#.00").Trim(), 10));
                        if (PropinaTxt2 != "")
                        {
                            ImprimirTexto(fs, margin + PropinaTxt2);
                        }
                        if (PropinaAliasMozo == true)
                        {
                            ImprimirTexto(fs, margin + "Alias: " + aliasMozo);
                        }

                    }
                    
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

        public static JsonResult GrabarMulti(EnMesaDetMult m, string? con, bool imprimir)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                command.Transaction = transaction;


                if (m.MesaDetM != null)
                {
                    try
                    {

                        foreach (var Mdet in m.MesaDetM)
                        {
                            command.Parameters.Clear();

                            // Grabo detalle en MesaDet o PedDet
                            // Si idMozo = 0 es pedido
                            // y en nroMesa viene el idPedido

                            if (Mdet.idMozo == 0)
                            {
                                command.CommandText =
                                "Insert into PedDet (idPedido, idDetalle ,idPlato, cant, pcioUnit, obs, idTamanio, " +
                                " procesado, hora, idUsuario, cocinado, descripcion, fechaHora) " +
                                " VALUES(@nroMesa, @idDetalle, @idPlato, @cant, @pcioUnit, @obs, @idTamanio, " +
                                " @procesado, @hora, @idUsuario, @cocinado,@descripcion, @fechaHora) ";
                            }
                            else
                            {
                                command.CommandText =
                                "Insert into En_MesaDet (nroMesa, idDetalle,idPlato,cant,pcioUnit,importe,obs,idTamanio," +
                                "tamanio, procesado, hora, idMozo, idUsuario, cocinado, esEntrada, descripcion," +
                                "fechaHora, comanda) " +
                                " VALUES(@nroMesa, @idDetalle, @idPlato, @cant, @pcioUnit, @importe, @obs, @idTamanio, " +
                                "@tamanio, @procesado, @hora, @idMozo, @idUsuario, @cocinado, @esEntrada, @descripcion," +
                                "@fechaHora, @comanda) ";
                            }


                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@nroMesa", Value = Mdet.nroMesa });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idDetalle", Value = Mdet.idDetalle });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idPlato", Value = Mdet.idPlato });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@cant", Value = Mdet.cant });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@pcioUnit", Value = Mdet.pcioUnit });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@importe", Value = Mdet.importe });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@obs", Value = Mdet.obs });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idTamanio", Value = Mdet.idTamanio });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@tamanio", Value = Mdet.tamanio });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@procesado", Value = Mdet.procesado });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@hora", Value = Mdet.hora });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idMozo", Value = Mdet.idMozo });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idUsuario", Value = Mdet.idUsuario });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@cocinado", Value = Mdet.cocinado });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@esEntrada", Value = Mdet.esEntrada });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@descripcion", Value = Mdet.descripcion });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@fechaHora", Value = Mdet.fechaHora });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@comanda", Value = Mdet.comanda });

                            command.ExecuteNonQuery();

                            if (Mdet.Gustos != null)
                            {
                                foreach (var Mgus in Mdet.Gustos)
                                {
                                    // Grabo gustos en En_MesaDet_Gustos
                                    // o PedDet_Gustos

                                    if (Mdet.idMozo == 0)
                                    {
                                        command.CommandText =
                                        "Insert into PedDet_Gustos (idPedido, idDetalle, idGusto) " +
                                         "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + Mgus.idGusto + ")";
                                    }
                                    else
                                    {
                                        command.CommandText =
                                      "Insert into En_MesaDet_Gustos (NroMesa, IdDetalle,idGusto, Descripcion) " +
                                      "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + Mgus.idGusto +
                                      ", '" + Mgus.descripcion + "')";
                                    }


                                    command.ExecuteNonQuery();
                                };
                            }

                            if (Mdet.Combos != null)
                            {
                                foreach (var MCom in Mdet.Combos)
                                {
                                    // Grabo combos en En_MesaDet_Combos
                                    // o PedDet_Combos
                                    if (Mdet.idMozo == 0)
                                    {
                                        command.CommandText =
                                        "Insert into PedDet_Combos (idPedido, idDetalle, idSeccion, idPlato, Cant," +
                                        " Procesado, IdTamanio, Obs, Cocinado, FechaHora) " +
                                        "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + MCom.idSeccion + "," +
                                        MCom.idPlato + ", " + MCom.cant + ",'" + MCom.procesado + "'," + MCom.idTamanio +
                                        ",'" + MCom.obs + "','" + MCom.cocinado + "','" + MCom.fechaHora + "' )";
                                    }
                                    else
                                    {
                                        command.CommandText =
                                        "Insert into En_MesaDet_Combos (NroMesa, IdDetalle,idSeccion, idPlato," +
                                        "Cant,Procesado,IdTamanio,Obs,Cocinado,FechaHora,Comanda) " +
                                        "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + MCom.idSeccion + "," +
                                         MCom.idPlato + ", " + MCom.cant + ",'" + MCom.procesado + "'," + MCom.idTamanio +
                                         ",'" + MCom.obs + "','" + MCom.cocinado + "','" + MCom.fechaHora + "','" +
                                         MCom.comanda + "')";
                                    }

                                    command.ExecuteNonQuery();

                                    if (MCom.CombosGustos != null)
                                    {
                                        foreach (var MComGust in MCom.CombosGustos)
                                        {
                                            // Grabo combos gustos en En_MesaDet_Combos_Gustos
                                            // o PedDet_Combos_Gustos
                                            if (Mdet.idMozo == 0)
                                            {
                                                command.CommandText =
                                                "Insert into PedDet_Combos_Gustos (idPedido, idDetalle, idSeccion, idPlato, idGusto) " +
                                                "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + MComGust.idSeccion +
                                                "," + MComGust.idPlato + "," + MComGust.idGusto + ")";
                                            }
                                            else
                                            {
                                                command.CommandText =
                                                "Insert into En_MesaDet_Combos_Gustos (NroMesa, IdDetalle,idSeccion,idPlato,idGusto) " +
                                                "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + MComGust.idSeccion +
                                                "," + MComGust.idPlato + "," + MComGust.idGusto + ")";
                                            }
                                            command.ExecuteNonQuery();
                                        };
                                    }
                                };
                            }

                        }
                        transaction.Commit();
                        if (imprimir)
                        {
                            Imprimir.ImprimirComanda(m, con);
                        }

                        connection.Close();
                        return new JsonResult(new { res = 0, mensaje = "commit" });
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        //Console.WriteLine("  Message: {0}", ex.Message);

                        // Attempt to roll back the transaction.
                        try
                        {
                            transaction.Rollback();
                            connection.Close();
                            return new JsonResult(new { res = 1, mensaje = "rollback: " + ex.Message });
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred
                            // on the server that would cause the rollback to fail, such as
                            // a closed connection.
                            //Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            //Console.WriteLine("  Message: {0}", ex2.Message);
                            connection.Close();
                            return new JsonResult(new { res = -1, mensaje = "error: " + ex2.Message });
                        }
                    }
                }
                connection.Close();
                return new JsonResult(new { res = 0, mensaje = "vacio" });

            }
        }

    }

 }
