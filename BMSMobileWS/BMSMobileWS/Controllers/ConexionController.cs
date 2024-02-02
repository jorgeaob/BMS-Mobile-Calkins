using BMSMobileWS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BMSMobileWS.Controllers
{
    public class ConexionController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage ProbarConexion()
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    int r = bd.Database.ExecuteSqlCommand("SELECT GETDATE()");
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error: ", ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ex.Message);
            }
        }
    }
}