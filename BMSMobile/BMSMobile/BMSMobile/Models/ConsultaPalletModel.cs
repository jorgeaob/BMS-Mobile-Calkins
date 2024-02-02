using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class ConsultaPalletModel
    {   
            public string cod_prod { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
            public decimal cantidad { get; set; } = decimal.Zero;
            public string localizacion { get; set; } = string.Empty;
            public string folio { get; set; } = string.Empty;               
            public System.DateTime fecha { get; set; } = DateTime.Now;
            public System.DateTime fecha_caducidad { get; set; } = DateTime.Now;
            public string folio_referencia { get; set; } = string.Empty;
            public string lote_fabricacion { get; set; } = string.Empty;      
    }
}
