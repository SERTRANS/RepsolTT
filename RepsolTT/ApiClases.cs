using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RepsolTT
{
    class ApiClases
    {
        public static string EncrptaPassword()
        {
            try
            {
                string PaswwordAEncriptar = Models.clsUtils.GlobalVariables.Password;
                var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/utils/encrypt?data=" + PaswwordAEncriptar);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("API-KEY", Models.clsUtils.GlobalVariables.API_KEY);
                IRestResponse response = client.Execute(request);
//                Console.WriteLine(response.Content);
                Models.clsUtils.retornoApi vRetApi = new Models.clsUtils.retornoApi();
                vRetApi = JsonConvert.DeserializeObject<Models.clsUtils.retornoApi>(response.Content);
                if (vRetApi.e_return.type == "S")
                    return vRetApi.e_return.code;
                else
                    return "";

            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string DameToken()
        {

            try
            {
                String PasswordEncriptado = EncrptaPassword();
                if (PasswordEncriptado != "")
                {

                    var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/auth/login");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("API-KEY", Models.clsUtils.GlobalVariables.API_KEY);
                    request.AddHeader("Content-Type", "application/json");

                    // string Body = "{\"user\": \"sertrans_int\",\"pass\": \"" + PasswordEncriptado + "\",\"lang_id\": 1}";
                    Models.clsUtils.Credenciales vCredenciales = new Models.clsUtils.Credenciales();
                    vCredenciales.user = "sertrans_int";
                    vCredenciales.pass = PasswordEncriptado;
                    vCredenciales.language_id = 1;

                    string Body = Newtonsoft.Json.JsonConvert.SerializeObject(vCredenciales);

                    request.AddParameter("application/json", Body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    //Console.WriteLine(response.Content);

                    Models.clsUtils.retornoApi vRetApi = new Models.clsUtils.retornoApi();
                    vRetApi = JsonConvert.DeserializeObject<Models.clsUtils.retornoApi>(response.Content);
                    if (vRetApi.e_return.type == "S")
                        return vRetApi.e_return.code;
                    else
                        return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                return "";

            } 

        }
         
        //public static void crearUnaSolaRuta()
        //{
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.Write("ALBARAN(ES) A INTEGRAR (SEPARADOS POR COMAS SI HAY MAS DE UNO):");
        //    string albaran = Console.ReadLine();


        //    string token;
        //    token = ApiClases.DameToken();

        //    if (token != "")
        //    {
        //        List<string> listaAlbaranes = new List<string>();
        //        string[] array = albaran.Split(',');
        //        foreach (string value in array)
        //        {
        //            listaAlbaranes.Add(value);
        //        }
        //            ApiClases.GeneraEdi(token, listaAlbaranes);
        //    }
        //    else
        //    {
        //        Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");

        //    }



        //}

        //public static void generaRutas()
        //{   

        //    string token;
        //    token = ApiClases.DameToken();

        //    if (token != "")
        //    {
        //        Console.WriteLine(DateTime.Now.ToString(), " RECOGIENDO TOKEN:" + token);
        //        List<string> listaAlbaranes = new List<string>();
        //        listaAlbaranes = ApiClases.DameTodasRutas(token);
        //        ApiClases.GeneraEdi(token, listaAlbaranes);
        //    }
        //    else

        //    {
        //        Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");
        //        Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN","ERROR CONGUIENDO TOKEN");
        //    }
        //}

        public static Models.clsUtils.DatosRuta DameDatosRuta(string token, string Albaran)
        {
            Models.clsUtils.DatosRuta vRetApi = null;
            try
            {
                var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/route?routeCode=" + Albaran);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("fieldeas-token", token);
                request.AddHeader("API-KEY", Models.clsUtils.GlobalVariables.API_KEY);
                IRestResponse response = client.Execute(request);
           //     Console.WriteLine(response.Content);
                vRetApi = new Models.clsUtils.DatosRuta();
                if (response.Content.Contains("e_return"))
                {

                    vRetApi.route_code = "NOEXISTE";
                    Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL DameDatosRutas", "No existe el albaran en la api");
                    //     Utils.WriteToFile(DateTime.Now.ToString() + ";" + Albaran + ";" + response.Content);
                         Utils.WriteToFile(DateTime.Now.ToString() + ";" + Albaran + ";" + response.Content);
                }
                else
                {
                    vRetApi = JsonConvert.DeserializeObject<Models.clsUtils.DatosRuta>(response.Content);
                }
                return vRetApi;
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL DameDatosRutas", ex.Message );
                vRetApi.route_code = "";
            }
            return vRetApi;
        }
        public static string AsignaChoferMatricula(string token, Models.clsUtils.ChoferMatricula choferMatricula, string albaran)
        {
            try
            {
                var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/route/assigns?routeCode=" + albaran);
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("fieldeas-token", token);
                request.AddHeader("Content-Type", "application/json");

                string Body = Newtonsoft.Json.JsonConvert.SerializeObject(choferMatricula);
                request.AddParameter("application/json", Body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);
                Models.clsUtils.retornoApi vRetApi = new Models.clsUtils.retornoApi();
                vRetApi = JsonConvert.DeserializeObject<Models.clsUtils.retornoApi>(response.Content);
                if (vRetApi.e_return.type == "S")
                    return vRetApi.e_return.message;
                else
                    return "";
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL AsignaChoferMatricula", ex.Message);
                return "";

            }

        }

        public static void Validacion ()
        {

            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("ALBARAN(ES) A DAR DE BAJA (SEPARADOS POR COMAS SI HAY MAS DE UNO):");
            string albaran = Console.ReadLine();

            string token;
            token = ApiClases.DameToken();
            Validar(token, albaran );
        }

        public static void Validar(string token,string IdRutas)
        {
            List<Int64> ids = new List<Int64>();

            string[] array = IdRutas.Split(',');

            foreach (string value in array)
            {
                ids.Add(Convert.ToInt64( value.Trim()));
            }

            validarRutaBajada(token, ids);
        }

        public static void validarRutaBajada(string token,List<Int64> idrutaLista)
        {

            try
            {

                foreach (Int64 item in idrutaLista)
                {
                    Int64 idruta = item;

                        var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/entities/RUTAS/rows");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.PUT);
                        request.AddHeader("fieldeas-token", token);
                        request.AddHeader("Content-Type", "application/json");

                        #region Body
                        string Body = "{" +
                                           "      \"data\": [ " +
                                           "         [" +
                                           "             {  " +
                                           "                 \"key\": \"ID_RUTA\",  " +
                                           "                 \"value\": " + idruta + "  " +
                                           "             },  " +
                                           "             {  " +
                                           "                 \"key\": \"FLAG_1\",  " +
                                           "                 \"value\": 0  " +
                                           "             }  " +
                                           "         ] " +
                                           "     ] " +
                                           " }";
                        #endregion



                        request.AddParameter("application/json", Body, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        //Console.WriteLine(response.Content);
                        Models.clsUtils.retornoApi vRetApi = new Models.clsUtils.retornoApi();
                        vRetApi = JsonConvert.DeserializeObject<Models.clsUtils.retornoApi>(response.Content);
                }
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL ValidarRutaBajada", ex.Message);
            }


        }

        public static List<string> DameTodasRutas(string token)
        {
            try
            {

                var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/entities/RUTAS/query?offset=0&limit=100");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("fieldeas-token", token);
                request.AddHeader("Content-Type", "application/json");

                #region Body
                string Body = "{" +
                                   "     \"filter\": { " +
                                   "         \"Clauses\": [{ " +
                                   "                 \"Field\": \"FLAG_1\", " +
                                   "             \"Value\": \"1\", " +
                                   "             \"Operator\": 2 " +
                                   "         },{ " +
                                   "                 \"Field\": \"ID_AGENCIA\", " +
                                   "             \"Value\": \"317434\", " +
                                   "             \"Operator\": 2 " +
                                   "           }], " +
                                   "         \"LogicFiltersOperators\": 0 " +
                                   "     } " +
                                   "     }";
                #endregion

                request.AddParameter("application/json", Body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);

                string json = response.Content;
                json = json.Replace("{\"table\":\"RUTAS\",\"data\":[[", ""); json = json.Replace(",", "," + Environment.NewLine); json = json.Replace("]]}", ","); json = json.Replace("}", ""); json = json.Replace("{", "");

                string[] array = json.Split(
                                new[] { Environment.NewLine },
                                StringSplitOptions.None
                                 );


                List<string> listaAlbaranes = new List<string>();
                
                string ruta=string.Empty;
                string idruta = string.Empty;

                for (int i = 0; i < array.Length; i++)
                {

                    if (array[i].Contains("ID_RUTA"))
                    {
                        i++;
                        idruta = Limpia(array[i]);
                    }


                    if (array[i].Contains("CODIGO_RUTA"))
                    {
                        i++;
                        listaAlbaranes.Add(Limpia(array[i]));
                        ruta = Limpia(array[i]);
                    }


                    if (array[i].Contains("FH_INICIO_PLANIFICADA_CERCANA"))
                    {
                        i++;
                        DateTime f = Convert.ToDateTime(Limpia(array[i]));
                        Utils.WriteToFileTMP(idruta + "," + ruta + "," + f.ToString());
                    }

                }

                return listaAlbaranes;

            }
            catch (Exception ex)
            {
                //Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                Utils.EnviarMailErrores(DateTime.Now.ToString() + " ERROR Repsol", ex.Message);
                return null;

            }
        }

        public static void InfoRuta()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("ALBARAN:");
            string albaran = Console.ReadLine();
            string token;
            token = ApiClases.DameToken();

            Models.clsUtils.DatosRuta datos = new Models.clsUtils.DatosRuta();
            datos=DameDatosRuta(token, albaran);
             
            string jsonData = JsonConvert.SerializeObject(datos);

            jsonData = Utils.PrettyJson(jsonData);
            Console.WriteLine(jsonData);
        }

    //public static List<int> NumeroCargasDescargas (Models.clsUtils.ProtocoloEdi edi)
    //    {


    //        try
    //        {

    //            int Cargas = 0;
    //            int DesCargas = 0;
    //            string albaran = string.Empty;

    //            Models.clsUtils.ProtocoloEdiDatos ediDatos = new Models.clsUtils.ProtocoloEdiDatos();

    //            foreach (Models.clsUtils.ProtocoloEdiDatos lista in edi.ProtocoloEdiDatosLista)
    //            {
    //                if (lista.CargaDescarga.ToUpper() == "C") Cargas += 1;
    //                if (lista.CargaDescarga.ToUpper() == "D") DesCargas += 1;
    //                albaran = lista.AlbaranOrdenante;

    //            }

    //            List<int> dev = new List<int>();
    //            dev.Add(Cargas);
    //            dev.Add(DesCargas);
    //            //Utils.WriteToFile(albaran.ToString().Trim() + ";" +  Cargas.ToString() + ";" + DesCargas.ToString());
    //            return dev;
    //        }
    //        catch (Exception ex)
    //        {
    //            Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
    //            Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL NumeroCargasDescargas", ex.Message);
    //            return null;
    //        }

    //    }


    //    public static void GeneraEdi(string token ,List<string> listaAlbaranes)
    //    {
    //        string ficheroCadena=string.Empty;
    //        try
    //        {
    //            foreach (string item in listaAlbaranes)
    //            {
    //                Models.clsUtils.ProtocoloEdi edi = new Models.clsUtils.ProtocoloEdi();

    //                edi = infoAlbaranes(token, item);
    //                if (edi != null)
    //                {
    //                    List<int> NCargasDescargas = new List<int>();
    //                    NCargasDescargas = NumeroCargasDescargas(edi);
    //                    if (NCargasDescargas != null)
    //                    {
    //                        if (NCargasDescargas[0] == 1 && NCargasDescargas[1] == 1)
    //                        {

    //                            ficheroCadena = unoAuno(token,edi);
    //                            if (ficheroCadena != "")
    //                                Utils.WriteToFile(DateTime.Now.ToString() + ";" + ficheroCadena);                                    

    //                        }
    //                        //Dos cargas un destino
    //                        else
    //                            if (NCargasDescargas[0] == 2 && NCargasDescargas[1] == 1)
    //                        {

    //                            ficheroCadena = dosAuno(token,edi);
    //                            //  Utils.GuardaEdi(ficheroCadena, item.Trim());
    //                            if (ficheroCadena != "")
    //                                Utils.WriteToFile(DateTime.Now.ToString() + ";" + ficheroCadena);
    //                        }
    //                        else
    //                        {
    //                            Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + "Mas cargas que las previstas" + ficheroCadena);
    //                            Utils.EnviarMailErrores(DateTime.Now.ToString() + " ERROR REPSOL", "Mas cargas que las previstas " + item);
    //                        }
    //                    }

    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
    //            Utils.EnviarMailErrores(DateTime.Now.ToString() + " ERROR REPSOL GENERA EDI", ex.Message);
    //        }             
         
    //}

        //public static string  generaEdiCadena(Models.clsUtils.ProtocoloEdiDatos edi)
        //{

        //    try
        //    {
        //        string linea;
        //        linea = edi.AlbaranOrdenante.Trim() + ";" +
        //                 edi.FechaSalidaExpedicion + ";" +
        //                 edi.HoraAcordadaSalida + ";" +
        //                 edi.FechaEntregaExpedicion + ";" +
        //                 edi.HoraAcordadaEntrega + ";" +
        //                 edi.NombreRemitente.Trim() + ";" +
        //                 edi.DomicilioRemitente.Trim() + ";" +
        //                 edi.PoblacionRemitente.Trim() + ";" +
        //                 edi.CodigoPostalRemitente.Trim() + ";" +
        //                 edi.CodigoPaisRemitente.Trim() + ";" +
        //                 edi.NombreDestinatario.Trim() + ";" +
        //                 edi.DomicilioDestinatario.Trim() + ";" +
        //                 edi.PoblacionDestinatario.Trim() + ";" +
        //                 edi.CodigoPostalDestinatario.Trim() + ";" +
        //                 edi.CodigoPaisDestinatario.Trim() + ";" +
        //                 edi.BaremoPalets + ";" +
        //                 edi.BaremoPesoBruto + ";" +
        //                 edi.BaremoVolumen + ";" +
        //                 edi.BaremoBultos + ";" +
        //                 "0" + ";" +
        //                 "" + ";" +
        //                 "" + ";" +
        //                 edi.OrderId
        //                ;

        //        return linea;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
        //        Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL GeneraEdiCadena", ex.Message);
        //        return "";

        //    }

        //}

        //public static string generaEdiCadenaLocal(Models.clsUtils.ProtocoloEdiDatos edi,int lineaEntrega,string tipus,int Kg)
        //{

        //    try
        //    {
        //        int kils=0;
        //        int lineaEntrega2 = lineaEntrega + 1;

        //        if (lineaEntrega2 == 2 && tipus=="unoAuno")
        //                lineaEntrega2 = 999;

        //        if (lineaEntrega2 == 4 )
        //        {
                
        //            lineaEntrega2 = 999;
                    
        //            if (Kg>0)
        //            {
        //                kils = Kg;
        //            }
        //            else
        //            {
        //                kils = Convert.ToInt32  (edi.BaremoPesoBruto);
        //            }
                    
                    
        //        }
        //        else
        //        {
        //            kils = Convert.ToInt32(edi.BaremoPesoBruto.Replace(".0",""));
        //        }


        //        string linea1;


        //        linea1 = edi.AlbaranOrdenante.Trim() + ";" +
        //                 "2" + ";" +
        //                 lineaEntrega.ToString() + ";" +
        //                 edi.FechaSalidaExpedicion + ";" +
        //                 "00:00" + ";" +
        //                 edi.NombreRemitente.Trim() + ";" +
        //                 edi.DomicilioRemitente.Trim() + ";" +
        //                 edi.PoblacionRemitente.Trim() + ";" +
        //                 edi.CodigoPostalRemitente.Trim() + ";" +
        //                 edi.CodigoPaisRemitente.Trim() + ";" +
        //                 edi.BaremoPalets + ";" +
        //                 edi.BaremoPesoBruto + ";" +
        //                 edi.BaremoVolumen + ";" +
        //                 edi.BaremoBultos + ";" +
        //                 edi.OrderId
        //                 ;

                         
        //            string linea2;
        //        linea2= edi.AlbaranOrdenante.Trim() + ";" +
        //                 "0" + ";" +
        //                 lineaEntrega2.ToString() + ";" +
        //                 edi.FechaEntregaExpedicion + ";" +
        //                 "00:00" + ";" +
        //                 edi.NombreDestinatario.Trim() + ";" +
        //                 edi.DomicilioDestinatario.Trim() + ";" +
        //                 edi.PoblacionDestinatario.Trim() + ";" +
        //                 edi.CodigoPostalDestinatario.Trim() + ";" +
        //                 edi.CodigoPaisDestinatario.Trim() + ";" +
        //                 edi.BaremoPalets + ";" +
        //                 kils.ToString() + ";" +
        //                 edi.BaremoVolumen + ";" +
        //                 edi.BaremoBultos + ";" +
        //                 edi.OrderId
        //                ;
        //        string linea3;

        //        if (lineaEntrega == 1 && tipus=="dosAuno")
        //            linea3 = linea1;
        //        else

        //            linea3 =linea1 + Environment.NewLine + linea2;

        //        return linea3;

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
        //        Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL GeneraEdiCadena", ex.Message);
        //        return "";

        //    }

        //}




        //public static void generaEdi(Models.clsUtils.ProtocoloEdiDatos edi)
        //{
        //    try
        //    {


        //        string linea;
        //        linea = edi.AlbaranOrdenante.Trim() + ";" +
        //                 edi.FechaSalidaExpedicion + ";" +
        //                 edi.HoraAcordadaSalida + ";" +
        //                 edi.FechaEntregaExpedicion + ";" +
        //                 edi.HoraAcordadaEntrega + ";" +
        //                 edi.NombreRemitente.Trim() + ";" +
        //                 edi.DomicilioRemitente.Trim() + ";" +
        //                 edi.PoblacionRemitente.Trim() + ";" +
        //                 edi.CodigoPostalRemitente.Trim() + ";" +
        //                 edi.CodigoPaisRemitente.Trim() + ";" +
        //                 edi.NombreDestinatario.Trim() + ";" +
        //                 edi.DomicilioDestinatario.Trim() + ";" +
        //                 edi.PoblacionDestinatario.Trim() + ";" +
        //                 edi.CodigoPostalDestinatario.Trim() + ";" +
        //                 edi.CodigoPaisDestinatario.Trim() + ";" +
        //                 edi.BaremoPalets + ";" +
        //                 edi.BaremoPesoBruto + ";" +
        //                 edi.BaremoVolumen + ";" +
        //                 edi.BaremoBultos + ";" +
        //                 "0" + ";" +
        //                 "" + ";" +
        //                 "" + ";"
        //                ;

        //        //         Utils.GuardaEdi(linea, edi.AlbaranOrdenante.Trim());

        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
        //    }

        //}

        //public static string  dosAuno(string token,Models.clsUtils.ProtocoloEdi edi)
        //{
        //    try
        //    {

        //        Models.clsUtils.ProtocoloEdiDatos carga1 = new Models.clsUtils.ProtocoloEdiDatos();
        //        Models.clsUtils.ProtocoloEdiDatos carga2 = new Models.clsUtils.ProtocoloEdiDatos();
        //        Models.clsUtils.ProtocoloEdiDatos Descarga = new Models.clsUtils.ProtocoloEdiDatos();
        //        Models.clsUtils.ProtocoloEdiDatos Tot = new Models.clsUtils.ProtocoloEdiDatos();

                
        //        string ficheroCadena = "";
        //        string ficheroCadenaLLocal = "";


        //        if (edi.ProtocoloEdiDatosLista[0].CargaDescarga == "D")
        //        {
        //            carga1 = edi.ProtocoloEdiDatosLista[1];
        //            carga2 = edi.ProtocoloEdiDatosLista[2];
        //            Descarga = edi.ProtocoloEdiDatosLista[0];
        //        }

        //        if (edi.ProtocoloEdiDatosLista[1].CargaDescarga == "D")
        //        {
        //            carga1 = edi.ProtocoloEdiDatosLista[0];
        //            carga2 = edi.ProtocoloEdiDatosLista[2];
        //            Descarga = edi.ProtocoloEdiDatosLista[1];
        //        }

        //        if (edi.ProtocoloEdiDatosLista[2].CargaDescarga == "D")
        //        {
        //            carga1 = edi.ProtocoloEdiDatosLista[0];
        //            carga2 = edi.ProtocoloEdiDatosLista[1];
        //            Descarga = edi.ProtocoloEdiDatosLista[2];
        //        }


        //        //Tot.AlbaranOrdenante = edi.AlbaranOrdenante.Trim();
        //        Tot.AlbaranOrdenante = carga1.AlbaranOrdenante + "-" + carga2.AlbaranOrdenante;
        //        Tot.FechaSalidaExpedicion = carga1.FechaSalidaExpedicion;
        //        Tot.HoraAcordadaSalida = carga1.HoraAcordadaSalida;
        //        Tot.FechaEntregaExpedicion = Descarga.FechaEntregaExpedicion;
        //        Tot.HoraAcordadaEntrega = Descarga.HoraAcordadaEntrega;

        //        Tot.NombreRemitente = carga1.NombreRemitente;
        //        Tot.DomicilioRemitente = carga1.DomicilioRemitente;
        //        Tot.PoblacionRemitente = carga1.PoblacionRemitente;
        //        Tot.CodigoPostalRemitente = carga1.CodigoPostalRemitente;
        //        Tot.CodigoPaisRemitente = carga1.CodigoPaisRemitente;

        //        Tot.NombreDestinatario = Descarga.NombreDestinatario;
        //        Tot.DomicilioDestinatario = Descarga.DomicilioDestinatario;
        //        Tot.PoblacionDestinatario = Descarga.PoblacionDestinatario;
        //        Tot.CodigoPostalDestinatario = Descarga.CodigoPostalDestinatario;
        //        Tot.CodigoPaisDestinatario = Descarga.CodigoPaisDestinatario;

        //        Tot.BaremoPalets = carga1.BaremoPalets;
        //        Tot.BaremoPesoBruto = carga1.BaremoPesoBruto.Replace(".0", "");
        //        Tot.BaremoVolumen = carga1.BaremoVolumen;
        //        Tot.BaremoBultos = carga1.BaremoBultos;
        //        Tot.OrderId = carga1.OrderId;


        //        ficheroCadena = generaEdiCadena(Tot);
        //        ficheroCadenaLLocal = generaEdiCadenaLocal(Tot,1, "dosAuno",0);


        //        Tot.AlbaranOrdenante = carga1.AlbaranOrdenante + "-" + carga2.AlbaranOrdenante;
        //        Tot.FechaSalidaExpedicion = carga2.FechaSalidaExpedicion;
        //        Tot.HoraAcordadaSalida = carga2.HoraAcordadaSalida;
        //        Tot.FechaEntregaExpedicion = Descarga.FechaEntregaExpedicion;
        //        Tot.HoraAcordadaEntrega = Descarga.HoraAcordadaEntrega;

        //        Tot.NombreRemitente = carga2.NombreRemitente;
        //        Tot.DomicilioRemitente = carga2.DomicilioRemitente;
        //        Tot.PoblacionRemitente = carga2.PoblacionRemitente;
        //        Tot.CodigoPostalRemitente = carga2.CodigoPostalRemitente;
        //        Tot.CodigoPaisRemitente = carga2.CodigoPaisRemitente;

        //        Tot.NombreDestinatario = Descarga.NombreDestinatario;
        //        Tot.DomicilioDestinatario = Descarga.DomicilioDestinatario;
        //        Tot.PoblacionDestinatario = Descarga.PoblacionDestinatario;
        //        Tot.CodigoPostalDestinatario = Descarga.CodigoPostalDestinatario;
        //        Tot.CodigoPaisDestinatario = Descarga.CodigoPaisDestinatario;

        //        Tot.BaremoPalets = carga2.BaremoPalets;
        //        Tot.BaremoPesoBruto = carga2.BaremoPesoBruto.Replace(".0", "");
        //        Tot.BaremoVolumen = carga2.BaremoVolumen;
        //        Tot.BaremoBultos = carga2.BaremoBultos;
        //        Tot.OrderId = carga2.OrderId;

        //        Tot.idRuta = carga1.idRuta;

        //        Int32 kgCarga1;
        //        Int32 kgCarga2;

        //        if (Utils.IsNumeric(carga1.BaremoPesoBruto.Replace(".0", "")))                
        //            kgCarga1 = Convert.ToInt32(carga1.BaremoPesoBruto.Replace(".0", ""));
        //        else
        //                kgCarga1 = 0;

        //        if (Utils.IsNumeric(carga2.BaremoPesoBruto.Replace(".0", "")))
        //            kgCarga2 = Convert.ToInt32(carga2.BaremoPesoBruto.Replace(".0", ""));
        //        else
        //            kgCarga2 = 0;


        //        Int32 kg;
        //        kg = kgCarga1 + kgCarga2;

                
        //        Tot.Departamento = Utils.DimeDepartamento(Descarga.CodigoPostalDestinatario, kg, Descarga.CodigoPaisDestinatario);
        //        if (Tot.Departamento == "4")
        //            Utils.EnviarMailErrores("REPSOL  Pedido paqueteria con mas de una carga", Tot.AlbaranOrdenante);


        //        if (Tot.Departamento == "8")
        //        {
        //            ficheroCadenaLLocal = ficheroCadenaLLocal + Environment.NewLine + generaEdiCadenaLocal(Tot,3, "dosAuno", kg);

        //            Utils.GuardaEdi(ficheroCadenaLLocal, Tot.AlbaranOrdenante, Tot.Departamento);
        //        }
        //        else
        //        {
        //            ficheroCadena = ficheroCadena + Environment.NewLine + generaEdiCadena(Tot);

        //            Utils.GuardaEdi(ficheroCadena, Tot.AlbaranOrdenante, Tot.Departamento);
        //        }


        //        Utils.WriteToFileTMP2(Tot.NombreRemitente + ";" + Tot.DomicilioRemitente + ";" + Tot.PoblacionRemitente + ";" + Tot.CodigoPostalRemitente + ";" + Tot.CodigoPaisRemitente);
        //        Utils.WriteToFileTMP2(Tot.NombreDestinatario + ";" + Tot.DomicilioDestinatario + ";" + Tot.PoblacionDestinatario + ";" + Tot.CodigoPostalDestinatario + ";" + Tot.CodigoPaisDestinatario);


        //             Validar(token, Tot.idRuta.ToString());


        //        return ficheroCadena;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
        //        Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL dosAUno", ex.Message);
        //        return "";

        //    }


        //}
        //public static string   unoAuno(string token,Models.clsUtils.ProtocoloEdi edi)
        //{
        //    try
        //    {
        //        Models.clsUtils.ProtocoloEdiDatos carga = new Models.clsUtils.ProtocoloEdiDatos();
        //        Models.clsUtils.ProtocoloEdiDatos Descarga = new Models.clsUtils.ProtocoloEdiDatos();
        //        Models.clsUtils.ProtocoloEdiDatos Tot = new Models.clsUtils.ProtocoloEdiDatos();

        //        Models.clsUtils.ProtocoloEdiDatos tmp = new Models.clsUtils.ProtocoloEdiDatos();

        //        tmp = edi.ProtocoloEdiDatosLista[0];
        //        if (tmp.CargaDescarga == "C")
        //        {
        //            carga = edi.ProtocoloEdiDatosLista[0];
        //            Descarga = edi.ProtocoloEdiDatosLista[1];
        //        }
        //        else
        //        {
        //            carga = edi.ProtocoloEdiDatosLista[1];
        //            Descarga = edi.ProtocoloEdiDatosLista[0];
        //        }


        //        Tot.AlbaranOrdenante = edi.AlbaranOrdenante.Trim();
        //        Tot.FechaSalidaExpedicion = carga.FechaSalidaExpedicion;
        //        //Tot.HoraAcordadaSalida = carga.HoraAcordadaSalida;
        //        Tot.HoraAcordadaSalida = "00:00";
        //        Tot.FechaEntregaExpedicion = Descarga.FechaEntregaExpedicion;
        //        //Tot.HoraAcordadaEntrega = Descarga.HoraAcordadaEntrega;
        //        Tot.HoraAcordadaEntrega = "00:00";

        //        Tot.NombreRemitente = carga.NombreRemitente;
        //        Tot.DomicilioRemitente = carga.DomicilioRemitente;
        //        Tot.PoblacionRemitente = carga.PoblacionRemitente;
        //        Tot.CodigoPostalRemitente = carga.CodigoPostalRemitente;
        //        Tot.CodigoPaisRemitente = carga.CodigoPaisRemitente;


        //        Tot.NombreDestinatario = Descarga.NombreDestinatario;
        //        Tot.DomicilioDestinatario = Descarga.DomicilioDestinatario;
        //        Tot.PoblacionDestinatario = Descarga.PoblacionDestinatario;
        //        Tot.CodigoPostalDestinatario = Descarga.CodigoPostalDestinatario;
        //        Tot.CodigoPaisDestinatario = Descarga.CodigoPaisDestinatario;


        //        Tot.BaremoPalets = Utils.IsNull(carga.BaremoPalets) ? "0" : carga.BaremoPalets;
        //        Tot.BaremoPesoBruto = Utils.IsNull(carga.BaremoPesoBruto) ? "0" : carga.BaremoPesoBruto;
        //        Tot.BaremoVolumen = Utils.IsNull(carga.BaremoVolumen) ? "0" : carga.BaremoVolumen;
        //        Tot.BaremoBultos = Utils.IsNull(carga.BaremoBultos) ? "0" : carga.BaremoBultos;
        //        Tot.OrderId = carga.OrderId;
                
        //        Tot.idRuta = carga.idRuta;

        //        Int32 kg;
        //        carga.BaremoPesoBruto = carga.BaremoPesoBruto.Replace(".0", "");

        //        if (Utils.IsNumeric(carga.BaremoPesoBruto))
        //            kg = Convert.ToInt32(carga.BaremoPesoBruto);
        //        else
        //            kg = 0;

        //        Tot.Departamento = Utils.DimeDepartamento(Descarga.CodigoPostalDestinatario, kg, Descarga.CodigoPaisDestinatario);

        //        string ficheroCadena;
        //        if (Tot.Departamento=="8")
        //        {                
        //            ficheroCadena = generaEdiCadenaLocal(Tot,1,"unoAuno",0);
        //        }
        //        else
        //        { 
        //            ficheroCadena = generaEdiCadena(Tot);
        //        }


        //        Utils.GuardaEdi(ficheroCadena, Tot.AlbaranOrdenante, Tot.Departamento);

        //            Validar(token, Tot.idRuta.ToString());

        //        Utils.WriteToFileTMP2(Tot.NombreRemitente + ";" + Tot.DomicilioRemitente + ";" + Tot.PoblacionRemitente + ";" + Tot.CodigoPostalRemitente + ";" + Tot.CodigoPaisRemitente);
        //        Utils.WriteToFileTMP2(Tot.NombreDestinatario + ";" + Tot.DomicilioDestinatario + ";" + Tot.PoblacionDestinatario + ";" + Tot.CodigoPostalDestinatario + ";" + Tot.CodigoPaisDestinatario);
        //        return ficheroCadena;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
        //        Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL unoAuno", ex.Message);
        //        return "";

        //    }


        //}

        //public static Models.clsUtils.ProtocoloEdi infoAlbaranes (string token, string Albaran)
        //{
        //    try
        //    {
        //        Models.clsUtils.ProtocoloEdi ProtocoloEdi = new Models.clsUtils.ProtocoloEdi();

        //        ProtocoloEdi.ProtocoloEdiDatosLista = new List<Models.clsUtils.ProtocoloEdiDatos>();


        //        Models.clsUtils.DatosRuta vRetApi = new Models.clsUtils.DatosRuta(); //todos los datos de la ruta
        //        vRetApi = DameDatosRuta(token, Albaran);
        //        if (vRetApi != null)
        //        {
        //            if (!Utils.IsNumeric(vRetApi.route_code.Trim()))
        //            {
        //                return null;

        //            }

        //            if (vRetApi.stops.Count > 2)
        //                Console.WriteLine("paradas:" + vRetApi.stops.Count + "  albaran" + vRetApi.route_code);

        //            ProtocoloEdi.AlbaranOrdenante = vRetApi.route_code;
        //            ProtocoloEdi.idRuta = vRetApi.id;


        //            Models.clsUtils.Stop parada = new Models.clsUtils.Stop();
        //            List<Models.clsUtils.Stop> paradas = new List<Models.clsUtils.Stop>();
        //            paradas = vRetApi.stops;




        //            foreach (Models.clsUtils.Stop para in paradas)
        //            {
        //                if (para.stop_type.ToString() == "C")
        //                {

        //                    Models.clsUtils.ProtocoloEdiDatos ProtocoloEdiDatos = new Models.clsUtils.ProtocoloEdiDatos();


        //                    DateTime vfecha = Utils.FromUnixTime(Convert.ToInt64(para.faraway_planned_timestamp)); 

        //                    ProtocoloEdiDatos.FechaSalidaExpedicion = vfecha.ToString("dd/MM/yyyy");
        //                    ProtocoloEdiDatos.HoraAcordadaSalida = vfecha.ToString("HH:mm");

        //                    ProtocoloEdiDatos.CargaDescarga = "C";

        //                    Int32 al = Convert.ToInt32(para.operations[0].related_data[0].row[0].value.ToString());
        //                    ProtocoloEdiDatos.AlbaranOrdenante = al.ToString();
        //                    //ProtocoloEdiDatos.AlbaranOrdenante = vRetApi.route_code;
        //                    ProtocoloEdiDatos.idRuta = vRetApi.id;
        //                    ProtocoloEdiDatos.NombreRemitente = para.name;
        //                    ProtocoloEdiDatos.DomicilioRemitente = para.address;
        //                    ProtocoloEdiDatos.PoblacionRemitente = para.city;
        //                    ProtocoloEdiDatos.CodigoPostalRemitente = para.postal_code;
        //                    ProtocoloEdiDatos.CodigoPaisRemitente = para.country;
        //                    ProtocoloEdiDatos.BaremoPalets = para.operations[0].expected_pallet_number;
        //                    ProtocoloEdiDatos.BaremoPesoBruto = para.operations[0].expected_weight;
        //                    ProtocoloEdiDatos.BaremoVolumen = para.operations[0].expected_volume;
        //                    ProtocoloEdiDatos.BaremoBultos = para.operations[0].expected_units;
        //                    ProtocoloEdiDatos.OrderId= para.operations[0].order_id;


        //                    ProtocoloEdi.ProtocoloEdiDatosLista.Add(ProtocoloEdiDatos);
        //                }

        //                if (para.stop_type.ToString() == "D")
        //                {
        //                    Models.clsUtils.ProtocoloEdiDatos ProtocoloEdiDatos = new Models.clsUtils.ProtocoloEdiDatos();

        //                    ProtocoloEdiDatos.AlbaranOrdenante = vRetApi.route_code;

        //                    DateTime vfecha = Utils.FromUnixTime(Convert.ToInt64(para.faraway_planned_timestamp));

        //                    ProtocoloEdiDatos.FechaEntregaExpedicion = vfecha.ToString("dd/MM/yyyy");
        //                    ProtocoloEdiDatos.HoraAcordadaEntrega = vfecha.ToString("HH:mm");


        //                    ProtocoloEdiDatos.CargaDescarga = "D";
        //                    ProtocoloEdiDatos.NombreDestinatario = para.name;
        //                    ProtocoloEdiDatos.DomicilioDestinatario = para.address;
        //                    ProtocoloEdiDatos.PoblacionDestinatario = para.city;
        //                    ProtocoloEdiDatos.CodigoPostalDestinatario = para.postal_code;
        //                    ProtocoloEdiDatos.CodigoPaisDestinatario = para.country;
        //                    ProtocoloEdiDatos.BaremoPalets = para.operations[0].expected_pallet_number;
        //                    ProtocoloEdiDatos.BaremoPesoBruto = para.operations[0].expected_weight;
        //                    ProtocoloEdiDatos.BaremoVolumen = para.operations[0].expected_volume;
        //                    ProtocoloEdiDatos.BaremoBultos = para.operations[0].expected_units;
        //                    ProtocoloEdiDatos.OrderId = para.operations[0].order_id;

        //                    ProtocoloEdi.ProtocoloEdiDatosLista.Add(ProtocoloEdiDatos);

        //                }


        //            }
        //        }
        //        return ProtocoloEdi;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
        //        Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR REPSOL infoAlbaranes", ex.Message);
        //        return null;
        //    }

        //}

        public static void ConsigueAlbaranes()
        {

            string token;
            token = ApiClases.DameToken();

            if (token != "")
            {
                Console.WriteLine(DateTime.Now.ToString() +  " RECOGIENDO TOKEN:" + token);
                List<string> listaAlbaranes = new List<string>();
                listaAlbaranes = ApiClases.DameTodasRutas(token);

                int cont = 0;
                foreach (string item in listaAlbaranes)
                {
                    nou(token, item);
                    cont += 1;
                    Console.WriteLine(DateTime.Now.ToString() + " Generando("+ cont.ToString()  + "/"+ listaAlbaranes.Count.ToString() +"):" + item);

                }
            }
            else
            {
                Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN", "ERROR CONGUIENDO TOKEN");
            }

        }

        public static void ConsigueUnSoloAlbaran()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("ALBARAN(ES) A INTEGRAR (SEPARADOS POR COMAS SI HAY MAS DE UNO):");
            string albaran = Console.ReadLine();

            string token;
            token = ApiClases.DameToken();

            if (token != "")
            {
                List<string> listaAlbaranes = new List<string>();
                string[] array = albaran.Split(',');
                foreach (string value in array)
                {                    
                    nou(token, value);
                }               
                
            }
            else
            {
                Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");

            }

        }



        public static void nou(string token,string Albaran)
        {
            //string token = DameToken();

            //Console.WriteLine(token);

            Models.clsUtils.DatosRuta todo = new Models.clsUtils.DatosRuta();
            todo  = DameDatosRuta(token, Albaran);
            List<Models.clsUtils.Stop> paradas = new List<Models.clsUtils.Stop>();
            paradas = todo.stops;

            List<Models.clsUtils.Stop> tipoC = todo.stops.Where(r => r.stop_type == "C").ToList();
            List<Models.clsUtils.Stop> tipoD = todo.stops.Where(r => r.stop_type == "D").ToList();


            Int32 Peso=PesoCargas(tipoC);
            List<string> vPaisDestinoYCodPostal = new List<string>();
            vPaisDestinoYCodPostal=PaisDestinoYCodPostal(tipoD);
            string TipoDepartamento = string.Empty;

            TipoDepartamento=Utils.DimeDepartamento(vPaisDestinoYCodPostal[1], Peso, vPaisDestinoYCodPostal[0]);

            if (TipoDepartamento.Trim()=="8")
            {
                string cargas=GeneraLocalCargas(tipoC);
                var array = cargas.Split(';');
                string desCargas = GeneraLocalDesCargas(token,tipoD,tipoD.Count,array[0]);

                Utils.GuardaEdi(cargas +   desCargas,array[0],TipoDepartamento);
            }
            else
            {
                List<Models.clsUtils.ProtocoloEdiDatos> listaDatos = new List<Models.clsUtils.ProtocoloEdiDatos>();
                listaDatos= GeneraResto(tipoC, tipoD);
                EmparejarCargaDescarga(token,listaDatos, TipoDepartamento);
            }
        }

        public static void EmparejarCargaDescarga(string token ,List<Models.clsUtils.ProtocoloEdiDatos> listaDatos,string TipoDepartamento)
        {

            List<string> listaAlbaranes = listaDatos.Select(a => a.AlbaranOrdenante).Distinct().ToList();

            foreach (string numAlbaranm in listaAlbaranes)            {

                List<Models.clsUtils.ProtocoloEdiDatos> filtrat = listaDatos.Where(l => l.AlbaranOrdenante == numAlbaranm).ToList();
                string fic= RelacionaCargaDescarga(token,filtrat);                

                string[] array = fic.Split(';');

                Utils.GuardaEdi(fic, array[0], TipoDepartamento);


            }
        }

        public static  string  RelacionaCargaDescarga(string token, List<Models.clsUtils.ProtocoloEdiDatos> listaDatos)
        {

            List<Models.clsUtils.ProtocoloEdiDatos> filtratCarga = listaDatos.Where(l => l.CargaDescarga=="C").ToList();
            List<Models.clsUtils.ProtocoloEdiDatos> filtratDesCarga = listaDatos.Where(l => l.CargaDescarga == "D").ToList();

            string linea=string.Empty ;

            int pesototal = 0;
            int peso = 0;
            foreach (Models.clsUtils.ProtocoloEdiDatos pesoSuma in filtratCarga)
            {
                peso = Convert.ToInt32( pesoSuma.BaremoPesoBruto);
                pesototal = pesototal + peso;

            } 


            linea = filtratCarga[0].AlbaranOrdenante + ";" +
                    Convert.ToDateTime(filtratCarga[0].FechaSalidaExpedicion).ToString("dd/MM/yyyy") + ";" +
                    filtratCarga[0].HoraAcordadaSalida + ";" +
                    Convert.ToDateTime(filtratDesCarga[0].FechaEntregaExpedicion).ToString("dd/MM/yyyy") + ";" +
                    filtratDesCarga[0].HoraAcordadaEntrega + ";" +
                    filtratCarga[0].NombreRemitente + ";" +
                    filtratCarga[0].DomicilioRemitente + ";" +
                    filtratCarga[0].PoblacionRemitente + ";" +
                    filtratCarga[0].CodigoPostalRemitente + ";" +
                    filtratCarga[0].CodigoPaisRemitente + ";" +


                    filtratDesCarga[0].NombreDestinatario + ";" +
                    filtratDesCarga[0].DomicilioDestinatario + ";" +
                    filtratDesCarga[0].PoblacionDestinatario + ";" +
                    filtratDesCarga[0].CodigoPostalDestinatario + ";" +
                    filtratDesCarga[0].CodigoPaisDestinatario + ";" +

                    filtratDesCarga[0].BaremoPalets + ";" +
                    pesototal.ToString() + ";" +
                    filtratDesCarga[0].BaremoVolumen + ";" +
                    filtratDesCarga[0].BaremoBultos + ";" +
                    filtratDesCarga[0].BaremoMetrosLineales + ";" +
                    "" + ";" +
                    "" + ";" +
                    filtratDesCarga[0].OrderId;

            Validar(token, filtratDesCarga[0].idRuta.ToString());

            return linea;

        }


        public static List<Models.clsUtils.ProtocoloEdiDatos> GeneraResto(List<Models.clsUtils.Stop>  tipoC, List<Models.clsUtils.Stop> tipoD)
        {

            // Cargamos en una lista Cargas y Descargas en listaDatos
            List<Models.clsUtils.ProtocoloEdiDatos> listaDatos = new List<Models.clsUtils.ProtocoloEdiDatos>();
            try
            {
                string orderId = "";
                
                foreach (Models.clsUtils.Stop item in tipoC)
                { 
          
                    List<Models.clsUtils.Operation> loperaciones = new List<Models.clsUtils.Operation>();
                    loperaciones = item.operations;
                    foreach (Models.clsUtils.Operation opitem in loperaciones)
                    {
                        Models.clsUtils.ProtocoloEdiDatos datos = new Models.clsUtils.ProtocoloEdiDatos();
                        string albaran = Convert.ToInt32(opitem.related_data[0].row[0].value.ToString()).ToString();
                        datos.AlbaranOrdenante = albaran;
                        datos.idRuta = opitem.route_id;
                        datos.CargaDescarga = "C";
                        DateTime FechaCarga = Utils.FromUnixTime(Convert.ToInt64(item.faraway_planned_timestamp));
                        datos.FechaSalidaExpedicion = FechaCarga.ToString("dd/MM/yyyy");
                        string HoraCarga = "00:00";
                        datos.HoraAcordadaSalida = HoraCarga;
                        string Nombre = item.name.ToUpper();
                        datos.NombreRemitente = Nombre;
                        string Domicilio = item.address.ToUpper();
                        datos.DomicilioRemitente = Domicilio;
                        string Poblacion = item.city.ToUpper();
                        datos.PoblacionRemitente = Poblacion;                        
                        string CodigoPostal = item.postal_code.ToUpper();
                        datos.CodigoPostalRemitente = CodigoPostal;
                        string CodigoPais = item.country.ToUpper();
                        datos.CodigoPaisRemitente = CodigoPais;

                        int peso = 0;
                        peso = Convert.ToInt32(opitem.expected_weight.Replace(".0", ""));
                   
                        orderId = opitem.order_id;
                        datos.BaremoPesoBruto = peso.ToString();
                        datos.BaremoVolumen = "0";
                        datos.BaremoPalets = "0";
                        datos.BaremoVolumen = "0";
                        datos.BaremoBultos = "0";
                        datos.BaremoMetrosLineales = "0";
                        datos.OrderId = orderId;
                        listaDatos.Add(datos);
                    }
        
                }



                foreach (Models.clsUtils.Stop item in tipoD)
                {
                    List<Models.clsUtils.Operation> loperaciones = new List<Models.clsUtils.Operation>();
                    loperaciones = item.operations;
                    foreach (Models.clsUtils.Operation opitem in loperaciones)
                    {
                        Models.clsUtils.ProtocoloEdiDatos datos = new Models.clsUtils.ProtocoloEdiDatos();
                        string albaran = Convert.ToInt32(opitem.related_data[0].row[0].value.ToString()).ToString();
                        datos.AlbaranOrdenante = albaran;
                        datos.idRuta = opitem.route_id;
                        datos.CargaDescarga = "D";
                        DateTime FechaDesCarga = Utils.FromUnixTime(Convert.ToInt64(item.faraway_planned_timestamp));
                        datos.FechaEntregaExpedicion = FechaDesCarga.ToString("dd/MM/yyyy");
                        string HoraDesCarga = "00:00";
                        datos.HoraAcordadaEntrega = HoraDesCarga;
                        string Nombre = item.name.ToUpper();
                        datos.NombreDestinatario = Nombre;
                        string Domicilio = item.address.ToUpper();
                        datos.DomicilioDestinatario = Domicilio;
                        string Poblacion = item.city.ToUpper();
                        datos.PoblacionDestinatario = Poblacion;
                        string CodigoPostal = item.postal_code.ToUpper();
                        datos.CodigoPostalDestinatario = CodigoPostal;
                        string CodigoPais = item.country.ToUpper();
                        datos.CodigoPaisDestinatario = CodigoPais;

                        int peso = 0;
                        peso = Convert.ToInt32(opitem.expected_weight.Replace(".0", ""));

                        

                        orderId = opitem.order_id;
                        datos.OrderId = orderId;
                        datos.BaremoPesoBruto = peso.ToString();
                        datos.BaremoVolumen = "0";
                        datos.BaremoPalets = "0";
                        datos.BaremoVolumen = "0";
                        datos.BaremoBultos = "0";
                        datos.BaremoMetrosLineales = "0";
                        


                        listaDatos.Add(datos);

                    }
                }
                return listaDatos;
            }
            catch (Exception ex)
            {
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ";GenerarResto ", ex.Message);
                return null;
                
            }

        }


        public static string GeneraLocalDesCargas(string token,List<Models.clsUtils.Stop> tipoD,int nLineas,string albaran)
        {
            try
            {
                 


                int orden = 0;
                string linea = "";
                string orderId = "";
                string idruta = "";
                foreach (Models.clsUtils.Stop item in tipoD)
                {
                    int pesoTotal = 0;
                    //string albaran = Convert.ToInt32(item.operations[0].related_data[0].row[0].value.ToString()).ToString();
                    idruta = item.route_id.ToString();
                    string tipo = "0";
                    orden = +1;
                    if (orden == tipoD.Count) orden = 999;
                    DateTime FechaDesCarga = Utils.FromUnixTime(Convert.ToInt64(item.faraway_planned_timestamp));
                    string HoraDesCarga = FechaDesCarga.ToString("HH:mm");
                    string Nombre = item.name.ToUpper();
                    string Domicilio = item.address.ToUpper();
                    string Poblacion = item.city.ToUpper();
                    string CodigoPostal = item.postal_code.ToUpper();
                    string CodigoPais = item.country.ToUpper();
                    Models.clsUtils.Operation operacion = new Models.clsUtils.Operation();
                    List<Models.clsUtils.Operation> loperaciones = new List<Models.clsUtils.Operation>();
                    loperaciones = item.operations;
                    foreach (Models.clsUtils.Operation opitem in loperaciones)
                    {
                        int peso = 0;
                        peso = Convert.ToInt32(opitem.expected_weight.Replace(".0", ""));
                        pesoTotal = peso + peso;
                        orderId = opitem.order_id;

                    }
                    linea = linea + albaran + ";" + tipo + ";" + orden + ";" + FechaDesCarga.ToString("dd/MM/yyyy") + ";" + HoraDesCarga + ";" + Nombre + ";" + Domicilio + ";" + Poblacion + ";" + CodigoPostal + ";" + CodigoPais + ";0;" + pesoTotal.ToString() + ";0;0;" + orderId + Environment.NewLine;
                }

                Validar(token, idruta);

                return linea;
            }
            catch (Exception ex)
            {
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ";GenerarLocalCargas ", ex.Message);
                return "";
            }
        }
         

        public static string  GeneraLocalCargas(List<Models.clsUtils.Stop> tipoC)
        {
            try
            {
                int orden = 0;
                string linea = "";
                string orderId = "";

                string albaran=string.Empty;
                List<Models.clsUtils.Operation> loperacionesTemp = new List<Models.clsUtils.Operation>();
                foreach (Models.clsUtils.Stop item in tipoC)
                {
                    loperacionesTemp = item.operations;
                    foreach (Models.clsUtils.Operation opitem in loperacionesTemp)
                    {
                        string a = Convert.ToInt32(opitem.related_data[0].row[0].value.ToString()).ToString();
                        if (albaran != Convert.ToInt32(opitem.related_data[0].row[0].value.ToString()).ToString())
                            albaran = albaran + a + "-";                    }
                }
                albaran = albaran.Substring(0, albaran.Length - 1);



                foreach (Models.clsUtils.Stop item in tipoC)
                {
                   
                    
                    string tipo = "2";
                    orden  +=1;
                    //if (orden == tipoC.Count) orden = 999;
                    DateTime FechaCarga = Utils.FromUnixTime(Convert.ToInt64(item.faraway_planned_timestamp));
                    string HoraCarga = FechaCarga.ToString("HH:mm");
                    string Nombre = item.name.ToUpper();
                    string Domicilio = item.address.ToUpper();
                    string Poblacion = item.city.ToUpper();
                    string CodigoPostal = item.postal_code.ToUpper();
                    string CodigoPais = item.country.ToUpper();
                    Models.clsUtils.Operation operacion = new Models.clsUtils.Operation();
                    List<Models.clsUtils.Operation> loperaciones = new List<Models.clsUtils.Operation>();
                    loperaciones = item.operations;
                    foreach (Models.clsUtils.Operation opitem in loperaciones)
                    {
             //           string albaran = Convert.ToInt32(opitem.related_data[0].row[0].value.ToString()).ToString();
                        int peso = 0;
                        peso = Convert.ToInt32(opitem.expected_weight.Replace(".0", ""));
                        
                        orderId = opitem.order_id;

                        linea = linea + albaran + ";" + tipo + ";" + orden + ";" + FechaCarga.ToString("dd/MM/yyyy") + ";" + HoraCarga + ";" + Nombre + ";" + Domicilio + ";" + Poblacion + ";" + CodigoPostal + ";" + CodigoPais + ";0;" + peso.ToString() + ";0;0;" + orderId + Environment.NewLine;

                    }
                    
                }
                return linea;
            }
            catch (Exception ex)
            {
                Utils.EnviarMailErrores(DateTime.Now.ToString() + ";GenerarLocalCargas " , ex.Message);
                return "";
            }
        }

        public static Int32 PesoCargas(List<Models.clsUtils.Stop> tipoC)
        {
            try
            {
                List<Models.clsUtils.Operation> op = new List<Models.clsUtils.Operation>();
                Int32 totPeso = 0;
                Int32 pesoOP = 0;
                foreach (Models.clsUtils.Stop item in tipoC)
                {
                    op = item.operations;
                    Models.clsUtils.Operation peso = new Models.clsUtils.Operation();
                    foreach (Models.clsUtils.Operation p in op)
                    {
                        pesoOP = Convert.ToInt32(p.expected_weight.Replace(".0", ""));
                        totPeso = totPeso + pesoOP;
                    }
                }
                return totPeso;
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public static List<string>  PaisDestinoYCodPostal(List<Models.clsUtils.Stop> tipoD)
        {
            try
            {
                List<string> vret = new List<string>();
                List<Models.clsUtils.Operation> op = new List<Models.clsUtils.Operation>();
                string pais=string.Empty;
                foreach (Models.clsUtils.Stop item in tipoD)
                {
                    vret.Add(item.country);
                    vret.Add(item.postal_code);
                }
                return vret;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static string Limpia(string texto)
        {
            texto = texto.Replace("\\", "");
            texto = texto.Replace("\"", "");
            texto = texto.Replace("value:", "");
            texto = texto.Replace(",", "");
            texto = texto.Trim();
            return texto;


        }


    }
}
