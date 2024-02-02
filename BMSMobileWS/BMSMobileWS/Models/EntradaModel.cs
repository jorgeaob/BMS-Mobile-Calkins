using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Models
{
    public class EntradaModel
    {
        public CalkinsWS_OrdenCompra_Result cabecero { get; set; }
        public List<CalkinsWS_OrdenCompraDetalle_Result> listaDetalle { get; set; }
    }
}