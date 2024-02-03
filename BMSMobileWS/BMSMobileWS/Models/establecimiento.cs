//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMSMobileWS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class establecimiento
    {
        public string cod_estab { get; set; }
        public string tipo_establecimiento { get; set; }
        public string nombre { get; set; }
        public string calle { get; set; }
        public string num_interior { get; set; }
        public string num_exterior { get; set; }
        public string colonia { get; set; }
        public string cod_postal { get; set; }
        public string pobmunedo { get; set; }
        public string telefono1 { get; set; }
        public string telefono2 { get; set; }
        public string rfc { get; set; }
        public int area_terreno { get; set; }
        public int area_uso { get; set; }
        public int cajones_estacionamiento { get; set; }
        public bool centro_comercial { get; set; }
        public bool aire_acondicionado { get; set; }
        public byte numero_cajas { get; set; }
        public string tipo_arrendamiento { get; set; }
        public string frecuencia_pago { get; set; }
        public decimal cantidad_pago { get; set; }
        public Nullable<System.DateTime> fecha_ultimo_pago { get; set; }
        public string lista_precios { get; set; }
        public string tipo_pallet { get; set; }
        public decimal tolerancia_bascula { get; set; }
        public string pais { get; set; }
        public string estado { get; set; }
        public string municipio { get; set; }
        public string poblacion { get; set; }
        public string region { get; set; }
        public string zona { get; set; }
        public string sector { get; set; }
        public bool sucursal { get; set; }
        public bool exige_vendedor { get; set; }
        public decimal maximo_descuento { get; set; }
        public int minimo { get; set; }
        public string tipo_minimo { get; set; }
        public byte dias_para_cargar { get; set; }
        public System.DateTime fecha_ultimo_corte { get; set; }
        public short orden_reparto { get; set; }
        public string ruta_reparto { get; set; }
        public short coordenada_x { get; set; }
        public short coordenada_y { get; set; }
        public string serie { get; set; }
        public string posicion_serie { get; set; }
        public string clave_iva { get; set; }
        public bool back_order { get; set; }
        public string metodo_pvs { get; set; }
        public string aplica_condiciones_venta { get; set; }
        public bool aplica_condiciones_financieras { get; set; }
        public bool aplica_grupo_facturacion { get; set; }
        public bool permite_descvta { get; set; }
        public string cliente_venta_publico { get; set; }
        public decimal margen_minimo { get; set; }
        public bool exige_existencia_total { get; set; }
        public string flexibilidad_precio_cajas { get; set; }
        public bool msc_libre { get; set; }
        public byte plazo_expiracion { get; set; }
        public bool aplica_cv_felab { get; set; }
        public string autorizacion_pedido { get; set; }
        public bool BMS { get; set; }
        public System.DateTime fecha_inicio_BMS { get; set; }
        public string proveedor_gastos_embarque { get; set; }
        public decimal costo_financiero { get; set; }
        public decimal costo_mensual_almacenaje { get; set; }
        public bool fotografia_productos_pos { get; set; }
        public bool incluir_negados_salidas { get; set; }
        public bool surtido_express { get; set; }
        public string modo_online_pos { get; set; }
        public string status { get; set; }
        public int id_afectacion_inventario_tickets { get; set; }
        public bool afectacion_automatica_inventario_tickets { get; set; }
        public bool mostrar_credito_disponible { get; set; }
        public string modo_operacion { get; set; }
        public System.DateTime fecha_envio_informacion { get; set; }
        public System.DateTime fecha_recepcion_informacion { get; set; }
        public bool notas_tickets { get; set; }
        public bool pagos_multiples_clientes { get; set; }
        public string leyenda_ticket { get; set; }
        public bool comentario_ventas { get; set; }
        public byte variacion_costo_promedio { get; set; }
        public string razon_social { get; set; }
        public string tipo_precio_pos { get; set; }
        public string afecta_inventario_pos { get; set; }
        public string aplica_condiciones_compra { get; set; }
        public bool verifica_margen_pos { get; set; }
        public bool ticket_ordenado { get; set; }
        public bool salidas_globales { get; set; }
        public string mapa { get; set; }
        public bool minimo_multiplo_pos { get; set; }
        public string lista_precio_publico_sugerido { get; set; }
        public string inventario_precios_busqueda_pos { get; set; }
        public bool turnos_ventas_mostrador { get; set; }
        public string transaccion_clasificador_producto_localizaciones { get; set; }
        public string manejo_ubicacion_sugerida { get; set; }
        public string manejo_recibo_valores { get; set; }
        public short orden { get; set; }
        public decimal volumen { get; set; }
        public int capacidad_pallets { get; set; }
        public bool comentario_ventas_renglon_aparte { get; set; }
        public string valor_transferencias { get; set; }
        public bool afecta_nd_ordenes_carga { get; set; }
        public bool anticipo_cliente_solo_apartados { get; set; }
        public bool pagare_embarques { get; set; }
        public string cfd_llave_privada { get; set; }
        public string cfd_certificado_digital { get; set; }
        public string cfd_archivos_xml { get; set; }
        public string cfd_contraseña_llave_privada { get; set; }
        public string codigo_gln { get; set; }
        public string ruta_lector_huella { get; set; }
        public string grupo_establecimiento { get; set; }
        public string abreviatura { get; set; }
        public string regimen_fiscal { get; set; }
        public string usuario_ultima_modificacion { get; set; }
        public System.DateTime ultima_modificacion { get; set; }
        public string ruta_xml_proveedores { get; set; }
        public string curp { get; set; }
        public string ruta_pdf_proveedores { get; set; }
        public Nullable<short> horas_diferencia_servidor { get; set; }
        public string geolocalizacion { get; set; }
        public string Colonia_codigo { get; set; }
        public bool Surtido_Picking { get; set; }
    }
}
