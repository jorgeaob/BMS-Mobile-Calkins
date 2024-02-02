using BMSMobile.Models;
using BMSMobile.Utilities;
using PropertyChanged;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BuscadorProductosVM
    {
        public INavigation Navigation { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }

        private ObservableCollection<Productos> _listProds { get; set; }
        public ObservableCollection<Productos> ListProds
        {
            get { return _listProds; }
            set
            {
                _listProds = value;
            }
        }

        private string _filtro { get; set; }
        public string Filtro
        {
            get { return _filtro; }
            set
            {
                _filtro = value;
            }
        }

        public Command TextChangedCommand { get; set; }

        public BuscadorProductosVM(INavigation navigation)
        {
            Navigation = navigation;
            MostrarMsg = new Mensajes();
            _listProds = new ObservableCollection<Productos>();
            aIModel = new AIModel();
            aIModel.IsBusy = false;
            _filtro = "";

            TextChangedCommand = new Command(BuscarProd);
        }

        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (Productos)item;
            MessagingCenter.Send<Productos>(i, "Producto_Selected");
            await PopupNavigation.Instance.PopAsync();
        });

        public async void BuscarProd()
        {
            try
            {
                if (string.IsNullOrEmpty(_filtro))
                    return;
                if(_filtro.Trim().Length < 3)
                {
                    _listProds.Clear();
                    return;
                }
                else
                {
                    aIModel.IsBusy = true;
                    _listProds.Clear();

                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Codigo", "");
                    parametros.Add("Filtro", _filtro.Trim());
                    parametros.Add("Estab", General.userCodEstab);
                    var url = "http://" + General.urlWS + "/api/Inventario/BuscarProductos";
                    var resp = await client.Get<ObservableCollection<Productos>>(url, parametros);

                    if (!resp.Ok)
                    {                        
                        return;
                    }
                    else
                    {
                        _listProds = resp.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
            finally
            {
                aIModel.IsBusy = false;
            }
        }
    }
}
