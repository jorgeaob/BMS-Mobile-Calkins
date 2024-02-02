using BMSMobileWS.Models;
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
    public class NuevoLoteController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GuardarNuevoLote(NuevoLoteModel rec)
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
 

                            var detalle = db.CalkinsWS_NuevoLote(rec.Folio, rec.CodEstab, rec.CodProd, decimal.Parse(rec.Cantidad), "", "", rec.FolioReferencia, rec.TransReferencia, rec.FechaCaducidad, rec.LoteFab, rec.Localizacion, rec.LoteRecep, msg);

                                if (detalle <= 0)
                                {
                                    dbContextTransaction.Rollback();
                                return Request.CreateErrorResponse(HttpStatusCode.Conflict, msg.Value.ToString()); ;
                                }
                           
                            else
                            {
                                dbContextTransaction.Commit();
                                return Request.CreateResponse(HttpStatusCode.OK,"Guardado");
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
