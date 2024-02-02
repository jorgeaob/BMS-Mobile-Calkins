using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Controllers
{
    public class ModificaInventarioLoc
    {
        public string CodProd { get; set; }
        public string Unidad { get; set; }
        public string Localizacion { get; set; }
        public string Lote { get; set; }
        public string CodEstab { get; set; }
        public bool AfectaNoDisponible { get; set; }
        public decimal Cantidad { get; set; }
        public bool Guardar { get; set; }
        public DateTime Fecha { get; set; }
        public string LoteRecepcion { get; set; }
        public string Transaccion { get; set; }
        public string FolioRef { get; set; }
        public string TransRef { get; set; }
        public string MSG { get; set; }
    } 

}