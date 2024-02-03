using BMSMobileWS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BMSMobileWS.Controllers
{
    public class InventarioController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [HttpGet]

        public HttpResponseMessage ConsultaInventario(string Folio)
        {
            try
            {
                using(BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_InfoConteo(Folio).SingleOrDefault();

                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontro información con el folio");
                    else
                    {
                        var det = db.CalkinsWS_DetalleConteo(Folio).ToList();

                        if(det.Count <= 0 || det == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontro los productos del folio");
                        }
                        else
                        {
                            var todo = new InfoConteoModel
                            {
                                folio = info.folio.Trim(),
                                fecha = info.fecha,
                                cod_estab = info.cod_estab.Trim(),
                                usuario = info.usuario.Trim(),
                                registrado = info.registrado,
                                ProductosConteo = det 
                            };

                            return Request.CreateResponse(HttpStatusCode.OK, todo);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]

        public HttpResponseMessage BuscarProductos(string Codigo, string Filtro, string Estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_BuscarProductos(Codigo,Filtro,Estab).ToList();

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

        [HttpPost]
        public HttpResponseMessage GuardarCantidadesConteo(GuardarConteoModel conteo)
        {            
            using (BMS2015Entities db = new BMS2015Entities())
            {
                using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var save = db.CalkinsWS_GuardarConteo(conteo.folio, conteo.cod_prod, conteo.fecha, conteo.usuario, conteo.unidades_compra, 
                            conteo.unidades_alternativas, conteo.exist_unidades_compra, conteo.exist_unidades_alternativas, conteo.programacion, conteo.notas, conteo.contado);
                        dbContextTransaction.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { Resultado = save });
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        log.Error("Error", ex);
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
            }
           
        }

        [HttpGet]

        public HttpResponseMessage CheckCodigoBarras(string CodigoBarras, string Estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_CheckEscanerProd(CodigoBarras, Estab).SingleOrDefault();

                    if (info == null)
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

        [HttpPost]
        public HttpResponseMessage GuardarNotasConteo(string Folio, string CodProd, string Notas)
        {
            using (BMS2015Entities db = new BMS2015Entities())
            {
                using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var save = db.CalkinsWS_GuardarNotasConteoProd(Folio, CodProd, Notas);
                        dbContextTransaction.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { Resultado = save });
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        log.Error("Error", ex);
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage InventarioInicial(string CodProd, string CodEstab, string Fecha)
        {
            try
            {
                DateTime fechaAux = DateTime.ParseExact(Fecha, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var result = db.CalkinsWS_InventarioProductoFecha(CodProd, CodEstab, fechaAux).SingleOrDefault();
                    //var result = db.Database.SqlQuery<Decimal>($"SELECT dbo.inventario_inicial('{CodProd}','{CodEstab}','{Fecha}')").SingleOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error: ", ex);   
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage FechaHoraServer()
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var result = db.Database.SqlQuery<DateTime>($"SELECT GETDATE()").SingleOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error: ", ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage ModificaInventarioLoc(ModificaInventarioLoc info)
        {
            using (BMS2015Entities db = new BMS2015Entities())
            {
                using (DbContextTransaction dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter cant = new ObjectParameter("cant", info.Cantidad);
                        var save = db.CalkinsWS_ModificaInventarioLoc(info.CodProd,info.Unidad,info.Localizacion,info.Lote,info.CodEstab,info.AfectaNoDisponible, cant,true,System.DateTime.Now,info.LoteRecepcion,info.Transaccion,info.FolioRef,info.TransRef,"");
                        dbContextTransaction.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK,save);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        log.Error("Error", ex);
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
            }

        }
    }
}