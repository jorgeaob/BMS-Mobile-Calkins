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
    public class ConsultaPalletController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage TraerPallet(string Pallet,string Localizacion, bool EsPallet, string CodEstab, bool TraerPallet)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var pallet = bd.CalkinsWS_ConsultaPallet(Pallet, Localizacion,EsPallet, CodEstab,TraerPallet).SingleOrDefault();
                    if (pallet == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existe información para ese pallet.");
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
        [HttpPost]
        public HttpResponseMessage Guardar(string Pallet, string Localizacion,string FechaCad, string LoteFab)
        {
            try
            {
                //parametros.Add("Pallet", pallet);
                //parametros.Add("Localizacion", localizacion);
                //parametros.Add("FechaCad", fecha_cad.ToString()); ;
                //parametros.Add("LoteFab", lotefab);
                var Fecha = DateTime.Parse(FechaCad);
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    if (string.IsNullOrEmpty(LoteFab)) { LoteFab = ""; }
                    var genFol = bd.CalkinsWS_GuardarPallet(Pallet, Localizacion,Fecha,LoteFab);
                    return Request.CreateResponse(HttpStatusCode.OK,"");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
