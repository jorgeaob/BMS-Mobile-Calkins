using Android.Views;
using BMSMobile.Models;
using BMSMobile.Utilities;
using BMSMobile.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    
    public class InicioVM
    {          
        public Mensajes MostrarMsg { get; set; }
        public ColorsModel colorModel { get; set; }

        public string userSession { get; set; }
        public string UserSession
        {
            get { return userSession; }
            set { userSession = value; }            
        }

        private ObservableCollection<EstablecimientosModel> _listaEstabs { get; set; }
        public ObservableCollection<EstablecimientosModel> ListaEstabs
        {
            get { return _listaEstabs; }
            set { _listaEstabs = value; }
        }

        public EstablecimientosModel _estabSelect { get; set; }
        public EstablecimientosModel EstabSelect
        {
            get { return _estabSelect; }
            set { _estabSelect = value; }
        }
        public InicioView view { get; set; }
        public InicioVM(InicioView _view)
        {
            view = _view;
            MostrarMsg = new Mensajes();
            colorModel = new ColorsModel();
            userSession = "";
            _listaEstabs = new ObservableCollection<EstablecimientosModel>();
            _estabSelect = new EstablecimientosModel();

            userSession = General.userCode.Trim() + " " + General.userName.Trim();
        }

        public ICommand SelectedIndexChangedCommand => new Command(async() =>
        { 
            try
            {
                General.EstabSession = _estabSelect.cod_estab.Trim();
                General.EstabName = _estabSelect.Nombre.Trim();
                General.cliente_venta_publico = _estabSelect.cliente_venta_publico.Trim();
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        });


        private async void Establecimientos()
        {
            try
            {               
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Usuario", General.userCode);
                var url = "http://" + General.urlWS + "/api/Login/Establecimientos";
                var resp = await client.Get<ObservableCollection<EstablecimientosModel>>(url, parametros);
                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " No se encontraron establecimientos.");
                    return;
                }
                if(resp.Result.Count <= 0)
                {
                    await MostrarMsg.ShowMessage("No tiene acceso a ningun establecimiento.");
                    return;
                }
                else
                {
                   // EstabSelect = new EstablecimientosModel { Nombre = General.EstabName, cod_estab = General.EstabSession, cliente_venta_publico = General.cliente_venta_publico };
                    _listaEstabs = resp.Result;
                    var estab = _listaEstabs.FirstOrDefault(e => e.cod_estab == General.EstabSession);
                    EstabSelect = estab;
                }
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        public async Task MostrarListaEstab()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Establecimientos();
                });
            });
        }
    }
}
