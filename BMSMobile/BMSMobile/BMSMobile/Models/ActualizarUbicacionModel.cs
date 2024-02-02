using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class ActualizarUbicacionModel
    {
    }

    public partial class CalkinsWS_ActualizarUbicacionLoc_Result
    {
        public string cod_estab { get; set; }
        public bool pick_slot { get; set; }
        public bool disponible { get; set; }
        public string cod_prod { get; set; }
        public string lote { get; set; }
    }
    public partial class CalkinsWS_ActualizarUbicacionLote_Result
    {
        public string cod_prod { get; set; }
        public string cod_estab { get; set; }
        public decimal cantidad { get; set; }
        public string unidad { get; set; }
        public System.DateTime fecha_caducidad { get; set; }
        public string lote_fabricacion { get; set; }
        public string descripcion { get; set; }
    }

    public class GuardarActualizarUbicacion 
    {
        public string loc { get; set; }
        public string cod_prodT { get; set ; }
        public string loteT { get; set; }
        public string cod_prodR { get; set; }  
        public string usuario { get; set; }
        public string loteR { get; set; }
        public decimal cantidad { get; set; }
        public string Estab { get; set; }
        public string Pallet { get; set; }
        public bool ChkUbicar { get; set; }

    }
}
