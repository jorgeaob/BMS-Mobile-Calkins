using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class EntradasSalidasLocModel
    {
        public string cod_prod { get; set; }
        public decimal cantidad { get; set; }
        public string tipo { get; set; }
        public System.DateTime fecha { get; set; }
        public string pallet { get; set; }
        public string nombre { get; set; }
        public string localizacion { get; set; }
    }
}
