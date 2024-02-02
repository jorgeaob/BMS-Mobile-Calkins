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
    public class RecepcionTransferenciaController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage InfoTransferencia(string Folio)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var mercancia = db.CalkinsWS_InfoTransferencia(Folio).SingleOrDefault();

                    if (mercancia == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de transferencia invalido");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, mercancia);
                }
            }
            catch (Exception ex)
            {
              
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage DetalleRecepcionTransferencia(string Folio)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var mercancia = db.CalkinsWS_DetalleRecepcionTransferencia(Folio).ToList();
                    if (mercancia == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de transferencia invalido");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, mercancia);
                }
            }
            catch (Exception ex)
            {
             
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage ProductosSurtidosTransferencia(string Folio)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var mercancia = db.CalkinsWS_ProductosSurtidos(Folio).ToList();
                    if (mercancia == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de transferencia invalido");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, mercancia);
                }
            }
            catch (Exception ex)
            {
             
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage GenerarFolio(string Transaccion, string CodEstab)
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    ObjectParameter folio = new ObjectParameter("Folio", typeof(string));
                    ObjectParameter maxreng = new ObjectParameter("MaxReng", typeof(int));
                    ObjectParameter unidad = new ObjectParameter("UnidadPermitida", "A");


                    var genFol = bd.CalkinsWS_GenerarFolio(Transaccion, CodEstab, true, folio, maxreng, "1", unidad);
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { Folio = folio.Value, MaxReng = maxreng.Value, UnidadPermitida = unidad.Value });
                }
            }
            catch (Exception ex)
            {
              
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage GuardarRecepcionTransferencia(RecepcionTransferenciaModel rec)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var det in rec.listaDetalle)
                            {
                                ObjectParameter cant = new ObjectParameter("cant", det.cant);
                                ObjectParameter msg = new ObjectParameter("msg", "");
                                ObjectParameter costoPromedio = new ObjectParameter("costo_promedio", typeof(decimal));

                                var detalle = db.CalkinsWS_GuardarDetalleRecepcion(det.cod_prod, det.unid, det.cod_estab, det.folioTransferencia, det.transaccion, det.backorder, det.afecta_no_disponible, cant, det.costo_operativo_unitario, costoPromedio,
                                    det.cod_prv, det.Cantidad_adicional, det.unidad, det.cantidad, det.precio_lista, det.tipo_precio_venta, det.importe, det.iva, det.ieps, det.costo, det.peso, det.volumen, det.id_origen, det.abreviatura_unidad,det.fecha_caducidad,det.lote_fabricacion,det.pallets,det.agregar_lote, msg);

                                if (detalle <= 0)
                                {
                                    dbContextTransaction.Rollback();
                                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo guardar la información.");
                                }
                            }

                            var cabecero = db.CalkinsWS_GuadarRecepcionTransferencia(rec.folio, rec.transaccion, rec.transaccion_origen, rec.cod_estab, rec.cod_estab_alterno, rec.folio_transferencia, rec.importeGeneral,
                                rec.ivaGeneral, rec.iepsGeneral, rec.costoGeneral, rec.unidadesGeneral, rec.piezasGeneral, rec.pesoGeneral, rec.volumenGeneral, rec.usuario, rec.razon_aod_inventario, rec.folio_referencia, rec.orden_embarque,
                                rec.recoge_mercancia, rec.notasGeneral, rec.clave_afectacion_inventario, rec.Operacion);


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

