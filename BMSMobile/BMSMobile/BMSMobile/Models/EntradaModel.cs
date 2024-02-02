using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;

namespace BMSMobile.Models
{
    [AddINotifyPropertyChangedInterface]
    public class EntradaModel
    {
        public OrdenCompraModel cabecero { get; set; }
        public ObservableCollection<OrdenCompraDetalleModel> listaDetalle { get; set; }
        //public ObservableCollection<ProductosAgregados> listaProductosAgregados { get; set; }
        //public Producto Producto { get; set; }
        
        public Entrada Entrada { get; set; }
        
        public EntradaModel() 
        { 
            Entrada = new Entrada();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class OrdenCompraModel
    {
        public string folio { get; set; }
        public string Transaccion { get; set; }
        public System.DateTime fecha { get; set; }
        public decimal descuento_porcentual { get; set; }
        public decimal importe_descuento { get; set; }
        public decimal importe { get; set; }
        public decimal iva { get; set; }
        public decimal ieps { get; set; }
        public decimal costo { get; set; }
        public decimal unidades { get; set; }
        public decimal piezas { get; set; }
        public decimal peso { get; set; }
        public decimal volumen { get; set; }
        public System.DateTime fecha_expiracion { get; set; }
        public bool back_order { get; set; }
        public bool recoge_mercancia { get; set; }
        public string cond_pago { get; set; }
        public string cod_estab { get; set; }
        public string numdpc { get; set; }
        public string pedido_proveedor { get; set; }
        public short plazo { get; set; }
        public string status_surtido { get; set; }
        public string notas { get; set; }
        public string status { get; set; }
        public string razon_social { get; set; }
        public string cod_prv { get; set; }
        public string comprador { get; set; }
        public string pedimento { get; set; }
        public string razon_compra { get; set; }
        public string nacionalidad { get; set; }
        public string mensaje { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class OrdenCompraDetalleModel
    {
        public string descripcion_completa { get; set; }
        public int id { get; set; }
        public string folio { get; set; }
        public string transaccion { get; set; }
        public System.DateTime fecha { get; set; }
        public string cod_prod { get; set; }
        public string unidad { get; set; }
        public decimal cantidad_pedida { get; set; }
        public decimal cantidad_autorizada { get; set; }
        public decimal cantidad_surtida { get; set; }
        public decimal precio_lista { get; set; }
        public decimal descuento_porcentual { get; set; }
        public decimal importe_descuento { get; set; }
        public decimal importe { get; set; }
        public decimal costo { get; set; }
        public decimal iva { get; set; }
        public decimal ieps { get; set; }
        public decimal peso { get; set; }
        public decimal volumen { get; set; }
        public bool back_order { get; set; }
        public string cod_prv { get; set; }
        public string cod_estab { get; set; }
        public string comprador { get; set; }
        public string status { get; set; }
        public decimal contenido { get; set; }
        public decimal pvs { get; set; }
        public short caducidad_recepcion { get; set; }
        public short caducidad_venta { get; set; }
        public Nullable<decimal> existencia { get; set; }
        public string abreviatura_unidad { get; set; }
        public string cod_prod_proveedor { get; set; }
        public string codigo_barras_unidad { get; set; }
        public string codigo_barras_pieza { get; set; }
        public System.DateTime caducidad_orden_compra { get; set; }
        public string UsaFichaTecProd { get; set; }
        public bool control_etiqueta { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class Producto
    {
        public string cod_prod { get; set; }
        public string descripcion_completa { get; set; }
        public float volumen { get; set; }
        public decimal peso_total { get; set; }
        public short caducidad_fabricacion { get; set; }
        public decimal contenido { get; set; }
        public string cod_estab { get; set; }
        public short caducidad_recepcion { get; set; }
        public short caducidad_venta { get; set; }
        public string presentacion { get; set; }
        public string unidad_compra { get; set; }
        public string StatusProd { get; set; }
        public string StatusProdEstab { get; set; }
        public string StatusProdPrv { get; set; }
        public Nullable<int> pallet_prod { get; set; }
        public short cama { get; set; }
        public short altura { get; set; }
        public string abrevUC { get; set; }
        public string abrevUA { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class UnidadesModel
    {
        public string Abreviatura { get; set; }
        public string Unidad { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class EntradaDetalle
    {
        public string cod_prod { get; set; } = string.Empty;
        public string descripcion { get; set; } = "";
        public int cantidad { get; set; } = 0;
        public object unidad { get; set; } = "U";
        public decimal peso { get; set; } = 0.0m;
        public decimal volumen { get; set; } = 0.0m;
        public string abreviatura_unidad { get; set; } = "";
        public string notas { get; set; } = "";
        public DateTime fecha_caducidad { get; set; } = DateTime.MinValue;
        public bool pallet { get; set; } = false;
        public string lote_fab { get; set; } = "";
        public bool guardado { get; set; } = false;
        public bool guardando { get; set; } = false;
        public int id_meos { get; set; } = 0;
        public int id_mped { get; set; } = 0;
        public string _fecha_caducidad { get; set;} ="";


        public EntradaDetalle() { 
        _fecha_caducidad = fecha_caducidad.ToShortDateString();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class Entrada
    {
        public ObservableCollection<EntradaDetalle> entradaDetalles { get; set; }

        // Propiedades calculadas
        public decimal UC
        {
            get { return entradaDetalles.Sum(d => d.unidad.ToString().Trim() == "U" ? d.cantidad : 0); }
            set { }
        }
        public decimal UA
        {
            get { return entradaDetalles.Sum(d => d.unidad.ToString().Trim() != "U" ? d.cantidad : 0); }
            set { }
        }
        public int PL
        {
            get { return entradaDetalles.Count(d => d.pallet); }
            set { }
        }
        public decimal PE
        {
            get { return entradaDetalles.Sum(d => d.peso); }
            set { }
        }
        public decimal VO
        {
            get { return entradaDetalles.Sum(d => d.volumen); }
            set { }
        }

        public string OC { get; set; } = "";
        public string Proveedor { get; set; } = "";
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

    public partial class proveedore
    {
        public string cod_prv { get; set; }
        public string tipo_cuenta_pagar { get; set; }
        public string tipo_proveedor { get; set; }
        public string razon_social { get; set; }
        public string clasificacion_fiscal { get; set; }
        public string nombre { get; set; }
        public string ap_paterno { get; set; }
        public string ap_materno { get; set; }
        public string nom_comercial { get; set; }
        public string calle { get; set; }
        public string num_exterior { get; set; }
        public string num_interior { get; set; }
        public string colonia { get; set; }
        public string cod_postal { get; set; }
        public string delegacion { get; set; }
        public string pobmunedo { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string fax { get; set; }
        public string rfc { get; set; }
        public string clave_iva { get; set; }
        public string flexibilidad_precio { get; set; }
        public bool productos_catalogados { get; set; }
        public string pago_flete { get; set; }
        public decimal reembolso_flete { get; set; }
        public decimal pedido_minimo { get; set; }
        public string tipo_minimo { get; set; }
        public string codigo_nuestro_proveedor { get; set; }
        public string manejo_devoluciones { get; set; }
        public short plazo { get; set; }
        public byte tolerancia_pago { get; set; }
        public bool abono_en_cuenta { get; set; }
        public bool retencion_pago { get; set; }
        public bool back_order { get; set; }
        public string autorizacion_ordenes_compra { get; set; }
        public string pagina_web { get; set; }
        public string moneda { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public string pais { get; set; }
        public string estado { get; set; }
        public string municipio { get; set; }
        public string poblacion { get; set; }
        public string cod_estab { get; set; }
        public bool linea_fletera { get; set; }
        public string formato_factura { get; set; }
        public bool pago_estricto { get; set; }
        public string status_proveedor { get; set; }
        public string e_mail { get; set; }
        public System.DateTime ultima_modificacion { get; set; }
        public string proveedor_linea_fletera { get; set; }
        public string condicion_plazo { get; set; }
        public string giro_proveedor { get; set; }
        public string curp { get; set; }
        public string entre_calles { get; set; }
        public string orientacion { get; set; }
        public string mapa { get; set; }
        public short coordenada_x { get; set; }
        public short coordenada_y { get; set; }
        public string clave_valor_pagar { get; set; }
        public byte evaluacion_proveedor { get; set; }
        public string notas { get; set; }
        public bool iva_incluido_precio { get; set; }
        public string nacionalidad { get; set; }
        public bool comitente { get; set; }
        public string condicion_pago { get; set; }
        public string cod_cte { get; set; }
        public bool envio_directo_cliente { get; set; }
        public short te { get; set; }
        public string comprador { get; set; }
        public decimal limite_credito { get; set; }
        public byte tolerancia_limite_credito { get; set; }
        public bool paqueteria { get; set; }
        public bool afianzadora { get; set; }
        public bool aseguradora { get; set; }
        public string tipo_documento_venta { get; set; }
        public string usuario_ultima_modificacion { get; set; }
        public short dias_atencion_oc { get; set; }
        public bool exige_orden_compra_anticipos { get; set; }
    }

}
