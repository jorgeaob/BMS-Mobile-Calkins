using BMSMobile.Models;
using BMSMobile.Utilities;
using BMSMobile.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using static Android.Icu.Text.IDNA;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class EntradasSalidasLocVM
    {
        public INavigation navigaton { get; set; }
        public EntradasSalidasLoc view { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command ActualizarCommand { get; set; }
        public Command TxtLocChanged { get; set; }
        public Command CompletedLocCommand { get; set; }
        private string localizacion { get; set; }
        public string Localizacion { get => localizacion; set { localizacion = value; } }
        private ObservableCollection<EntradasSalidasLocModel> lista { get; set; }
        public ObservableCollection<EntradasSalidasLocModel> Lista { get { return lista; } set { lista = value; } }
        public EntradasSalidasLocVM(INavigation _navigation, EntradasSalidasLoc _view)
        {
            view = _view;
            navigaton = _navigation;
            MostrarMsg = new Mensajes();
            aIModel = new AIModel();
            aIModel.IsBusy = false;
            localizacion = "";
            lista = new ObservableCollection<EntradasSalidasLocModel>();
            ActualizarCommand = new Command(Actualizar);
            TxtLocChanged = new Command(Limpiar);
            CompletedLocCommand = new Command(Actualizar);
            LimpiarCommand = new Command(Limpiar);
        }

        private async void Limpiar()
        {
            if (lista.Count > 0)
            {
                lista = new ObservableCollection<EntradasSalidasLocModel>();
                localizacion = "";
            }
        }
            private async void Actualizar()
        {
            lista = new ObservableCollection<EntradasSalidasLocModel>();       
                try
                {
                    if (string.IsNullOrEmpty(localizacion)) { return; }
                    localizacion = ValidaDigitoVerificador(localizacion, false);
                    if (localizacion == "-1")
                    {
                        await MostrarMsg.ShowMessage(string.Format("Error {0}", "Folio de pallet no válido"));
                    }
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Localizacion", localizacion);  
                    var url = "http://" + General.urlWS + "/api/EntradasSalidasLoc/TraerPallet";
                    var resp = await client.Get<ObservableCollection<EntradasSalidasLocModel>>(url, parametros);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage("No se pudo encontrar información con la localización ingresada.");
                        return;
                    }
                    lista = resp.Result;
                }
                catch (Exception ex) { await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message)); }        
        }

        public string ValidaDigitoVerificador(string numero, bool pallet)
        {
            string DV1;
            int PosDV = numero.IndexOf("-");

            if (PosDV < 0)
            {
                if (pallet)
                    return numero.Trim();
                return "-1";
            }
            else
            {
                DV1 = AsignaDigitoVerificador(numero.Substring(0, PosDV));
                if (DV1.Trim().ToLower() == numero.Trim().ToLower())
                    return numero.Substring(0, PosDV);
                return "-1";
            }
        }

        public string AsignaDigitoVerificador(string Numero)
        {
            string DVNum = Numero;
            for (int i = Numero.Length - 1; i >= 0; i--)
            {
                if (!(Numero[i] >= '0' && Numero[i] <= '9'))
                {
                    Numero = Numero.Replace(Numero[i].ToString(), "");
                }
            }

            if (Numero.Trim() == "")
                return "ERROR1";

            Numero = Convert.ToDecimal(Numero).ToString();
            string DV = "";
            decimal Total = 0;
            int[,] DigitoV = new int[3, Numero.Length];

            for (int I = 0; I < Numero.Length; I++)
            {
                DigitoV[0, I] = Convert.ToInt32(Numero.Substring(I, 1));
                DigitoV[1, I] = Numero.Length - I + 1;
                DigitoV[2, I] = DigitoV[0, I] * DigitoV[1, I];
                Total += DigitoV[2, I];
            }

            Total = Math.Abs((Total * 10) / 11);
            DV = ((int)Total).ToString();

            int Aux = 0;
            do
            {
                Aux = 0;
                for (int I = 0; I < DV.Length; I++)
                {
                    Aux += Convert.ToInt32(DV.Substring(I, 1));
                }
                DV = Aux.ToString();
            } while (Aux > 11);

            if (DV == "10")
                DV = "S";
            if (DV == "11")
                DV = "0";

            if (DV.Length > 1)
            {
                DVNum = "ERROR2";
            }
            else
            {
                DVNum = DVNum.Trim() + "-" + DV;
            }

            return DVNum;
        }
    }
}
