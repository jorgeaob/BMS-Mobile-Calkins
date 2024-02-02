using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class RetirarMercanciaModel
    {
    }

    public class Localizacion
    {
        public string cod_prod { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public string lote_recepcion { get; set; }
        public string folio_referencia { get; set; }
        public string localizacion { get; set; }
        public bool multiples_productos { get; set; }
        public bool pick_slot { get; set; }
        public Nullable<int> productos { get; set; }
    }

    public class ProductoLoc
    {
        public string cod_prod { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public string lote_recepcion { get; set; }
        public string folio_referencia { get; set; }
    }

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
