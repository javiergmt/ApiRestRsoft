using System;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ApiRestRs
{
    public class Imprimir
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
                       uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
                                  FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);
            
        public static void ImprimirComanda()
        {
            string impresora = "CAJA";
            //string hostName = System.Net.Dns.GetHostName();
            //string hostName = System.Environment.MachineName;
            string hostName = "ASUSJMT"; // Nombre del equipo donde esta la impresora
            string ubicacion = string.Format("\\\\{0}\\{1}", hostName, impresora);
            SafeFileHandle fileHandle = CreateFile(ubicacion, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0 , IntPtr.Zero);
            if (fileHandle.IsInvalid)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            using (FileStream fs = new(fileHandle, FileAccess.Write))
            {
                Inicializar(fs);
                Letra_Normal_Normal(fs);
                ImprimirTexto(fs, "########## INICIO ##########");
                //SaltarLinea(fs, "10");
                Letra_Doble_Alto(fs);
                ImprimirTexto(fs, "Sector: COCINA");
                ImprimirTexto(fs, "Mesa: 10");
                ImprimirTexto(fs, "Mozo: Juan Pablo");
                ImprimirTexto(fs, "25/04/2024 - 17:30:00");
                ImprimirTexto(fs, "----------------------------");

                ImprimirTexto(fs, "1 POLLO AL HORNO");
                ImprimirTexto(fs, "2 PAPAS FRITAS");
                ImprimirTexto(fs, "1 COCA COLA");

                ImprimirTexto(fs, "----------------------------");
                SaltarLinea(fs, "1");
                Letra_Normal_Normal(fs);
                ImprimirTexto(fs, "########## FIN ##########");
                SaltarLinea(fs, "7");
                Corte_Papel(fs);
              
                

                fs.Dispose();

            }
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
