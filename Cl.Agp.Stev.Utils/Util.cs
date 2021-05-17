using Cl.Agp.Stev.Utils.Dictionaries;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Cl.Agp.Stev.Utils
{
    public static class Util
    {
        private static readonly ILog _log;

        static Util()
        {
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Método que registra la llamada y respuesta de un servicio dentro de un archivo.  
        /// </summary>
        /// <param name="_traceService"></param>
        /// <param name="_traceParam"></param>
        public static void CapTrace(string _traceService, string _traceParam)
        {
            string writefile = string.Empty;
            try
            {
                string pathFile = (Util.GetServerFilesRequest() + Util.GetValue("SoapTrace:PATH") + Util.GetValue("SoapTrace:NameBase") + _traceService + ".xml").ToString().Trim();
                if (!File.Exists(pathFile))
                {
                    File.AppendAllText(pathFile, _traceParam);
                }
                else
                {
                    File.Move(pathFile, (Util.GetServerFilesRequest() + Util.GetValue("SoapTrace:PATH") + Util.GetValue("SoapTrace:NameBase") + _traceService + DateTime.Now.ToString("ddMMyyyyhhmmssf").Replace(":", "") + ".xml").ToString().Trim());
                }
            }
            catch (IOException io)
            {
                _log.Error("Exeptions : " + io.Message + " StackTrace : " + io.StackTrace);
            }
            catch (Exception e)
            {
                _log.Error("Exeptions : " + e.Message + " StackTrace : " + e.StackTrace);
            }
        }

        /// <summary>
        /// Método que obtiene la ruta de ejecución del servidor. 
        /// </summary>
        /// <returns>string</returns>
        public static string GetServerFilesRequest()
        {
            return HttpContext.Current.Server.MapPath("");
        }

        /// <summary>
        /// Obtener configuracion a partir de un parametro del web.config, sección key
        /// </summary>
        /// <param name="_paramConfig"></param>
        /// <returns>string</returns>
        public static string GetValue(string _paramConfig)
        {
            try
            {
                string configValue = ConfigurationManager.AppSettings[_paramConfig.Trim().ToString()];
                return configValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string IsNotNull(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }else
            {
                return value;
            }
        }

        /// <summary>
        /// Método que obtiene el valor de una dirección IP consultante. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string/null</returns>
        public static string GetIpDirection(HttpRequest request)
        {
            try
            {
                string ipAddress = string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? request.ServerVariables["REMOTE_ADDR"].ToString() : request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                return ipAddress;
            }
            catch
            {
                return null;
            }
        }

        public static string ConvertSpecialCharacters(string text)
        {
            if (text != null)
            {
                foreach (KeyValuePair<string, string> character in SpecialCharactersDictionary.SpecialCharacterList())
                {
                    text = text.Replace(character.Key, character.Value);
                }
            }
            return text?.Trim();
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
