using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BMSMobile.Models
{
    public class RecepcionTransferenciaModel
    {
        public string folio { get; set; }
        public string transaccion { get; set; }
        public System.DateTime fecha { get; set; }
        public string cod_estab { get; set; }
        public string NombreEstab { get; set; }
        public string cod_estab_alterno { get; set; }
        public string NombreEstabAlterno { get; set; }
        public decimal importe { get; set; }
        public decimal iva { get; set; }
        public decimal ieps { get; set; }
        public decimal piezas { get; set; }
        public decimal peso { get; set; }
        public decimal volumen { get; set; }
        public string usuario { get; set; }
        public string status { get; set; }
        public bool surtido { get; set; }
        public Nullable<decimal> total { get; set; }
        public string status_recepcion { get; set; }
        public decimal costo { get; set; }
        public decimal unidades { get; set; }
        public string razon_aod_inventario { get; set; }
        public string clave_afectacion_inventario { get; set; }
        public string notas { get; set; }
        public string cod_cte { get; set; }
        public string razon_social { get; set; }
        public List<RecepcionTransferenciaDetalleModel> listaDetalle { get; set; }

        //public ObservableCollection<Inventario> inventario {get; set;} = new ObservableCollection<Inventario> {
        //    new Inventario { nombre = "Disponible" },
        //    new Inventario {nombre = "No Disponible"}
        //};

        public RecepcionTransferenciaModel()
        {
            folio = "";
            transaccion = "";
            //transaccion_origen = "";
            //cod_estab = "";
            //cod_estab_alterno = "";
            //folio_transferencia = "";
            //importeGeneral = 0m;
            //ivaGeneral = 0m;
            //iepsGeneral = 0m;
            //costoGeneral = 0m;
            //unidadesGeneral = 0m;
            //piezasGeneral = 0m;
            //pesoGeneral = 0m;
            //volumenGeneral = 0m;
            //usuario = "";
            //razon_aod_inventario = "";
            //folio_referencia = "";
            //orden_embarque = 0;
            //recoge_mercancia = false;
            //notasGeneral = "";
            //clave_afectacion_inventario = "";
            //Operacion = "";
            cod_cte = "";
            razon_social = "";
            listaDetalle = new List<RecepcionTransferenciaDetalleModel>();
        }
    }

    public class RecepcionTransferenciaDetalleModel
    {
        public int id { get; set; }
        public string folio { get; set; }
        public string transaccion { get; set; }
        public System.DateTime fecha { get; set; }
        public string cod_prod { get; set; }
        public string descripcion_completa { get; set; }
        public string unidad { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio_lista { get; set; }
        public decimal importe { get; set; }
        public decimal iva { get; set; }
        public decimal ieps { get; set; }
        public decimal costo { get; set; }
        public Nullable<decimal> utilidad_bruta { get; set; }
        public decimal peso { get; set; }
        public decimal volumen { get; set; }
        public string cod_estab { get; set; }
        public string abreviatura_unidad { get; set; }
        public Nullable<decimal> total { get; set; }
        public string codigo_unidad { get; set; }
        public string NombreUnidad { get; set; }
        public string UsaFichaTecProd { get; set; }
        public Nullable<bool> inventario_identificado { get; set; }
        public Nullable<bool> control_etiqueta { get; set; }
        public string nom_talla { get; set; }
        public string comentario_venta { get; set; }
        public string codigo_barras_unidad { get; set; }
        public string codigo_barras_pieza { get; set; }
        public Nullable<decimal> contenido { get; set; }
        public Nullable<decimal> costo_promedio { get; set; }
        public string tipo_precio_venta { get; set; }
        public Nullable<short> cama { get; set; }
        public Nullable<short> altura { get; set; }
        public Nullable<short> caducidad_fabricacion { get; set; }
        public string familia { get; set; }
        public string codigoEscaneado { get; set; } 
        public decimal cantidad_ingresar {get; set; }
        public System.DateTime fecha_caducidad { get; set; } = DateTime.Now;
        public string lote_fabricacion { get; set; } = "";
        public int pallets { get; set; } = 1;
        public bool agregar_lote { get; set; } = false;


        public RecepcionTransferenciaDetalleModel()
        {
            cod_prod = "";
            //unid = "";
            cod_estab = "";
            //folioTransferencia = "";
            transaccion = "";
            //backorder = false;
            //afecta_no_disponible = false;
            //cant = 0m;
            //costo_operativo_unitario = 0m;
            //costo_promedio_as = 0m;
            //cod_prv = "";
            //Cantidad_adicional = 0m;
            unidad = "";
            cantidad = 0m;
            precio_lista = 0m;
            tipo_precio_venta = "";
            importe = 0m;
            iva = 0m;
            ieps = 0m;
            costo = 0m;
            peso = 0m;
            volumen = 0m;
            //id_origen = 0;
            abreviatura_unidad = "";
            cantidad_ingresar = 0;
            codigoEscaneado = "";
        }
    }

    public class GuardarRecepcionModel
    {
        public string folio { get; set; }
        public string transaccion { get; set; }
        public string transaccion_origen { get; set; }
        public string cod_estab { get; set; }
        public string cod_estab_alterno { get; set; }
        public string folio_transferencia { get; set; }
        public decimal importeGeneral { get; set; }
        public decimal ivaGeneral { get; set; }
        public decimal iepsGeneral { get; set; }
        public decimal costoGeneral { get; set; }
        public decimal unidadesGeneral { get; set; }
        public decimal piezasGeneral { get; set; }
        public decimal pesoGeneral { get; set; }
        public decimal volumenGeneral { get; set; }
        public string usuario { get; set; }
        public string razon_aod_inventario { get; set; }
        public string folio_referencia { get; set; }
        public short orden_embarque { get; set; }
        public bool recoge_mercancia { get; set; }
        public string notasGeneral { get; set; }
        public string clave_afectacion_inventario { get; set; }
        public string Operacion { get; set; }
            public string cod_cte { get; set; }
        public List<GuardarRecepcionDetalleModel> listaDetalle { get; set; }

        public GuardarRecepcionModel()
        {
            folio = "";
            transaccion = "";
            transaccion_origen = "";
            cod_estab = "";
            cod_estab_alterno = "";
            folio_transferencia = "";
            importeGeneral = 0m;
            ivaGeneral = 0m;
            iepsGeneral = 0m;
            costoGeneral = 0m;
            unidadesGeneral = 0m;
            piezasGeneral = 0m;
            pesoGeneral = 0m;
            volumenGeneral = 0m;
            usuario = "";
            razon_aod_inventario = "";
            folio_referencia = "";
            orden_embarque = 0;
            recoge_mercancia = false;
            notasGeneral = "";
            clave_afectacion_inventario = "";
            Operacion = "";
            cod_cte = "";
            listaDetalle = new List<GuardarRecepcionDetalleModel>();
        }
    }

    public class GuardarRecepcionDetalleModel
    {
        public string cod_prod { get; set; }
        public string unid { get; set; }
        public string cod_estab { get; set; }
        public string folioTransferencia { get; set; }
        public string transaccion { get; set; }
        public bool backorder { get; set; }
        public bool afecta_no_disponible { get; set; }
        public decimal cant { get; set; }
        public decimal costo_operativo_unitario { get; set; }
        public decimal costo_promedio_as { get; set; }
        public string cod_prv { get; set; }
        public decimal Cantidad_adicional { get; set; }
        public string unidad { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio_lista { get; set; }
        public string tipo_precio_venta { get; set; }
        public decimal importe { get; set; }
        public decimal iva { get; set; }
        public decimal ieps { get; set; }
        public decimal costo { get; set; }
        public decimal peso { get; set; }
        public decimal volumen { get; set; }
        public int id_origen { get; set; }
        public string abreviatura_unidad { get; set; }
        public System.DateTime fecha_caducidad { get; set; } = DateTime.Now;
        public string lote_fabricacion { get; set; } = "";
        public int pallets { get; set; } = 1;
        public bool agregar_lote { get; set; } = false;


        public GuardarRecepcionDetalleModel()
        {
            cod_prod = "";
            unid = "";
            cod_estab = "";
            folioTransferencia = "";
            transaccion = "";
            backorder = false;
            afecta_no_disponible = false;
            cant = 0m;
            costo_operativo_unitario = 0m;
            costo_promedio_as = 0m;
            cod_prv = "";
            Cantidad_adicional = 0m;
            unidad = "";
            cantidad = 0m;
            precio_lista = 0m;
            tipo_precio_venta = "";
            importe = 0m;
            iva = 0m;
            ieps = 0m;
            costo = 0m;
            peso = 0m;
            volumen = 0m;
            id_origen = 0;
            abreviatura_unidad = "";
        }
    }

    public class ProductosSurtidosModel
    {
        public string cod_prod { get; set; } = "";
        public string unidad { get; set; } = "";
        public decimal cantidad { get; set; } = 0;
        public Nullable<bool> encontrado { get; set; } = false;
        public string comentario_venta { get; set; } = "";
    }

    public class TotalesTransferenciaModel
    {
        public decimal cantidadTotal { get; set; }
        public decimal volumenTotal { get; set; }
        public decimal pesoTotal { get; set; }
        public decimal costoTotal { get; set; } 
    }

    public class razones_devoluciones_clientes
    {
        public string razon_devoluciones_clientes { get; set; }
        public string nombre { get; set; }
        public string abreviatura { get; set; }
        public string status { get; set; }
        public string causante_devoluciones_clientes { get; set; }
        public bool afecta_comision_ventas { get; set; }
        public byte cargo { get; set; }
        public string cod_prod_cargo { get; set; }
        public bool exige_notas { get; set; }
        public string razon_credito_clientes { get; set; }
    }

}
