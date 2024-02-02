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
    public class EntradaDevolucionesController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage RazonesDevClientes()
        {
            try
            {
                using (BMS2015Entities bd = new BMS2015Entities())
                {
                    var razones = bd.razones_devoluciones_clientes.Where(x => x.status.Trim().ToUpper() == "V").ToList();
                    if (razones == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Razones no encontradas");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, razones);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage InfoDocumento(string Folio,string Trans)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var mercancia = db.CalkinsWS_EntradaDEvolucion(Folio,Trans).SingleOrDefault();

                    if (mercancia == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de documento invalido");
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
        public HttpResponseMessage DetalleEntradaDevolcion(string Folio, string Trans)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var mercancia = db.CalkinsWS_DetalleEntradaDevolucion(Folio,Trans).ToList();
                    if (mercancia == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Código de documento invalido");
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
        public HttpResponseMessage GuardarEntradaDevolucion(RecepcionTransferenciaModel rec)
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

                                var detalle = db.CalkinsWS_GuardarDetalleEntradaDevolucion(det.cod_prod, det.unid, det.cod_estab, det.folioTransferencia, det.transaccion, det.backorder, det.afecta_no_disponible, cant, det.costo_operativo_unitario, costoPromedio,
                                    det.cod_prv, det.Cantidad_adicional, det.unidad, det.cantidad, det.precio_lista, det.tipo_precio_venta, det.importe, det.iva, det.ieps, det.costo, det.peso, det.volumen, det.id_origen, det.abreviatura_unidad, det.fecha_caducidad, det.lote_fabricacion, det.pallets, det.agregar_lote, msg);

                                if (detalle <= 0)
                                {
                                    dbContextTransaction.Rollback();
                                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo guardar la información.");
                                }
                            }

                            var cabecero = db.CalkinsWS_GuardarEntradaDevolucion(rec.folio, rec.transaccion, rec.transaccion_origen, rec.cod_estab, rec.cod_estab_alterno, rec.folio_transferencia, rec.importeGeneral,
                                rec.ivaGeneral, rec.iepsGeneral, rec.costoGeneral, rec.unidadesGeneral, rec.piezasGeneral, rec.pesoGeneral, rec.volumenGeneral, rec.usuario, rec.razon_aod_inventario, rec.folio_referencia, rec.orden_embarque,
                                rec.recoge_mercancia, rec.notasGeneral, rec.clave_afectacion_inventario, rec.Operacion,rec.cod_cte);


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
