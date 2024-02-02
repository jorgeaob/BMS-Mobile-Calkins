using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BMSMobile.Utilities
{
    public class HttpRespuesta<T>
    {
        public T Result { get; set; }
        public bool Ok { get; set; }
        public string Message { get; set; }
        public HttpResponseMessage Response { get; set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public string Content { get; internal set; }
    }
}
