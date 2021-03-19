using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepsolTT.Models
{
    public class clsUtils
    {
        public class GlobalVariables
        {
            public static string Api { get; set; }

            public static string API_KEY { get; set; }
            public static string Usuario { get; set; }
            public static string Password { get; set; }
            public static string RutaEDI { get; set; }
            public static string Departamento { get; set; }

        }

        public class EReturn
        {
            public string type { get; set; }
            public string message { get; set; }
            public string code { get; set; }
            public object error { get; set; }
        }

        public class retornoApi
        {
            public EReturn e_return { get; set; }
        }


        public class Credenciales
        {
            public string user { get; set; }
            public string pass { get; set; }
            public int language_id { get; set; }
        
        }

        public class ChoferMatricula
        {
            public string driver_id { get; set; }
            public string driver_name { get; set; }
            public string driver_phone_number { get; set; }
            public string registration_one { get; set; }
            public string registration_two { get; set; }
            public string registration_three { get; set; }
            public int assign_timestamp { get; set; }
        }






        #region   Protocolo Edi campos
        public class ProtocoloEdi
        {
            public Int64 idRuta { get; set; }
            public string AlbaranOrdenante { get; set; }
            public List<ProtocoloEdiDatos> ProtocoloEdiDatosLista { get; set; }
        }
        public class ProtocoloEdiDatos
        {
            public Int64 idRuta { get; set; }
            public string CargaDescarga { get; set; }
            public string AlbaranOrdenante { get; set; }
            public string FechaSalidaExpedicion { get; set; }
            public string HoraAcordadaSalida { get; set; }
            public string FechaEntregaExpedicion { get; set; }
            public string HoraAcordadaEntrega { get; set; }
            public string Tipo { get; set; }
            public string Linea { get; set; }
            public string NombreRemitente { get; set; }
            public string DomicilioRemitente { get; set; }
            public string PoblacionRemitente { get; set; }
            public string CodigoPostalRemitente { get; set; }
            public string CodigoPaisRemitente { get; set; }
            public string NombreDestinatario { get; set; }
            public string DomicilioDestinatario { get; set; }
            public string PoblacionDestinatario { get; set; }
            public string CodigoPostalDestinatario { get; set; }
            public string CodigoPaisDestinatario { get; set; }
            public string BaremoPalets { get; set; }
            public string BaremoPesoBruto { get; set; }
            public string BaremoVolumen { get; set; }
            public string BaremoBultos { get; set; }
            public string BaremoMetrosLineales { get; set; }
            public string Observaciones1 { get; set; }
            public string Observaciones2 { get; set; }
            public string Departamento { get; set; }
            public string OrderId { get; set; }
            public string IdModoEnvio { get; set; }

        }

        #endregion

        #region Datos Ruta

        public class Row
        {
            public string key { get; set; }
            public object value { get; set; }
        }

        public class RelatedData
        {
            public string entity { get; set; }
            public List<Row> row { get; set; }
        }

        public class Operation
        {
            public int id { get; set; }
            public int stop_id { get; set; }
            public int route_id { get; set; }
            public string operation_code { get; set; }
            public int operation_type { get; set; }
            public string operation_description { get; set; }
            public string order_id { get; set; }
            public string real_pallet_number { get; set; }
            public string expected_pallet_number { get; set; }
            public string expected_package_number { get; set; }
            public string real_package_number { get; set; }
            public string linear_occupation { get; set; }
            public string status { get; set; }
            public string observations { get; set; }
            public string customer_id { get; set; }
            public string customer_name { get; set; }
            public string customer_phone { get; set; }
            public string customer_email { get; set; }
            public string expected_weight { get; set; }
            public string real_weight { get; set; }
            public string expected_units { get; set; }
            public string real_units { get; set; }
            public string expected_volume { get; set; }
            public string real_volume { get; set; }
            public string picking { get; set; }
            public List<RelatedData> related_data { get; set; }
            public List<object> containers { get; set; }
            public List<object> references { get; set; }
            public string confirmation_timestamp { get; set; }
            public List<object> incidences { get; set; }
            public List<object> documents { get; set; }
            public List<object> images { get; set; }
        }

        public class Event
        {
            public int stop_id { get; set; }
            public string stop_code { get; set; }
            public int stop_order { get; set; }
            public string additional_data_desc { get; set; }
            public string additional_data_value { get; set; }
            public int id { get; set; }
            public int route_id { get; set; }
            public string route_code { get; set; }
            public int status_id { get; set; }
            public string user_id { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string event_timestamp { get; set; }
            public string origin_code { get; set; }
        }

        public class Stop
        {
            public int id { get; set; }
            public int route_id { get; set; }
            public string internal_code { get; set; }
            public int order { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string postal_code { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string country { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string time_margin { get; set; }
            public string observations { get; set; }
            public string status { get; set; }
            public string duration { get; set; }
            public string receiver_id { get; set; }
            public string receiver_name { get; set; }
            public string receiver_phone_number { get; set; }
            public string receiver_sms { get; set; }
            public string receiver_whatsapp { get; set; }
            public string receiver_mail { get; set; }
            public string receiver_mail_alerts { get; set; }
            public string receiver_mail_events { get; set; }
            public string stop_type { get; set; }
            public string nearly_planned_timestamp { get; set; }
            public string faraway_planned_timestamp { get; set; }
            public string nearly_requested_timestamp { get; set; }
            public string faraway_requested_timestamp { get; set; }
            public List<object> related_data { get; set; }
            public List<Operation> operations { get; set; }
            public List<object> incidences { get; set; }
            public List<Event> events { get; set; }
            public object arrival_estimation { get; set; }
        }

        public class HistoricAssign
        {
        }

        public class DatosRuta
        {
            public int id { get; set; }
            public string route_code { get; set; }
            public string route_description { get; set; }
            public string business_id { get; set; }
            public string zone_id { get; set; }
            public int stops_number { get; set; }
            public string origin { get; set; }
            public int status { get; set; }
            public string observations { get; set; }
            public object version { get; set; }
            public string agency_id { get; set; }
            public string agency_desc { get; set; }
            public string agency_phone_number { get; set; }
            public string driver_id { get; set; }
            public string driver_name { get; set; }
            public string driver_phone_number { get; set; }
            public double total_load { get; set; }
            public object initial_kilometers { get; set; }
            public object final_kilometers { get; set; }
            public object avg_consumption { get; set; }
            public object meal_included { get; set; }
            public string group_code { get; set; }
            public string registration_one { get; set; }
            public string registration_two { get; set; }
            public string registration_three { get; set; }
            public string  vehicle_type_id_one { get; set; }
            public string vehicle_type_id_two { get; set; }
            public object vehicle_type_id_three { get; set; }
            public object motor_type_id { get; set; }
            public string convoy_type_id { get; set; }
            public string flag_1 { get; set; }
            public object flag_2 { get; set; }
            public object flag_3 { get; set; }
            public string management_code { get; set; }
            public string creation_timestamp { get; set; }
            public string nearly_planned_init_timestamp { get; set; }
            public string faraway_planned_init_timestamp { get; set; }
            public string nearly_planned_last_timestamp { get; set; }
            public string faraway_planned_last_timestamp { get; set; }
            public List<RelatedData> related_data { get; set; }
            public List<Stop> stops { get; set; }
            public List<object> incidences { get; set; }
            public List<Event> events { get; set; }
            public List<HistoricAssign> historic_assigns { get; set; }
            public List<object> documents { get; set; }
        }



        #endregion


        public class IdRuta
        {
            public Int64 Id { get; set; }
            public string Ruta { get; set; }
        }

        public class ModelEmail
        {
            public List<string> direccionesPara { get; set; }
            public List<string> direccionesCopia { get; set; }
            public List<string> direccionesCopiaOculta { get; set; }
            public string asunto { get; set; }
            public string mensaje { get; set; }
            public string fichero { get; set; }
            public bool isBodyHtml { get; set; }
        }


    }
}
