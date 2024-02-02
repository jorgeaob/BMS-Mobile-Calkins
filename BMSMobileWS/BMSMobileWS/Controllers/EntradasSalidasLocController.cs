using BMSMobileWS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BMSMobileWS.Controllers
{
    public class EntradasSalidasLocController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        public HttpResponseMessage TraerPallet(string Localizacion)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var pallet = bd.CalkinsWS_EntradasSalidasLoc(Localizacion).ToList();
                    if (pallet == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existe información para esta localización.");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, pallet);
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
