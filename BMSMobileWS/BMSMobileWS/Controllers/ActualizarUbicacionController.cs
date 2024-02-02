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

    public class ActualizarUbicacionController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Localizacion(string Loc)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var estab = db.CalkinsWS_ActualizarUbicacionLoc(Loc).FirstOrDefault();
                    if (estab == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentró información.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, estab);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage Lote(string Folio)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var estab = db.CalkinsWS_ActualizarUbicacionLote(Folio).FirstOrDefault();
                    if (estab == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentró información.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, estab);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage Guardar(ActualizarUbicacionModel info)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var genFol = bd.CalkinsWS_ActualizarUbicacionGuardar(info.loc,info.cod_prodT,info.loteT,info.cod_prodR,info.loteR,info.usuario,info.ChkUbicar,info.Estab,info.cantidad,info.Pallet);
                    return Request.CreateResponse(HttpStatusCode.OK, "Información Guardara correctamente");
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
