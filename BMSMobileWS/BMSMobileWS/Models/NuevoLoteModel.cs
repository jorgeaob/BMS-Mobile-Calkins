using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Models
{
    public class NuevoLoteModel
    {
        public string Folio { get; set; }
        public string CodEstab { get; set; }
        public string CodProd { get; set; }
        public string Cantidad { get; set; }
        public string FolioReferencia { get; set; }
        public string TransReferencia { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string LoteFab { get; set; }
        public string Localizacion { get; set; }
        public string LoteRecep { get; set; }
    }
}