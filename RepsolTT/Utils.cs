using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepsolTT
{
    class Utils
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static  bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }


        public static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\"+ projectName + "_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        public static void WriteToFileTMP(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\listado" + projectName + "_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        public static string Menu(string[] MiMenu)
        {
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleListBox.BreakHandler);
            Console.Clear();
            Console.CursorVisible = false; 

            ConsoleListBox.WriteColorString("Elegir Opcion de Menu", 12, 20, ConsoleColor.Black, ConsoleColor.White);

            int choice = ConsoleListBox.ChooseListBoxItem(MiMenu, 34, 3, ConsoleColor.Blue, ConsoleColor.White);
            // do something with choice
            //ConsoleListBox.WriteColorString("You chose " + MiMenu[choice - 1] + ". Press any key to exit", 21, 22, ConsoleColor.Black, ConsoleColor.White);
            //Console.ReadKey();
            ConsoleListBox.CleanUp();
            return MiMenu[choice - 1];

        }
        public static void WriteToFileFallo(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\Fallos_" + projectName + "_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public static void GuardaEdi(string Message,string albaran,string departamento)
        {
            string path = Models.clsUtils.GlobalVariables.RutaEDI;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = path + "\\" + departamento +"_" + albaran +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }


        public static DateTime FromUnixTime(  long unixTime)
        {

            DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            DateTime fecha = dateTimeOffSet.DateTime;

            return fecha;

           
        }

      
        public static string DimeDepartamento(string codPostal, Int32 kg, string Pais)
        {
            if (Pais != "ES")
                return "21";

            if ("43170825".Contains(codPostal.Substring(0, 2)) && kg < 17000 && Pais == "ES")            
                return "4";
            
            if ("43170825".Contains(codPostal.Substring(0, 2)) && kg > 17000 && Pais == "ES")
                return "8";

            if (!"43170825".Contains(codPostal.Substring(0, 2)) && Pais == "ES")
                return "23";

            return "8";

        }


        public static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return System.Text.Json.JsonSerializer.Serialize(jsonElement, options);
        }

        public static void CargaVariables()
        {

            Models.clsUtils.GlobalVariables.Api = System.Configuration.ConfigurationManager.AppSettings["Api"].ToString();
            Models.clsUtils.GlobalVariables.API_KEY = System.Configuration.ConfigurationManager.AppSettings["API_KEY"].ToString();
            Models.clsUtils.GlobalVariables.Usuario = System.Configuration.ConfigurationManager.AppSettings["Usuario"].ToString();
            Models.clsUtils.GlobalVariables.Password = Utils.Base64Decode(System.Configuration.ConfigurationManager.AppSettings["Password"].ToString());
            Models.clsUtils.GlobalVariables.RutaEDI = System.Configuration.ConfigurationManager.AppSettings["RutaEDI"].ToString();

        }
        public static bool IsNull(  object source)
        {
            return source == null;
        }

    }
}
