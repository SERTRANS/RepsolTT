
using ComunesRedux;
using System;
using System.Collections;

namespace RepsolTT
{
    class Program
    {

   

        static void Main(string[] args)
        {
 

            Utils.CargaVariables();


            //ApiClases.ConsigueAlbaranes();Environment.Exit(0);

            // ApiClases.ConsigueUnSoloAlbaran(); Environment.Exit(0);


            
 

            //if (args.Length==0)
            //{ 
            //    ApiClases.ConsigueAlbaranes();Environment.Exit(0);
            //}

            if (args.Length > 0)                {                    
                    foreach (Object argumento in args)
                    {
                        string argument = argumento.ToString();
                        switch (argument.ToString().ToUpper())
                        {
                            case "NUEVAS_RUTAS":
                                ApiClases.ConsigueAlbaranes();
                            Environment.Exit(0);
                            break;
                            case "UNA_RUTA":
                                ApiClases.ConsigueUnSoloAlbaran();
                            Environment.Exit(0);
                            break;
                            case "ACTUALIZAR_FLAG":
                                ApiClases.Validacion();
                            Environment.Exit(0);
                            break;
                            case "INFO_RUTA":
                                ApiClases.InfoRuta();
                            Environment.Exit(0);
                            Console.ReadKey();
                                break;

                        }
                    }
                } 

                string[] MiMenu = new string[] { "NUEVAS_RUTAS", "UNA_RUTA", "ACTUALIZAR_FLAG", "INFO_RUTA","SALIR" };
                string devMenu = Utils.Menu(MiMenu);

                if (devMenu == "SALIR") Environment.Exit(0);


                switch (devMenu)
                //switch (Console.Read())
                {
                    case "NUEVAS_RUTAS":
                        ApiClases.ConsigueAlbaranes();
                        break;
                    case "UNA_RUTA":
                        ApiClases.ConsigueUnSoloAlbaran();
                        break;
                    case "ACTUALIZAR_FLAG":
                        ApiClases.Validacion();
                        break;
                    case "INFO_RUTA":
                        ApiClases.InfoRuta();
                        break;
                }

                Console.WriteLine("PULSA TECLA PARA SALIDA");
                Console.ReadKey();
                Environment.Exit(0);
             
            
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
    



    }
}
