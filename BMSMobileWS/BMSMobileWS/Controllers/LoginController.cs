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
    public class LoginController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage LoginApp(string Usuario, string Clave)
        {
            try
            {
                using(BMS2015Entities db = new BMS2015Entities())
                {
                    var user = db.MobileBMS_Login(Usuario, Clave).SingleOrDefault();
                    if (user == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Usuario o contraseña erroneos.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                }
            }
            catch(Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage Establecimientos(string Usuario)
        {
            try
            {
                using(BMS2015Entities db = new BMS2015Entities())
                {
                    var estab = db.MobileBMS_EstabsUsuario(Usuario).ToList();
                    if (estab.Count() <= 0 || estab == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentraron establecimientos.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, estab);
                }
            }
            catch(Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}