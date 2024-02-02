using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class SurtidoPickingModel
    {
    }

    public class Tickets
    {
        public string ticket { get; set; }
        public Nullable<bool> contenedor { get; set; }
        public string razon_social { get; set; }
        public string cod_cte { get; set; }
        public string numdpc { get; set; }
    }

    public class ProductosTicket
    {
        public int id { get; set; }
        public short ticket { get; set; }
        public string cod_prod { get; set; }
        public string descripcion_completa { get; set; }
        public string unidad { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidad_surtida { get; set; }
        public string contenedor { get; set; }
        public Nullable<decimal> existencia { get; set; }
    }

    public class GuardarSurtidoPicking
    {
        public string Folio { get; set; }
        public string Trans { get; set; }
        public string Ticket { get; set; }
        public string CodCte { get; set; }
        public string Numdpc { get; set; }
        public string Prod { get; set; }
        public string Unid { get; set; }
        public string Localizacion { get; set; }
        public decimal Cant { get; set; }
        public string Estab { get; set; }
        public string Usuario { get; set; }
        public string Contenedor { get; set; }
        public int TipoPick { get; set; }
    }

    public partial class SurtidoPickingValidaLoc
    {
        public string localizacion { get; set; }
        public string cod_prod { get; set; }
        public decimal cantidad { get; set; }
    }


}
