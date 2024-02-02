using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BMSMobile.Utilities
{
    public class RestClient
    {
        private Dictionary<string, string> _headers;
        public RestClient(Dictionary<string, string> headers)
        {
            if (headers != null)
                this._headers = headers;
            else
                this._headers = new Dictionary<string, string>();
        }
        public string this[string key]
        {
            get
            {
                return this._headers[key];
            }
            set
            {
                this._headers[key] = value;
            }
        }
        public async Task<HttpRespuesta<T>> Get<T>(string url, Dictionary<string, string> parametros = null, int timeout = 30)
        {
            if (parametros != null)
            {
                string query;
                var par = new FormUrlEncodedContent(parametros);
                query = par.ReadAsStringAsync().Result;
                var builder = new UriBuilder(url);
                builder.Query = query;
                url = builder.ToString();
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            client.Timeout = TimeSpan.FromSeconds(timeout);
            foreach (var header in this._headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            HttpResponseMessage responsemsg = null;
            var respuesta = new HttpRespuesta<T>();
            try
            {
                responsemsg = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Message = $"No hay conexión con el servidor, Msg:{ex.Message}";
                return respuesta;
            }
            respuesta.Response = responsemsg;
            if (responsemsg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonstring = await responsemsg.Content.ReadAsStringAsync();
                respuesta.Result = JsonConvert.DeserializeObject<T>(jsonstring);
                respuesta.Ok = true;
            }
            else
            {
                respuesta.Ok = false;
                respuesta.Message = responsemsg.ReasonPhrase;
            }
            return respuesta;
        }

        public async Task<HttpRespuesta<T>> Post<T>(string url, Dictionary<string, string> parametros = null, Object data = null)
        {
            if (parametros != null)
            {
                string query;
                var par = new FormUrlEncodedContent(parametros);
                query = par.ReadAsStringAsync().Result;
                var builder = new UriBuilder(url);
                builder.Query = query;
                url = builder.ToString();
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            var jsonstring = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            foreach (var header in this._headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            HttpResponseMessage responsemsg = null;
            var respuesta = new HttpRespuesta<T>();
            try
            {
                responsemsg = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Message = $"No hay conexión con el servidor, Msg:{ex.Message}";
                return respuesta;
            }
            respuesta.Response = responsemsg;
            if (responsemsg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonstringresult = await responsemsg.Content.ReadAsStringAsync();
                respuesta.Result = JsonConvert.DeserializeObject<T>(jsonstringresult);
                respuesta.Ok = true;
            }
            else
            {
                respuesta.Ok = false;
                respuesta.Message = responsemsg.ReasonPhrase;
            }
            return respuesta;
        }
    }
}
