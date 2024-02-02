using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Models
{
    public class InfoConteoModel
    {
        public string folio { get; set; }
        public System.DateTime fecha { get; set; }
        public string cod_estab { get; set; }
        public string usuario { get; set; }
        public bool registrado { get; set; }

        public List<CalkinsWS_DetalleConteo_Result> ProductosConteo { get; set; }
    }   
    
    public class GuardarConteoModel
    {
        public string folio { get; set; }
        public string cod_prod { get; set; }
        public System.DateTime fecha { get; set; }
        public string usuario { get; set; }
        public decimal unidades_compra { get; set; }
        public decimal unidades_alternativas { get; set; }
        public decimal exist_unidades_compra { get; set; }
        public decimal exist_unidades_alternativas { get; set; }
        public string programacion { get; set; }
        public string notas { get; set; }        
        public bool contado { get; set; }        
    }
}