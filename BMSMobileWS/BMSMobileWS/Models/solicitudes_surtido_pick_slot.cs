//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMSMobileWS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class solicitudes_surtido_pick_slot
    {
        public string folio { get; set; }
        public string cod_prod { get; set; }
        public System.DateTime fecha { get; set; }
        public Nullable<System.DateTime> fecha_surtido { get; set; }
        public string usuario { get; set; }
        public string usuario_surtido { get; set; }
        public string status { get; set; }
        public string orden_carga { get; set; }
        public string localizacion { get; set; }
    }
}