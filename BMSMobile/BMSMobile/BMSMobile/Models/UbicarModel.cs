using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BMSMobile.Models
{
    public class UbicarModel
    {
        public ObservableCollection<Lote> Lotes { get; set; }

        public ObservableCollection<TipoDocumento> TipoDocuments { get; set; } = new ObservableCollection<TipoDocumento>() {
                new TipoDocumento { Nombre = "Ordenes de carga" },
                new TipoDocumento { Nombre = "Embarques" },
                new TipoDocumento { Nombre = "Picking" },
                new TipoDocumento { Nombre = "Salida por hospedaje" },
                new TipoDocumento { Nombre = "Pedido a establecimiento" },
                new TipoDocumento { Nombre = "Pedido salida por hospedaje" }
        };

        public ObservableCollection<Area> Areas { get; set; } = new ObservableCollection<Area>()
        {
            new Area { Nombre = "Bodega" },
            new Area { Nombre = "Picking" }
        };
        public ObservableCollection<Area> Tipo { get; set; } = new ObservableCollection<Area>()
        {
            new Area { Nombre = "Pallet" }
        };

        public ObservableCollection<Disponible> Disponibles { get; set; } = new ObservableCollection<Disponible>()
        {
            new Disponible {Nombre = "Disponible"},
            new Disponible {Nombre= "No Disponible"}
        };
    
    }

        public class Lote
        {
            public string Folio { get; set; } = "";
            public string CodEstab { get; set; } = "";
            public string CodProd { get; set; } = "";
            public string Descripcion { get; set; } = "";
            public decimal Cantidad { get; set; } = 0m;
            public string Unidad { get; set; } = "";
            public string AbreviaturaUnidad { get; set; } = "";
            public DateTime Fecha { get; set; } = DateTime.MinValue;
            public string FolioReferencia { get; set; } = "";
            public string TransaccionReferencia { get; set; } = "";
            public DateTime FechaCaducidad { get; set; } = DateTime.MinValue;
            public string LoteFabricacion { get; set; } = "";
            public string Localizacion { get; set; } = "";
            public string LocalizacionIntercambio { get; set; } = "";
            public bool Ubicado { get; set; } = false;
            public decimal Prodxpall { get; set; } = 0m;
            public decimal Picking { get; set; } = 0m;
            public decimal CantPendiente { get; set; } = 0m;
            public decimal CantUbicada { get; set; } = 0m;

            public Lote()
            {
                // Puedes configurar valores predeterminados específicos en el constructor si lo deseas.
                // Por ejemplo:
                // Folio = "ValorPredeterminado";
                // Cantidad = 0.0m;
            }
        }

        public class TipoDocumento
        {
            public string Nombre { get; set; }
        }

        public class Area
        {
            public string Nombre { get; set; }
        }
        public class Disponible
        {
           public string Nombre { get; set; }
        }

    public class tLotes
    {
        public string folio { get; set; }
        public string cod_estab { get; set; }
        public string cod_prod { get; set; }
        public string unidad { get; set; }
        public string abreviatura_unidad { get; set; }
        public System.DateTime fecha { get; set; }
        public string folio_referencia { get; set; }
        public string transaccion_referencia { get; set; }
        public System.DateTime fecha_caducidad { get; set; }
        public string lote_fabricacion { get; set; }
        public string localizacion { get; set; }
        public string localizacion_intercambio { get; set; }
        public bool ubicado { get; set; }
        public int prodxpall { get; set; }
        public int picking { get; set; }
        public int cant_ubicada { get; set; }
        public decimal cant_pendiente { get; set; }
        public string descripcion { get; set; }
    }
}

