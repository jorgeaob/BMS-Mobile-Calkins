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
    public class UbicarController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Refrescar(string folio ,string trans , string cod_estab ,bool pallet, bool disponible, int indexDoc, bool picking)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_UbicarDocumento(folio,trans,cod_estab,pallet,disponible,indexDoc,picking,"").ToList();
                    if (info == null || info.Count <= 0)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentró información.");
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
        public HttpResponseMessage Busqueda(string folio, string documento)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_UbicarBusqueda(folio, documento).ToList();
                    if (info == null || info.Count <= 0)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentró información.");
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
        public HttpResponseMessage Localizacion(string folio, string cod_prod,string cod_estab,string rack,string disponible,string pickslot)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_Ubicar_Localizacion(folio, cod_prod, cod_estab, int.Parse(rack), bool.Parse(disponible), bool.Parse(pickslot)).FirstOrDefault();
                    if (info == null || string.IsNullOrEmpty(info))
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentró información.");
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
        public HttpResponseMessage Guardar(string folio,string loc,bool disponible,bool picking,string cod_estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //foreach (var det in rec.listaDetalle)
                            //{

                            ObjectParameter guardo = new ObjectParameter("guardo", false);


                            var detalle = db.CalkinsWS_Ubicar_Guardar(folio,loc,disponible,picking,cod_estab,guardo);
                            if ((bool)guardo.Value == false)
                            {
                                dbContextTransaction.Rollback();
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "");
                            }                          
                            else
                            {
                                dbContextTransaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.OK, (bool)guardo.Value);
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

        [HttpGet]
        public HttpResponseMessage UbicacionSugerida(string Estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var estab = db.establecimientos
                                .Where(x => x.cod_estab == Estab)
                                .Select(x => x.manejo_ubicacion_sugerida)
                                .FirstOrDefault();

                    if (estab != null)
                    {
                        // El valor de manejo_ubicacion se encuentra en la variable 'estab'
                        // Puedes utilizarlo como lo necesites
                        string valorManejoUbicacion = estab.ToString();
                    }
                    if (estab == "" || estab == null)
                        return Request.CreateResponse(HttpStatusCode.OK, "O");
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
    }
}
