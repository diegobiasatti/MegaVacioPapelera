using System;
using System.Diagnostics;
using System.IO;

namespace MegaVacioPapelera
{
    class Program
    {
        static string BasePath = Properties.Resources.BasePath;
        static string PathLocal = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\";
        static string MsgError;
        static string fecha = DateTime.Now.ToString("s");
        static void Main(string[] args)
        {
            if(File.Exists(PathLocal + "response.txt")) { File.Delete(PathLocal + "response.txt"); }
            Console.WriteLine();
            Console.WriteLine("######## Debe estar Iniciado el MEGAcmd ##########");
            Console.WriteLine();
            Console.WriteLine("Intentando Iniciar el Proceso.");
            Console.WriteLine();
            if (PapeleraConCosas())
            {

                if (BorroPapelera())
                {
                    EscriboLOG("Se Borraron datos de la Papelera.");
                }
                else EscriboLOG("Error no se pudo Borrar. " );
            }
            else EscriboLOG("Papelera Vacia. " + MsgError);
            Console.WriteLine("Fin del Proceso.");
        }
        static private void EscriboLOG(string msg)
        {
            
            StreamWriter sw = new StreamWriter(PathLocal + "LOG.txt", true);
            sw.WriteLine(fecha + " " + msg);
            sw.Close();
        }

        static private bool BorroPapelera()
        {
            try
            {
                Process borro = EjecutoProceso(BasePath, "rm -r -f //bin/* -o response.txt");
                borro.Start();
                borro.WaitForExit();

                StreamReader LeoResponse = new StreamReader(PathLocal + "response.txt");
                string response = LeoResponse.ReadToEnd();
                LeoResponse.Close();
                if (response == "")
                {
                    return true;
                }
                else return false;
            }
            catch(Exception e)
            {
                EscriboLOG(e.Message);
                return false;
            }
            

        }
        static private bool PapeleraConCosas()
        {
            try
            {
                Process compruebo = EjecutoProceso(BasePath, @"ls //bin/");
                compruebo.Start();
                compruebo.WaitForExit();

                string line = compruebo.StandardOutput.ReadToEnd();
                Debug.WriteLine(line);
                if(line != null)
                {
                    if (line.Length >=1)
                    {
                        return true;
                    }
                    else
                    {
                        MsgError = "No se encontraron Archivos a Borrar";
                        return false;
                    }
                }
                else
                {
                    MsgError = "No se encontraron Archivos a Borrar";
                    return false;
                }

            }
            catch(Exception e)
            {
                EscriboLOG(e.Message);
                return false;
            }
        }

        static private Process EjecutoProceso(string Programa, string Argumentos)
        {
            Process proceso = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Programa,
                    Arguments = Argumentos,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }

            };
            return proceso;
        }
    }
}
