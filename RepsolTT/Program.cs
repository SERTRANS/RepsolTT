 
using System;
 

namespace RepsolTT
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.CargaVariables();         

                if (args.Length > 0)
                {
                    Console.WriteLine("PARAMETROS");
                    foreach (Object argumento in args)
                    {
                        string argument = argumento.ToString();
                        switch (argument.ToString().ToUpper())
                        {
                            case "GENERAR_NUEVAS_RUTAS":
                                ApiClases.generaRutas();
                                break;
                            case "GENERAR_FICHERO":
                                ApiClases.crearRutas();
                                break;
                            case "ACTUALIZAR_FLAG":
                                ApiClases.Validacion();
                                break;
                            case "INFO_RUTA":
                                ApiClases.InfoRuta();
                                break;
                        }
                    }
                } 


                string[] MiMenu = new string[] { "GENERAR_NUEVAS_RUTAS", "GENERAR_FICHERO", "ACTUALIZAR_FLAG", "INFO_RUTA","SALIR" };

                string devMenu = Utils.Menu(MiMenu);

                if (devMenu == "SALIR") Environment.Exit(0);


                switch (devMenu)
                //switch (Console.Read())
                {
                    case "GENERAR_NUEVAS_RUTAS":
                        ApiClases.generaRutas();
                        break;
                    case "GENERAR_FICHERO":
                        ApiClases.crearRutas();
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
