using BMSMobileWS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BMSMobileWS.Controllers
{
    public class SurtidoPickingController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Refrescar(string folio, string cod_estab, string trans)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    ObjectParameter msg = new ObjectParameter("msg", "");
                    var info = db.CalkinsWS_SurtidoPicking_Refrescar(folio.Trim(),trans.Trim(),cod_estab.Trim(), msg).ToList();

                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, msg.Value.ToString());
                    else
                    {
                        if (msg.Value.ToString() != "")
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, msg.Value.ToString());
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, info);
                    }                   
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage TraerProdTickets(string folio, string cod_estab, string trans,string ticket)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    ObjectParameter msg = new ObjectParameter("msg", "");
                    var info = db.CalkinsWS_SurtidoPickingProdTicket(folio.Trim(), trans.Trim(), cod_estab.Trim(), ticket.Trim()).ToList();

                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Este ticket no tiene productos pendientes");
                    else
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
        public HttpResponseMessage LocProductoIntegracion(string cod_cte, string cod_estab, string numdpc, string cod_prod)
        {
            try
            {
                if (cod_estab is null) { cod_estab = ""; } if (numdpc is null) { numdpc = ""; } if(cod_prod is null) { cod_prod = ""; }
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    ObjectParameter msg = new ObjectParameter("msg", "");
                    var info = db.CalkinsWS_LocProductoIntegracion(cod_cte.Trim(), numdpc.Trim(), cod_prod.Trim(), cod_estab.Trim()).FirstOrDefault();

                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
                    else
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
        public HttpResponseMessage ValidarLocalizacion(string loc, string estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    ObjectParameter msg = new ObjectParameter("msg", "");
                    var info = db.CalkinsWS_SurtidoPickingValidaLoc(loc.Trim(),estab.Trim()).FirstOrDefault();

                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, info);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage Guardar(GuardarSurtidoPicking info)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
         
                            ObjectParameter msg = new ObjectParameter("msg", "");

                            var detalle = db.CalkinsWS_SurtidoPickingGuardar(info.Folio.Trim(), info.Trans.Trim(),info.Ticket, info.CodCte.Trim(),info.Numdpc, info.Prod.Trim(), info.Unid.Trim(), info.Localizacion.Trim(), info.Cant, info.Estab.Trim(), info.Usuario.Trim(),info.Contenedor.Trim(),info.TipoPick, msg);
                            if (msg.Value.ToString() != "")
                            {
                                dbContextTransaction.Rollback();
                                return Request.CreateResponse(HttpStatusCode.OK, msg.Value.ToString());
                            }
                            else
                            {
                                dbContextTransaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.OK, msg.Value.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
