using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class ReubicarMercanciaModel
    {
    }

    public class RazonND
    {
        public string razon { get; set; }   
        public string nombre { get; set; }
    }

    public partial class ReubicarProducto
    {
        public string cod_prod { get; set; }
        public string descripcion { get; set; }
        public string lote { get; set; }
        public decimal exist_unidades { get; set; }
        public decimal exist_piezas { get; set; }
        public System.DateTime fecha { get; set; }
        public string lote_recepcion { get; set; }
        public string localizacion { get; set; }
        public string cod_estab { get; set; }
        public bool multiples_productos { get; set; }
        public bool mercancia_disponible { get; set; }
        public bool picking { get; set; }
        public bool mercancia_disponible1 { get; set; }
        public Nullable<int> prodxpall { get; set; }
        public decimal costo { get; set; }
        public string unidad_compra { get; set; }
        public float volumen { get; set; }
        public decimal peso { get; set; }
        public decimal ieps { get; set; }
    }
    public class SqlNuevaLoc
    {
        public string localizacion { get; set; }
        public bool picking { get; set; }
        public bool mercancia_disponible { get; set; }
        public bool multiples_productos { get; set; }
        public int productos { get; set; }
    }
}
