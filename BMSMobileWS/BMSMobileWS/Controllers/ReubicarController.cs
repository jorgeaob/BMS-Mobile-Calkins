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
    public class ReubicarController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage RazonesND()
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.razones_no_disponible
                        .Where(r => r.status.ToUpper().Trim() == "V")
                        .OrderBy(r => r.nombre)
                        .Select(r => new
                        {
                            razon = r.razon_nodisponible.Trim(),
                            nombre = r.nombre.Trim()
                        })
                        .ToList(); 

                    if (info == null || info.Count == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró información.");
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
        public HttpResponseMessage ValidaLocProducto(string localizacion, string cod_estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_ReubicarProducto(localizacion.Trim(),cod_estab.Trim()).ToList();

                    if (info == null || info.Count == 0)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró información.");
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
        public HttpResponseMessage LocDisponibles(string cod_prod, string cod_estab,bool montacargas)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_ReubicarLocDisponibles(cod_prod.Trim(), cod_estab.Trim(),montacargas).FirstOrDefault();

                    if (info == null || info == "")
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró información.");
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
        public HttpResponseMessage NuevaLoc(string localizacion)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {

                    var consulta = from l in db.localizaciones
                                   where l.localizacion == localizacion && l.disponible == true
                                   select new
                                   {
                                       l.localizacion,
                                       picking = l.pick_slot,
                                       l.mercancia_disponible,
                                       l.multiples_productos,
                                       productos = db.inventario_localizacion.Count(il => il.localizacion == localizacion)
                                   };

                    var resultado = consulta.FirstOrDefault();


                    if (resultado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró información.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, resultado);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage ValidaSolicitud(string Solicitud,string CodProd)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {

                    var query = from s in db.solicitudes_surtido_pick_slot
                                where s.orden_carga.Trim() == Solicitud && s.cod_prod.Trim() == CodProd && s.status.Trim().ToUpper() == "V"
                                select new
                                {
                                    folio = s.folio.Trim() ?? ""
                                };

                    var resultado = query.FirstOrDefault();

                    string folio = resultado?.folio ?? "";


                    if (folio == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró información.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, folio);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage NoDisponible(string cod_prod ,string unidad,string trans, decimal cant ,string cod_estab ,string razonND ,string usuario)
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


                            var detalle = db.CalkinsWS_ModificaNoDisponible(cod_prod,unidad,trans,cant,cod_estab,msg,razonND,usuario);
                            if (msg.Value.ToString() != "")
                            {
                                dbContextTransaction.Rollback();
                                return Request.CreateResponse(HttpStatusCode.OK, msg.Value.ToString());
                            }
                            else
                            {
                                dbContextTransaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.OK,"");
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
