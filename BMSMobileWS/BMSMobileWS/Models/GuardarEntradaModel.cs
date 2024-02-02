using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace BMSMobileWS.Models
{
    public class GuardarEntradaModel
    {
        public Entrada Entrada { get; set; }

    }

    public class EntradaDetalle
    {
        public string cod_prod { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public int cantidad { get; set; } = 0;
        public object unidad { get; set; } = null;
        public decimal peso { get; set; } = 0.0m;
        public decimal volumen { get; set; } = 0.0m;
        public string abreviatura_unidad { get; set; } = string.Empty;
        public string notas { get; set; } = string.Empty;
        public DateTime fecha_caducidad { get; set; } = DateTime.MinValue;
        public bool pallet { get; set; } = false;
        public string lote_fab { get; set; } = string.Empty;
        public bool guardado { get; set; } = false;
        public bool guardando { get; set; } = false;
        public int id_meos { get; set; } = 0;
        public int id_mped { get; set; } = 0;


        public EntradaDetalle() { }
    }

    public class Entrada
    {
        public ObservableCollection<EntradaDetalle> entradaDetalles { get; set; }

        public string OC { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Tabla { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Estab { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public string Msg { get; set; } = string.Empty;
        public string Folio { get; set; } = string.Empty;
        public bool Terminar { get; set; } = false;
        public int IdMeos { get; set; } = 0;
        public int IdMped { get; set; } = 0;

        public Entrada()
        {
            entradaDetalles = new ObservableCollection<EntradaDetalle>();
        }
    }
}