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
    
    public partial class MobileBMS_BuscarProductos_Result
    {
        public string cod_prod { get; set; }
        public string descripcion_completa { get; set; }
        public decimal contenido { get; set; }
        public decimal costo_promedio { get; set; }
        public string presentacion { get; set; }
        public string forma_expresar_inventario { get; set; }
        public Nullable<decimal> exist_unidades { get; set; }
        public Nullable<decimal> exist_piezas { get; set; }
        public string NombreUC { get; set; }
        public string NombreUA { get; set; }
        public string AbrevUC { get; set; }
        public string AbrevUA { get; set; }
        public string codigo_barras_unidad { get; set; }
        public string codigo_barras_pieza { get; set; }
    }
}