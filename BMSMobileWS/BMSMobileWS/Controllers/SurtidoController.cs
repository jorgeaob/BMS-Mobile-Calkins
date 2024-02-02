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
    public class SurtidoController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Refrescar(string folio, int index, string cod_estab)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    var info = db.CalkinsWS_Surtido(index,cod_estab,folio).ToList();
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
        public HttpResponseMessage PalletLoc(string Loc)
        {
            try
            {
                using (BMS2015Entities db = new BMS2015Entities())
                {
                    //Item resultado = items.FirstOrDefault(item => item.Localizacion == "Loc");

                    var info = db.inventario_localizacion.FirstOrDefault(x => x.localizacion == Loc.Trim());
                    if (info == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentró información.");
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, new { Cod_prod = info.cod_prod.Trim(), Pallet = info.lote_recepcion.Trim() });
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage Guardar(SurtidoModel info)
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

                            ObjectParameter msg = new ObjectParameter("msg", "");


                            var detalle = db.CalkinsWS_SurtidoGuardar(info.Loc.Trim(),info.CodProd.Trim(), int.Parse(info.Ticket),info.FolioSurtido.Trim(),decimal.Parse(info.CantSurtida),info.TransSurtido.Trim(),info.Trans.Trim(),info.Folio.Trim(),info.CodEstab.Trim().Trim(),info.Usuario.Trim(),info.Documento.Trim(),msg);
                            if (msg.Value.ToString() != "")
                            {
                                dbContextTransaction.Rollback();
                                return Request.CreateResponse(HttpStatusCode.OK,msg.Value.ToString());
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
