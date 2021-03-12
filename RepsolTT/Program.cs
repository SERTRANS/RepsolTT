using ComunesRedux;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RepsolTT
{
    class Program
    {
        static void Main(string[] args)
        {



     //       DateTime m = Utils.FromUnixTime(1604656800000);

            CargaVariables();
        

            string token;
            token = ApiClases.DameToken();


            if (token != "")
            {

                

//                Models.clsUtils.ProtocoloEdi DameRutas = new Models.clsUtils.ProtocoloEdi();
                List<string> listaAlbaranes = new List<string>();
                listaAlbaranes= ApiClases.DameTodasRutas(token);
                ApiClases.GeneraEdi(token, listaAlbaranes);
            }
            else

            {
                Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");

            }


            //if (token != "")
            //{

            //    Models.clsUtils.ChoferMatricula choferMatricula = new Models.clsUtils.ChoferMatricula();
            //    choferMatricula.driver_id = "FK826185";
            //    choferMatricula.driver_name = "MAKSYM YARMOLENKO";
            //    choferMatricula.driver_phone_number = "649882999";
            //    choferMatricula.registration_one = "KCH-04609";
            //    choferMatricula.registration_two = "R-4508-BCS";
            //    choferMatricula.assign_timestamp = 1;


            //    ApiClases.AsignaChoferMatricula(token,choferMatricula, "24924297");
            //}
            //else

            //{
            //    Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");

            //}
          


        }
        public static void CargaVariables()
        {

            Models.clsUtils.GlobalVariables.Api = System.Configuration.ConfigurationManager.AppSettings["Api"].ToString();
            Models.clsUtils.GlobalVariables.API_KEY = System.Configuration.ConfigurationManager.AppSettings["API_KEY"].ToString();
            Models.clsUtils.GlobalVariables.Usuario = System.Configuration.ConfigurationManager.AppSettings["Usuario"].ToString();
            Models.clsUtils.GlobalVariables.Password =  Utils.Base64Decode( System.Configuration.ConfigurationManager.AppSettings["Password"].ToString());
            Models.clsUtils.GlobalVariables.RutaEDI = System.Configuration.ConfigurationManager.AppSettings["RutaEDI"].ToString();
            
        }



    }
}
