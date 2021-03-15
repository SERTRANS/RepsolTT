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
                Console.WriteLine(response.Content);
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
                    Console.WriteLine(response.Content);

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
         
        public static void crearRutas()
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
                    listaAlbaranes.Add(value);

                }
                    ApiClases.GeneraEdi(token, listaAlbaranes);
            }
            else

            {
                Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");

            }



        }

        public static void generaRutas()
        {   

            string token;
            token = ApiClases.DameToken();

            if (token != "")
            {
                List<string> listaAlbaranes = new List<string>();
                listaAlbaranes = ApiClases.DameTodasRutas(token);
                ApiClases.GeneraEdi(token, listaAlbaranes);
            }
            else

            {
                Console.WriteLine(DateTime.Now.ToString() + ",ERROR CONSIGUIENDO TOKEN");

            }
        }

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
                Console.WriteLine(response.Content);
                vRetApi = new Models.clsUtils.DatosRuta();
                if (response.Content.Contains("e_return"))
                {
                    vRetApi.route_code = "NOEXISTE";
               //     Utils.WriteToFile(DateTime.Now.ToString() + ";" + Albaran + ";" + response.Content);
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
                Console.WriteLine(response.Content);
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

            validarRutaBajda(token, ids);
        }

            public static void validarRutaBajda(string token,List<Int64> idrutaLista)
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
                        Console.WriteLine(response.Content);
                        Models.clsUtils.retornoApi vRetApi = new Models.clsUtils.retornoApi();
                        vRetApi = JsonConvert.DeserializeObject<Models.clsUtils.retornoApi>(response.Content);
                }
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
            }


        }

        public static List<string> DameTodasRutas(string token)
        {
            try
            {



                var client = new RestClient(Models.clsUtils.GlobalVariables.Api + "/entities/RUTAS/query?offset=0&limit=1150");
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
                Console.WriteLine(response.Content);

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


                    //if (array[i].Contains("ID_RUTA"))
                    //{
                    //    i++;
                    //    idruta = Limpia(array[i]);

                    //}



                    if (array[i].Contains("CODIGO_RUTA"))
                    {
                        i++;
                        listaAlbaranes.Add(Limpia(array[i]));
                        ruta = Limpia(array[i]);

                    }



                    //if (array[i].Contains("FH_INICIO_PLANIFICADA_CERCANA"))
                    //{
                    //    i++;
                    //    DateTime f = Convert.ToDateTime(Limpia(array[i]));

                    //    Utils.WriteToFileTMP(idruta + "," + ruta + "," + f.ToString());

                    //}

                }

                return listaAlbaranes;

            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
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

    public static List<int> NumeroCargasDescargas (Models.clsUtils.ProtocoloEdi edi)
        {


            try
            {

                int Cargas = 0;
                int DesCargas = 0;
                string albaran = string.Empty;

                Models.clsUtils.ProtocoloEdiDatos ediDatos = new Models.clsUtils.ProtocoloEdiDatos();

                foreach (Models.clsUtils.ProtocoloEdiDatos lista in edi.ProtocoloEdiDatosLista)
                {
                    if (lista.CargaDescarga.ToUpper() == "C") Cargas += 1;
                    if (lista.CargaDescarga.ToUpper() == "D") DesCargas += 1;
                    albaran = lista.AlbaranOrdenante;

                }

                List<int> dev = new List<int>();
                dev.Add(Cargas);
                dev.Add(DesCargas);
                //Utils.WriteToFile(albaran.ToString().Trim() + ";" +  Cargas.ToString() + ";" + DesCargas.ToString());
                return dev;
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                return null;

            }

        }


        public static void GeneraEdi(string token ,List<string> listaAlbaranes)
        {
            string ficheroCadena=string.Empty;
            try
            {

                foreach (string item in listaAlbaranes)
                {
                    Models.clsUtils.ProtocoloEdi edi = new Models.clsUtils.ProtocoloEdi();
                    edi = infoAlbaranes(token, item);
                    if (edi != null)
                    {
                        List<int> NCargasDescargas = new List<int>();
                        NCargasDescargas = NumeroCargasDescargas(edi);
                        if (NCargasDescargas != null)
                        {
                            if (NCargasDescargas[0] == 1 && NCargasDescargas[1] == 1)
                            {

                                ficheroCadena = unoAuno(token,edi);
                                if (ficheroCadena != "")
                                    Utils.WriteToFile(DateTime.Now.ToString() + ";" + ficheroCadena);
                                    

                            }
                            //Dos cargas un destino
                            else
                                if (NCargasDescargas[0] == 2 && NCargasDescargas[1] == 1)
                            {

                                ficheroCadena = dosAuno(token,edi);
                                //  Utils.GuardaEdi(ficheroCadena, item.Trim());
                                if (ficheroCadena != "")
                                    Utils.WriteToFile(DateTime.Now.ToString() + ";" + ficheroCadena);
                            }
                            else
                            {

                                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + "Mas cargas que las previstas" + ficheroCadena);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);


            }
             
         
    }

        public static string  generaEdiCadena(Models.clsUtils.ProtocoloEdiDatos edi)
        {

            try
            {
                string linea;
                linea = edi.AlbaranOrdenante.Trim() + ";" +
                         edi.FechaSalidaExpedicion + ";" +
                         edi.HoraAcordadaSalida + ";" +
                         edi.FechaEntregaExpedicion + ";" +
                         edi.HoraAcordadaEntrega + ";" +
                         edi.NombreRemitente.Trim() + ";" +
                         edi.DomicilioRemitente.Trim() + ";" +
                         edi.PoblacionRemitente.Trim() + ";" +
                         edi.CodigoPostalRemitente.Trim() + ";" +
                         edi.CodigoPaisRemitente.Trim() + ";" +
                         edi.NombreDestinatario.Trim() + ";" +
                         edi.DomicilioDestinatario.Trim() + ";" +
                         edi.PoblacionDestinatario.Trim() + ";" +
                         edi.CodigoPostalDestinatario.Trim() + ";" +
                         edi.CodigoPaisDestinatario.Trim() + ";" +
                         edi.BaremoPalets + ";" +
                         edi.BaremoPesoBruto + ";" +
                         edi.BaremoVolumen + ";" +
                         edi.BaremoBultos + ";" +
                         "0" + ";" +
                         "" + ";" +
                         "" + ";"
                        ;

                return linea;

            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                return "";

            }

        }


        public static void generaEdi(Models.clsUtils.ProtocoloEdiDatos edi)
        {
            try
            {


                string linea;
                linea = edi.AlbaranOrdenante.Trim() + ";" +
                         edi.FechaSalidaExpedicion + ";" +
                         edi.HoraAcordadaSalida + ";" +
                         edi.FechaEntregaExpedicion + ";" +
                         edi.HoraAcordadaEntrega + ";" +
                         edi.NombreRemitente.Trim() + ";" +
                         edi.DomicilioRemitente.Trim() + ";" +
                         edi.PoblacionRemitente.Trim() + ";" +
                         edi.CodigoPostalRemitente.Trim() + ";" +
                         edi.CodigoPaisRemitente.Trim() + ";" +
                         edi.NombreDestinatario.Trim() + ";" +
                         edi.DomicilioDestinatario.Trim() + ";" +
                         edi.PoblacionDestinatario.Trim() + ";" +
                         edi.CodigoPostalDestinatario.Trim() + ";" +
                         edi.CodigoPaisDestinatario.Trim() + ";" +
                         edi.BaremoPalets + ";" +
                         edi.BaremoPesoBruto + ";" +
                         edi.BaremoVolumen + ";" +
                         edi.BaremoBultos + ";" +
                         "0" + ";" +
                         "" + ";" +
                         "" + ";"
                        ;

                //         Utils.GuardaEdi(linea, edi.AlbaranOrdenante.Trim());

            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
            }

        }

        public static string  dosAuno(string token,Models.clsUtils.ProtocoloEdi edi)
        {
            try
            {

                Models.clsUtils.ProtocoloEdiDatos carga1 = new Models.clsUtils.ProtocoloEdiDatos();
                Models.clsUtils.ProtocoloEdiDatos carga2 = new Models.clsUtils.ProtocoloEdiDatos();
                Models.clsUtils.ProtocoloEdiDatos Descarga = new Models.clsUtils.ProtocoloEdiDatos();
                Models.clsUtils.ProtocoloEdiDatos Tot = new Models.clsUtils.ProtocoloEdiDatos();

                string ficheroCadena = "";


                if (edi.ProtocoloEdiDatosLista[0].CargaDescarga == "D")
                {
                    carga1 = edi.ProtocoloEdiDatosLista[1];
                    carga2 = edi.ProtocoloEdiDatosLista[2];
                    Descarga = edi.ProtocoloEdiDatosLista[0];
                }

                if (edi.ProtocoloEdiDatosLista[1].CargaDescarga == "D")
                {
                    carga1 = edi.ProtocoloEdiDatosLista[0];
                    carga2 = edi.ProtocoloEdiDatosLista[2];
                    Descarga = edi.ProtocoloEdiDatosLista[1];
                }

                if (edi.ProtocoloEdiDatosLista[2].CargaDescarga == "D")
                {
                    carga1 = edi.ProtocoloEdiDatosLista[0];
                    carga2 = edi.ProtocoloEdiDatosLista[1];
                    Descarga = edi.ProtocoloEdiDatosLista[2];
                }


                Tot.AlbaranOrdenante = edi.AlbaranOrdenante.Trim();
                Tot.FechaSalidaExpedicion = carga1.FechaSalidaExpedicion;
                Tot.HoraAcordadaSalida = carga1.HoraAcordadaSalida;
                Tot.FechaEntregaExpedicion = Descarga.FechaEntregaExpedicion;
                Tot.HoraAcordadaEntrega = Descarga.HoraAcordadaEntrega;

                Tot.NombreRemitente = carga1.NombreRemitente;
                Tot.DomicilioRemitente = carga1.DomicilioRemitente;
                Tot.PoblacionRemitente = carga1.PoblacionRemitente;
                Tot.CodigoPostalRemitente = carga1.CodigoPostalRemitente;
                Tot.CodigoPaisRemitente = carga1.CodigoPaisRemitente;

                Tot.NombreDestinatario = Descarga.NombreDestinatario;
                Tot.DomicilioDestinatario = Descarga.DomicilioDestinatario;
                Tot.PoblacionDestinatario = Descarga.PoblacionDestinatario;
                Tot.CodigoPostalDestinatario = Descarga.CodigoPostalDestinatario;
                Tot.CodigoPaisDestinatario = Descarga.CodigoPaisDestinatario;

                Tot.BaremoPalets = carga1.BaremoPalets;
                Tot.BaremoPesoBruto = carga1.BaremoPesoBruto;
                Tot.BaremoVolumen = carga1.BaremoVolumen;
                Tot.BaremoBultos = carga1.BaremoBultos;


                ficheroCadena = generaEdiCadena(Tot);



                Tot.AlbaranOrdenante = edi.AlbaranOrdenante.Trim();
                Tot.FechaSalidaExpedicion = carga2.FechaSalidaExpedicion;
                Tot.HoraAcordadaSalida = carga2.HoraAcordadaSalida;
                Tot.FechaEntregaExpedicion = Descarga.FechaEntregaExpedicion;
                Tot.HoraAcordadaEntrega = Descarga.HoraAcordadaEntrega;

                Tot.NombreRemitente = carga2.NombreRemitente;
                Tot.DomicilioRemitente = carga2.DomicilioRemitente;
                Tot.PoblacionRemitente = carga2.PoblacionRemitente;
                Tot.CodigoPostalRemitente = carga2.CodigoPostalRemitente;
                Tot.CodigoPaisRemitente = carga2.CodigoPaisRemitente;

                Tot.NombreDestinatario = Descarga.NombreDestinatario;
                Tot.DomicilioDestinatario = Descarga.DomicilioDestinatario;
                Tot.PoblacionDestinatario = Descarga.PoblacionDestinatario;
                Tot.CodigoPostalDestinatario = Descarga.CodigoPostalDestinatario;
                Tot.CodigoPaisDestinatario = Descarga.CodigoPaisDestinatario;

                Tot.BaremoPalets = carga2.BaremoPalets;
                Tot.BaremoPesoBruto = carga2.BaremoPesoBruto;
                Tot.BaremoVolumen = carga2.BaremoVolumen;
                Tot.BaremoBultos = carga2.BaremoBultos;

                Int32 kg;
                Tot.BaremoPesoBruto = Tot.BaremoPesoBruto.Replace(".0", "");

                if (Utils.IsNumeric(Tot.BaremoPesoBruto))
                    kg = Convert.ToInt32(Tot.BaremoPesoBruto);
                else
                    kg = 0;


                Tot.Departamento = Utils.DimeDepartamento(Descarga.CodigoPostalDestinatario, kg, Descarga.CodigoPaisDestinatario);



                ficheroCadena = ficheroCadena + Environment.NewLine + generaEdiCadena(Tot);

                Utils.GuardaEdi(ficheroCadena, Tot.AlbaranOrdenante, Tot.Departamento);

           //     Validar(token, Tot.idRuta.ToString());


                return ficheroCadena;
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                return "";

            }


        }
        public static string   unoAuno(string token,Models.clsUtils.ProtocoloEdi edi)
        {
            try
            {
                Models.clsUtils.ProtocoloEdiDatos carga = new Models.clsUtils.ProtocoloEdiDatos();
                Models.clsUtils.ProtocoloEdiDatos Descarga = new Models.clsUtils.ProtocoloEdiDatos();
                Models.clsUtils.ProtocoloEdiDatos Tot = new Models.clsUtils.ProtocoloEdiDatos();

                Models.clsUtils.ProtocoloEdiDatos tmp = new Models.clsUtils.ProtocoloEdiDatos();

                tmp = edi.ProtocoloEdiDatosLista[0];
                if (tmp.CargaDescarga == "C")
                {
                    carga = edi.ProtocoloEdiDatosLista[0];
                    Descarga = edi.ProtocoloEdiDatosLista[1];
                }
                else
                {
                    carga = edi.ProtocoloEdiDatosLista[1];
                    Descarga = edi.ProtocoloEdiDatosLista[0];
                }


                Tot.AlbaranOrdenante = edi.AlbaranOrdenante.Trim();
                Tot.FechaSalidaExpedicion = carga.FechaSalidaExpedicion;
                Tot.HoraAcordadaSalida = carga.HoraAcordadaSalida;
                Tot.FechaEntregaExpedicion = Descarga.FechaEntregaExpedicion;
                Tot.HoraAcordadaEntrega = Descarga.HoraAcordadaEntrega;

                Tot.NombreRemitente = carga.NombreRemitente;
                Tot.DomicilioRemitente = carga.DomicilioRemitente;
                Tot.PoblacionRemitente = carga.PoblacionRemitente;
                Tot.CodigoPostalRemitente = carga.CodigoPostalRemitente;
                Tot.CodigoPaisRemitente = carga.CodigoPaisRemitente;


                Tot.NombreDestinatario = Descarga.NombreDestinatario;
                Tot.DomicilioDestinatario = Descarga.DomicilioDestinatario;
                Tot.PoblacionDestinatario = Descarga.PoblacionDestinatario;
                Tot.CodigoPostalDestinatario = Descarga.CodigoPostalDestinatario;
                Tot.CodigoPaisDestinatario = Descarga.CodigoPaisDestinatario;


                Tot.BaremoPalets = Utils.IsNull(carga.BaremoPalets) ? "0" : carga.BaremoPalets;
                Tot.BaremoPesoBruto = Utils.IsNull(carga.BaremoPesoBruto) ? "0" : carga.BaremoPesoBruto;
                Tot.BaremoVolumen = Utils.IsNull(carga.BaremoVolumen) ? "0" : carga.BaremoVolumen;
                Tot.BaremoBultos = Utils.IsNull(carga.BaremoBultos) ? "0" : carga.BaremoBultos;

                Int32 kg;
                carga.BaremoPesoBruto = carga.BaremoPesoBruto.Replace(".0", "");

                if (Utils.IsNumeric(carga.BaremoPesoBruto))
                    kg = Convert.ToInt32(carga.BaremoPesoBruto);
                else
                    kg = 0;

                Tot.Departamento = Utils.DimeDepartamento(Descarga.CodigoPostalDestinatario, kg, Descarga.CodigoPaisDestinatario);

                string ficheroCadena;
                ficheroCadena = generaEdiCadena(Tot);   
                Utils.GuardaEdi(ficheroCadena, Tot.AlbaranOrdenante, Tot.Departamento);

            //    Validar(token, Tot.idRuta.ToString());

                return ficheroCadena;
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
                return "";

            }


        }

        public static Models.clsUtils.ProtocoloEdi infoAlbaranes (string token, string Albaran)
        {
            try
            {
                Models.clsUtils.ProtocoloEdi ProtocoloEdi = new Models.clsUtils.ProtocoloEdi();

                ProtocoloEdi.ProtocoloEdiDatosLista = new List<Models.clsUtils.ProtocoloEdiDatos>();


                Models.clsUtils.DatosRuta vRetApi = new Models.clsUtils.DatosRuta(); //todos los datos de la ruta
                vRetApi = DameDatosRuta(token, Albaran);
                if (vRetApi != null)
                {
                    if (!Utils.IsNumeric(vRetApi.route_code.Trim()))
                    {
                        return null;

                    }


                    if (vRetApi.stops.Count > 2)
                        Console.WriteLine("paradas:" + vRetApi.stops.Count + "  albaran" + vRetApi.route_code);


                    ProtocoloEdi.AlbaranOrdenante = vRetApi.route_code;
                    ProtocoloEdi.idRuta = vRetApi.id;



                    Models.clsUtils.Stop parada = new Models.clsUtils.Stop();
                    List<Models.clsUtils.Stop> paradas = new List<Models.clsUtils.Stop>();
                    paradas = vRetApi.stops;




                    foreach (Models.clsUtils.Stop para in paradas)
                    {
                        if (para.stop_type.ToString() == "C")
                        {

                            Models.clsUtils.ProtocoloEdiDatos ProtocoloEdiDatos = new Models.clsUtils.ProtocoloEdiDatos();


                            DateTime vfecha = Utils.FromUnixTime(Convert.ToInt64(para.faraway_planned_timestamp));

                            ProtocoloEdiDatos.FechaSalidaExpedicion = vfecha.ToString("dd/MM/yyyy");
                            ProtocoloEdiDatos.HoraAcordadaSalida = vfecha.ToString("hh:mm");

                            ProtocoloEdiDatos.CargaDescarga = "C";
                            ProtocoloEdiDatos.AlbaranOrdenante = vRetApi.route_code;
                            ProtocoloEdiDatos.idRuta = vRetApi.id;
                            ProtocoloEdiDatos.NombreRemitente = para.name;
                            ProtocoloEdiDatos.DomicilioRemitente = para.address;
                            ProtocoloEdiDatos.PoblacionRemitente = para.city;
                            ProtocoloEdiDatos.CodigoPostalRemitente = para.postal_code;
                            ProtocoloEdiDatos.CodigoPaisRemitente = para.country;
                            ProtocoloEdiDatos.BaremoPalets = para.operations[0].expected_pallet_number;
                            ProtocoloEdiDatos.BaremoPesoBruto = para.operations[0].expected_weight;
                            ProtocoloEdiDatos.BaremoVolumen = para.operations[0].expected_volume;
                            ProtocoloEdiDatos.BaremoBultos = para.operations[0].expected_units;
                            ProtocoloEdi.ProtocoloEdiDatosLista.Add(ProtocoloEdiDatos);
                        }

                        if (para.stop_type.ToString() == "D")
                        {
                            Models.clsUtils.ProtocoloEdiDatos ProtocoloEdiDatos = new Models.clsUtils.ProtocoloEdiDatos();

                            ProtocoloEdiDatos.AlbaranOrdenante = vRetApi.route_code;

                            DateTime vfecha = Utils.FromUnixTime(Convert.ToInt64(para.faraway_planned_timestamp));

                            ProtocoloEdiDatos.FechaEntregaExpedicion = vfecha.ToString("dd/MM/yyyy");
                            ProtocoloEdiDatos.HoraAcordadaEntrega = vfecha.ToString("hh:mm");


                            ProtocoloEdiDatos.CargaDescarga = "D";
                            ProtocoloEdiDatos.NombreDestinatario = para.name;
                            ProtocoloEdiDatos.DomicilioDestinatario = para.address;
                            ProtocoloEdiDatos.PoblacionDestinatario = para.city;
                            ProtocoloEdiDatos.CodigoPostalDestinatario = para.postal_code;
                            ProtocoloEdiDatos.CodigoPaisDestinatario = para.country;
                            ProtocoloEdiDatos.BaremoPalets = para.operations[0].expected_pallet_number;
                            ProtocoloEdiDatos.BaremoPesoBruto = para.operations[0].expected_weight;
                            ProtocoloEdiDatos.BaremoVolumen = para.operations[0].expected_volume;
                            ProtocoloEdiDatos.BaremoBultos = para.operations[0].expected_units;

                            ProtocoloEdi.ProtocoloEdiDatosLista.Add(ProtocoloEdiDatos);

                        }


                    }
                }
                return ProtocoloEdi;
            }
            catch (Exception ex)
            {
                Utils.WriteToFileFallo(DateTime.Now.ToString() + ";" + ex.Message);
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
