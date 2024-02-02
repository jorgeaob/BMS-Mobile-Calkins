using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BMSMobile.Models
{
    public class InventarioModel
    {
        public string folio { get; set; }
        public System.DateTime fecha { get; set; }
        public string cod_estab { get; set; }
        public string usuario { get; set; }
        public bool registrado { get; set; }
        public ObservableCollection<DetalleConteo> ProductosConteo { get; set; }

        public InventarioModel()
        {
            folio = "";
            fecha = DateTime.Now;
            cod_estab = "";
            usuario = "";
            registrado = false;
            ProductosConteo = new ObservableCollection<DetalleConteo>();
        }
    }

    public class DetalleConteo
    {
        public string folio { get; set; }
        public string cod_prod { get; set; }
        public System.DateTime fecha { get; set; }
        public string usuario { get; set; }
        public decimal unidades_compra { get; set; }
        public decimal unidades_alternativas { get; set; }
        public decimal exist_unidades_compra { get; set; }
        public decimal exist_unidades_alternativas { get; set; }
        public string programacion { get; set; }        
        public string descripcion_completa { get; set; }
        public string NombreUC { get; set; }
        public string NombreUA { get; set; }
        public string forma_expresar_inventario { get; set; }
        public decimal difUC { get; set; }
        public decimal difUA { get; set; }
        public decimal contenido { get; set; }
        public string AbrevUC { get; set; }
        public string AbrevUA { get; set; }
        public string codigo_barras_unidad { get; set; }
        public string codigo_barras_pieza { get; set; }
        public string notas { get; set; }
        public bool contado { get; set; }


        public DetalleConteo()
        {
            folio = "";
            cod_prod = "";
            fecha = DateTime.Now;
            usuario = "";
            unidades_compra = 0m;
            unidades_alternativas = 0m;
            exist_unidades_compra = 0m;
            exist_unidades_alternativas = 0m;
            programacion = "";            
            descripcion_completa = "";
            NombreUC = "Unidades";
            NombreUA = "Piezas";
            forma_expresar_inventario = "";
            difUC = 0m;
            difUA = 0m;
            contenido = 0m;
            AbrevUC = "";
            AbrevUA = "";
            codigo_barras_unidad = "";
            codigo_barras_pieza = "";
            notas = "";
            contado = false;
        }
    }

    public class Productos
    {
        public string cod_prod { get; set; }
        public string descripcion_completa { get; set; }
        public decimal contenido { get; set; }
        public decimal costo_promedio { get; set; }
        public string presentacion { get; set; }
        public string forma_expresar_inventario { get; set; }
        public decimal exist_unidades { get; set; }
        public decimal exist_piezas { get; set; }
        public string NombreUC { get; set; }
        public string NombreUA { get; set; }
        public string AbrevUC { get; set; }
        public string AbrevUA { get; set; }
        public string codigo_barras_unidad { get; set; }
        public string codigo_barras_pieza { get; set; }
        
        public Productos()
        {
            cod_prod = "";
            descripcion_completa = "";
            contenido = 0m;
            costo_promedio = 0m;
            presentacion = "";
            forma_expresar_inventario = "";
            exist_unidades = 0m;
            exist_piezas = 0m;
            NombreUC = "Unidades";
            NombreUA = "Piezas";
            AbrevUC = "";
            AbrevUA = "";
            codigo_barras_unidad = "";
            codigo_barras_pieza = "";           
        }
    }

    public class InventarioProductoFechaModel
    {
        public decimal uc { get; set; }
        public decimal ua { get; set; }
        public string codprod { get; set; }               

        public InventarioProductoFechaModel()
        {
            uc = 0m;
            ua = 0m;
            codprod = "";           
        }
    }
}
