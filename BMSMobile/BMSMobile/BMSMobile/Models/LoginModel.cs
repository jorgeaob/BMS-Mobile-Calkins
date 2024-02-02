using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    public class LoginModel
    {
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string status { get; set; }
        public string cod_estab { get; set; }
        public string vendedor_default_ventas { get; set; }
       
        public LoginModel()
        {
            usuario = "";
            nombre = "";
            status = "";
            cod_estab = "";
            vendedor_default_ventas = "";           
        }
    }

    public class EstablecimientosModel
    {
        public string cod_estab { get; set; }
        public string Nombre { get; set; }
        public string cliente_venta_publico { get; set; }

        public EstablecimientosModel()
        {
            cod_estab = "";
            Nombre = "";
            cliente_venta_publico = "";
        }
    }
}
