using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public static void GuardaEdi(string Message,string albaran)
        {
            string path = Models.clsUtils.GlobalVariables.RutaEDI;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = path + "\\" + albaran +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
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

      




    }
}
