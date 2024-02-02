using BMSMobileWS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BMSMobileWS.Controllers
{
    public class RetirarMercanciaController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        public HttpResponseMessage TraerLoc(string Localizacion)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var info = bd.CalkinsWS_RetirarMercanciaLoc(Localizacion).ToList();
                    if (info == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existe información para esta localización.");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, info);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage TraerProd(string Localizacion,string CodProd)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var info = bd.CalkinsWS_RetirarMercanciaProd(Localizacion,CodProd).SingleOrDefault();
                    if (info == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existe información para esta localización.");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, info);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
