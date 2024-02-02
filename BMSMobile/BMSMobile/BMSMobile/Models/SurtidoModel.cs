using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class SurtidoModel
    {      
            public string folio_surtido { get; set; }
            public string transaccion_surtido { get; set; }
            public string cod_prod { get; set; }
            public int id_origen { get; set; }
            public decimal cantidad_surtida { get; set; }
            public string descripcion { get; set; }
            public string ticket { get; set; }
            public string localizacion { get; set; }
            public string folio { get; set; }
            public string transaccion { get; set; }
            public string cod_cte { get; set; }
            public string numdpc { get; set; }
            public string localizaciones { get; set; }
       
    }

    public class PalletLoc
    {
        public string Cod_prod { get; set; }
        public string Pallet { get; set; }
    }

    public class GuardarSurtido
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
}
