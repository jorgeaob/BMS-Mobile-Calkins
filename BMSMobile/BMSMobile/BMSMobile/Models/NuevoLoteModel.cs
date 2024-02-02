using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
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
        public string LoteRecep { get; set; }
        public  string Localizacion { get; set; }
        public NuevoLoteModel() {
            Folio = string.Empty;      
        }
        
    }


}
