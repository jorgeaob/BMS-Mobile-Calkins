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
    public class EntradaController : ApiController
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]

        public HttpResponseMessage TraerOrdenCompra(string Folio,string Transaccion, string Estab, string Usuario)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_OrdenCompra(Folio,Transaccion,Estab,Usuario).SingleOrDefault();

                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontro información con el folio");
                    else
                    {
                        var det = db.CalkinsWS_OrdenCompraDetalle(Folio, Transaccion, Estab, Usuario).ToList();

                        if (det.Count <= 0 || det == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontro los productos del folio");
                        }
                        else
                        {
                            var todo = new EntradaModel
                            {
                                cabecero = info,
                                listaDetalle = det
                            };

                            return Request.CreateResponse(HttpStatusCode.OK, todo);
                        }
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
        public HttpResponseMessage ValidarProducto(string Cod_prod, string Cod_estab, string OC, string Caducidad, string EsLote, string Cant, string unidad)
        {
            try
            {

                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    // Convertir Caducidad a DateTime
                    DateTime caducidad;
                    if (!DateTime.TryParse(Caducidad, out caducidad))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "La fecha de caducidad no es válida.");
                    }

                    // Convertir EsLote a bool
                    bool esLote;
                    if (!bool.TryParse(EsLote, out esLote))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "El valor de EsLote no es válido. Debe ser 'true' o 'false'.");
                    }

                    // Convertir Cant a decimal
                    decimal cantidad;
                    if (!decimal.TryParse(Cant, out cantidad))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "La cantidad no es válida.");
                    }


                    var mensaje = bd.CalkinsWS_ValidarProdEntrada(Cod_prod,Cod_estab,OC,caducidad, esLote, cantidad, unidad).SingleOrDefault();
                    //if (mensaje == null)
                    //{
                    //    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de producto no válido");
                    //    //prod = new CalkinsWS_ProductoEntrada_Result();
                    //}
                    return Request.CreateResponse(HttpStatusCode.OK, mensaje);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage TraerProducto(string Cod_prv, string Cod_estab, string Cod_prod)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var prod = bd.CalkinsWS_ProductoEntrada(Cod_prv, Cod_estab, Cod_prod).SingleOrDefault();
                    if (prod == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de producto no válido");
                        //prod = new CalkinsWS_ProductoEntrada_Result();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, prod);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage GenerarFolio(string Transaccion, string EstabCode)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    ObjectParameter folio = new ObjectParameter("Folio", typeof(string));
                    ObjectParameter maxreng = new ObjectParameter("MaxReng", typeof(int));
                    ObjectParameter unidad = new ObjectParameter("UnidadPermitida", "A");
                    var genFol = bd.CalkinsWS_GenerarFolio(Transaccion, EstabCode, true, folio, maxreng,"1", unidad);
                    return Request.CreateResponse(HttpStatusCode.OK, new { Folio = folio.Value, MaxReng = maxreng.Value, UnidadPermitida = unidad.Value });
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GuardarEntrada(Entrada rec)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var i = 0;
                            foreach (var det in rec.entradaDetalles)
                            {
                            //    ObjectParameter cant = new ObjectParameter("cant", det.cantidad);
                                
                                ObjectParameter mensaje = new ObjectParameter("Msg", typeof(string));

                                var detalle = db.CalkinsWS_GuardarDetalleEntrada(rec.Folio,rec.OC,0,det.cod_prod,det.cantidad,(string)det.unidad, det.pallet,det.fecha_caducidad,det.lote_fab,det.notas,0,rec.Estab,rec.Proveedor, mensaje);

                                if (detalle <= 0)
                                {
                                    dbContextTransaction.Rollback();
                                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo guardar la información.");
                                }
                            }

                            var cabecero = db.CalkinsWS_GuardarEntrada(rec.Folio, rec.Usuario, rec.Estab,rec.Notas,rec.OC,rec.Proveedor);


                            if (cabecero <= 0)
                            {
                                dbContextTransaction.Rollback();
                                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo guardar la información.");
                            }
                            else
                            {
                                dbContextTransaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.OK, new { Resultado = cabecero });
                            }
                        }
                        catch (Exception ex)
                        {

                            log.Error("Error: ", ex);
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                        }
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
        public HttpResponseMessage TraerProveedor(string Prv)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var prv = bd.proveedores.Where(x => x.cod_prv == Prv).SingleOrDefault();
                    if ( prv == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Proveedor no válido");
                        //prod = new CalkinsWS_ProductoEntrada_Result();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, prv);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]

        public HttpResponseMessage BuscarProveedores(string CodPrv)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.proveedores
                                 .Where(proveedor => proveedor.cod_prv.Contains(CodPrv) || proveedor.razon_social.Contains(CodPrv))
                                 .ToList();

                    if (info == null || info.Count <= 0)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontro información.");
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

    }
}
