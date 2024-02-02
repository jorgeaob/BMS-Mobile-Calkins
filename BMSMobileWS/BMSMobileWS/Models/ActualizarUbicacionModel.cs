using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Models
{
    public class ActualizarUbicacionModel
    {
        public string loc { get; set; }
        public string cod_prodT { get; set; }
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