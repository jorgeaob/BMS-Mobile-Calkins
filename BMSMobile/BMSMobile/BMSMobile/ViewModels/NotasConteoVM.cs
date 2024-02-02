using BMSMobile.Models;
using BMSMobile.Utilities;
using PropertyChanged;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class NotasConteoVM
    {
        INavigation Navigation { get; set; }
        
        public Mensajes MostrarMsg { get; set; }
        private DetalleConteo _productoNota { get; set; }
        public DetalleConteo ProductoNota
        {
            get { return _productoNota; }
            set
            {
                _productoNota = value;
            }
        }

        public Command GuardarNotasCommand { get; set; }

        public NotasConteoVM(INavigation navigation)
        {
            Navigation = navigation;
            MostrarMsg = new Mensajes();
            _productoNota = new DetalleConteo();

            GuardarNotasCommand = new Command(GuardarNotas);

            MessagingCenter.Subscribe<DetalleConteo>(this, "DifProdSelected", (item) =>
            {
                _productoNota = new DetalleConteo
                {
                    folio = item.folio.Trim(),
                    cod_prod = item.cod_prod.Trim(),
                    fecha = item.fecha,
                    usuario = General.userCode,
                    unidades_compra = item.unidades_compra,
                    unidades_alternativas = item.unidades_alternativas,
                    exist_unidades_compra = item.exist_unidades_compra,
                    exist_unidades_alternativas = item.exist_unidades_alternativas,
                    programacion = "",
                    notas = item.notas,
                    descripcion_completa = item.descripcion_completa.Trim(),
                    NombreUC = item.NombreUC.Trim(),
                    NombreUA = item.NombreUA.Trim(),
                    forma_expresar_inventario = item.forma_expresar_inventario.Trim(),
                    contenido = item.contenido,
                    AbrevUC = item.AbrevUC.Trim(),
                    AbrevUA = item.AbrevUA.Trim()
                };

                MessagingCenter.Unsubscribe<DetalleConteo>(this, "DifProdSelected");
            });

        }

        private async void GuardarNotas()
        {
            try
            {
                
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>
                {
                    { "Folio", _productoNota.folio.Trim() },
                    { "CodProd", _productoNota.cod_prod.Trim() },
                    { "Notas", _productoNota.notas.Trim() }
                };
                var url = "http://" + General.urlWS + "/api/Inventario/GuardarNotasConteo";
                var resp = await client.Post<DetalleConteo>(url, parametros, null);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " No se pudo guardar las cantidades.");
                    return;
                }
                else
                {
                    await MostrarMsg.ShowMessage("Se guardaron las notas.");
                    MessagingCenter.Send<string>("Ok", "NotasGuardadas");
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }
    }
}
