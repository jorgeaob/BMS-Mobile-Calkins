using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Models
{
    public class SurtidoModel
    {
        public string Pallet { get; set; }
        public string Loc { get; set; }
        public string CodProd { get; set; }
        public string Ticket { get; set; }
        public string FolioSurtido { get; set; }
        public string CantSurtida { get; set; }
        public string TransSurtido { get; set; }
        public string Trans { get; set; }
        public string Folio { get; set; }
        public string CodEstab { get; set; }
        public string Usuario { get; set; }
        public string Documento { get; set; }
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
}