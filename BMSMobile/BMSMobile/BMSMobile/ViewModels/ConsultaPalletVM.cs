using Android.Content.Res;
using Android.Views.Accessibility;
using BMSMobile.Models;
using BMSMobile.Utilities;
using BMSMobile.Views;
using Java.Util;
using Javax.Security.Auth;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ConsultaPalletVM
    {
        public INavigation navigaton { get; set; }
        public ConsultaPalletView view { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command TxtLocChanged { get; set; }
        public Command GuardarCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command ActualizarCommand { get; set; }
        public Command CompletedLocCommand { get; set; }
        public Command CompletedPalletCommand { get; set; }
        public ConsultaPalletModel Info { get; set; }
        private string pallet { get; set; }
        public string Pallet { get => pallet; set { pallet = value; } }
        private string localizacion { get; set; }
        public string Localizacion { get => localizacion; set { localizacion = value; } }
        private string codprod { get; set; }
        public string CodProd { get => codprod; set { codprod = value; } }
        private string descripcion { get; set; }
        public string Descripcion { get => descripcion; set { descripcion = value; } }
        private decimal cantidad { get; set; }
        public decimal Cantidad { get => cantidad; set { cantidad = value; } }
        private DateTime fecha { get; set; }
        public DateTime Fecha { get => fecha; set { fecha = value; } }
        private DateTime fecha_cad { get; set; }
        public DateTime FechaCad { get => fecha_cad; set { fecha_cad = value; } }

        public DateTime MiniumDate = DateTime.MinValue;
        private bool modificando { get; set; }
        public bool Modificando { get => modificando; set { modificando = value; } }
        public bool Actualizando { get; set; }
        private string lotefab { get; set; }
        public string LoteFab { get => lotefab; set { lotefab = value; } }
        private string loterec { get; set; }
        public string LoteRec { get => loterec; set { loterec = value; } }

        public ConsultaPalletVM(INavigation _navigation, ConsultaPalletView _view)
        {
            view = _view;
            navigaton = _navigation;
            MostrarMsg = new Mensajes();
            aIModel = new AIModel();
            aIModel.IsBusy = false;
            localizacion = "";
            pallet = "";
            codprod = "";
            cantidad = 1;
            descripcion = "";
            fecha = DateTime.Today;
            fecha_cad = DateTime.Today;
            lotefab = "";
            loterec = "";
            modificando = false;
            Actualizando = false;
            Info = new ConsultaPalletModel();
            LimpiarCommand = new Command(Limpiar);
            GuardarCommand = new Command(Guardar);
            EditarCommand = new Command(Editar);
            ActualizarCommand = new Command(Actualizar);
            CompletedPalletCommand = new Command(RefrescarPallet);
            CompletedLocCommand = new Command(RefrescarLoc);
            TxtLocChanged = new Command(Limpiar);

        }
        private async void RefrescarPallet()
        {
            try
            {
                Actualizando = true;
                Info = new ConsultaPalletModel();
                if (string.IsNullOrEmpty(pallet)) { return; }
                pallet = ValidaDigitoVerificador(pallet, true);
                if ( pallet == "-1")
                {
                    await MostrarMsg.ShowMessage(string.Format("Error {0}", "Folio de pallet no válido"));
                    return;
                }
             
                    //string Pallet,string Localizacion, bool EsPallet, string CodEstab, bool TraerPallet
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Pallet", pallet);
                    parametros.Add("Localizacion", localizacion);
                    parametros.Add("EsPallet", "True"); ;
                    parametros.Add("CodEstab", General.EstabSession);
                    parametros.Add("TraerPallet", modificando ? "False" : "True");
                    var url = "http://" + General.urlWS + "/api/ConsultaPallet/TraerPallet";
                    var resp = await client.Get<ConsultaPalletModel>(url, parametros);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage("No se pudo encontrar información con el folio ingresado.");
                        return;
                    }
                Info = resp.Result;
                AsignarValores();

            }
            catch (Exception ex) { await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message)); }
            finally { Actualizando = false; }
        }
        private async void RefrescarLoc()
        {
            try 
            {
                Actualizando = true;
            Info = new ConsultaPalletModel();
            if (string.IsNullOrEmpty(localizacion)) { return; }
            localizacion = ValidaDigitoVerificador(localizacion, false);
            if (localizacion == "-1")
            {
                await MostrarMsg.ShowMessage(string.Format("Error {0}", "Folio de localización no válido"));
                    return; 
            }

            //string Pallet,string Localizacion, bool EsPallet, string CodEstab, bool TraerPallet
            RestClient client = new RestClient(null);
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Pallet", pallet);
            parametros.Add("Localizacion", localizacion);
            parametros.Add("EsPallet", "False"); ;
            parametros.Add("CodEstab", General.EstabSession);
            parametros.Add("TraerPallet", "False");
            var url = "http://" + General.urlWS + "/api/ConsultaPallet/TraerPallet";
            var resp = await client.Get<ConsultaPalletModel>(url, parametros);

            if (!resp.Ok)
            {
                await MostrarMsg.ShowMessage("No se pudo encontrar información con el folio ingresado.");
                return;
            }
                Info = resp.Result;
                AsignarValores();
            }
            catch (Exception ex) { await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message)); }
            finally { Actualizando = false; }
        }
        //private async void TraerPallet()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message));
        //    }
        //}
        private void Editar()
        {
            modificando = true;
        }
        private async void Actualizar()
        {
            if (modificando) { return; }
            Actualizando = true;
            Info = new ConsultaPalletModel();
            modificando = false;
            if (string.IsNullOrEmpty(pallet))
            {
                RefrescarPallet();
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(pallet)) { return; }
                    pallet = ValidaDigitoVerificador(pallet, true);
                    if (pallet == "-1")
                    {
                        await MostrarMsg.ShowMessage(string.Format("Error {0}", "Folio de pallet no válido"));
                    }
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Pallet", pallet);
                    parametros.Add("Localizacion", localizacion);
                    parametros.Add("EsPallet", "True"); ;
                    parametros.Add("CodEstab", General.EstabSession);
                    parametros.Add("TraerPallet", modificando ? "False" : "True");
                    var url = "http://" + General.urlWS + "/api/ConsultaPallet/TraerPallet";
                    var resp = await client.Get<ConsultaPalletModel>(url, parametros);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage("No se pudo encontrar información con el folio ingresado.");
                        return;
                    }
                    Info = resp.Result;
                    AsignarValores();
                }
                catch (Exception ex){ await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message));}
                finally { Actualizando = false; }

            }
        }
        private async void Guardar()
        {
            try
            {
                if(!modificando || string.IsNullOrEmpty(pallet)) { return; }

                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Pallet", pallet);
                parametros.Add("Localizacion", localizacion);
                parametros.Add("FechaCad", fecha_cad.ToString()); ;
                parametros.Add("LoteFab", lotefab);
                var url = "http://" + General.urlWS + "/api/ConsultaPallet/Guardar";
                var resp = await client.Post<string>(url, parametros,null);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo guardar la información.");
                    return;
                }
                await MostrarMsg.ShowMessage("Información guardada correctamente.");
                modificando = false;
                Limpiar();

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message));
            }
        }
        private void Limpiar()
        {
            if (string.IsNullOrEmpty(Info.cod_prod)) { return;}
            if(Actualizando) { return; }
            modificando = false;
            Info = new ConsultaPalletModel();
            AsignarValores();
            pallet = "";
        }

        private void AsignarValores()
        {
            codprod = Info.cod_prod;
            descripcion = Info.descripcion;
            cantidad = Info.cantidad;
            localizacion = Info.localizacion;
            lotefab = Info.lote_fabricacion;
            loterec = Info.folio_referencia;
            fecha = Info.fecha;
            fecha_cad = Info.fecha_caducidad;
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
