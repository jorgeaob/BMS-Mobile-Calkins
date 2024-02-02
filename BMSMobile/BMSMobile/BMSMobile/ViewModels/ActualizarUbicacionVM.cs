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
    public class ActualizarUbicacionVM
    {
        public INavigation navigaton { get; set; }
        public ActualizarUbicacionView view { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command TxtLocChanged { get; set; }
        public Command GuardarCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command ActualizarCommand { get; set; }
        public Command CompletedLocCommand { get; set; }
        public Command CompletedPalletCommand { get; set; }
        public CalkinsWS_ActualizarUbicacionLoc_Result Loc { get; set; }
        public CalkinsWS_ActualizarUbicacionLote_Result Lote { get; set; }
        private GuardarActualizarUbicacion _guardar { get; set; }
        private string pallet { get; set; }
        public string Pallet { get => pallet; set { pallet = value; } }
        private string localizacion { get; set; }
        public string Localizacion { get => localizacion; set { localizacion = value; } }
        private string codprod { get; set; }
        private string CodProdT { get; set; }
        private string LoteT { get; set; }
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
        private bool chkUbicar { get; set; }
        public bool ChkUbicar { get => chkUbicar; set { chkUbicar = value; } }
        public bool Actualizando { get; set; }
        private string lotefab { get; set; }
        public string LoteFab { get => lotefab; set { lotefab = value; } }
        private string loterec { get; set; }
        public string LoteRec { get => loterec; set { loterec = value; } }

        public ActualizarUbicacionVM(INavigation _navigation, ActualizarUbicacionView _view)
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
            ChkUbicar = false;
            Actualizando = false;
            Loc = new CalkinsWS_ActualizarUbicacionLoc_Result();
            Lote = new CalkinsWS_ActualizarUbicacionLote_Result();
            LimpiarCommand = new Command(Limpiar);
            GuardarCommand = new Command(Guardar);
            //ActualizarCommand = new Command(Actualizar);
            CompletedPalletCommand = new Command(RefrescarPallet);
            CompletedLocCommand = new Command(RefrescarLoc);
            TxtLocChanged = new Command(Limpiar);

        }
        private async void RefrescarPallet()
        {
            try
            {
                if(Loc is null) { return; }

                Lote = new CalkinsWS_ActualizarUbicacionLote_Result();
                if (string.IsNullOrEmpty(pallet)) { return; }
                pallet = ValidaDigitoVerificador(pallet, true);
                if (pallet == "-1")
                {
                    await MostrarMsg.ShowMessage(string.Format("Error {0}", "Folio de pallet no válido"));
                    return;
                }

                //string Pallet,string Localizacion, bool EsPallet, string CodEstab, bool TraerPallet
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Folio", pallet);
                var url = "http://" + General.urlWS + "/api/ActualizarUbicacion/Lote";
                var resp = await client.Get<CalkinsWS_ActualizarUbicacionLote_Result>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo encontrar información con el folio ingresado.");
                    return;
                }
                Lote = resp.Result;

                if(Lote.cod_estab.Trim() != General.EstabSession.Trim())
                {
                    await MostrarMsg.ShowMessage("Este lote no pertenece al establecimiento.");
                    return;
                }

                codprod = Lote.cod_prod.Trim();
                descripcion = Lote.descripcion.Trim();
                cantidad = Lote.cantidad;
                lotefab = Lote.lote_fabricacion.Trim();
                fecha_cad = Lote.fecha_caducidad;

            }
            catch (Exception ex) { await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message)); }
            finally { Actualizando = false; }
        }
        private async void RefrescarLoc()
        {
            try
            {
                Actualizando = true;
                Loc = new CalkinsWS_ActualizarUbicacionLoc_Result();
                if (string.IsNullOrEmpty(localizacion)) { return; }
                //localizacion = ValidaDigitoVerificador(localizacion, false);
                //if (localizacion == "-1")
                //{
                //    await MostrarMsg.ShowMessage(string.Format("Error {0}", "Folio de localización no válido"));
                //    return;
                //}

                //string Pallet,string Localizacion, bool EsPallet, string CodEstab, bool TraerPallet
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
              
                parametros.Add("Loc", localizacion);                
                var url = "http://" + General.urlWS + "/api/ActualizarUbicacion/Localizacion";
                var resp = await client.Get<CalkinsWS_ActualizarUbicacionLoc_Result>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo encontrar información con el folio ingresado.");
                    return;
                }
                Loc = resp.Result;
                if (Loc.cod_estab.Trim() != General.EstabSession.Trim())
                {
                    await MostrarMsg.ShowMessage("Esta localización no pertenece al establecimiento.");
                    return;
                }
                if (Loc.pick_slot == true)
                {
                    await MostrarMsg.ShowMessage("Esta localización es de picking.");
                    return;
                }
                if (Loc.disponible == false)
                {
                    await MostrarMsg.ShowMessage("Esta localización no está disponible.");
                    return;
                }

                CodProdT = Loc.cod_prod;
                LoteT = Loc.lote;               

            }
            catch (Exception ex) { await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message)); }
            finally { Actualizando = false; }
        }
  
        private async void Guardar()
        {
            try
            {
                if(Lote is null)
                {
                    await MostrarMsg.ShowMessage("Debe proprocionar pallet.");
                    return;
                }
                if (string.IsNullOrEmpty(lotefab))
                {
                    await MostrarMsg.ShowMessage("Debe proprocionar lote de fabricación.");
                    return;
                }

                _guardar = new GuardarActualizarUbicacion();
                _guardar.usuario = General.userCode.Trim();
                _guardar.cod_prodR = CodProd.Trim();
                _guardar.cantidad = cantidad;
                _guardar.loteR = LoteFab.Trim();
                _guardar.loteT = LoteT.Trim();
                _guardar.cod_prodT = CodProdT.Trim();             
                _guardar.Pallet = Pallet.Trim();
                _guardar.Estab = General.EstabSession.Trim();
                _guardar.ChkUbicar = ChkUbicar;
                _guardar.loc = localizacion.Trim();

                RestClient client = new RestClient(null);
                var url = "http://" + General.urlWS + "/api/ActualizarUbicacion/Guardar";
                var resp = await client.Post<string>(url, null, _guardar);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo guardar la información.");
                    return;
                }
                await MostrarMsg.ShowMessage("Información guardada correctamente.");               
                Limpiar();

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(string.Format("Error {0}", ex.Message));
            }
        }
        private void Limpiar()
        {
            if (string.IsNullOrEmpty(Loc.cod_prod)) { return; }
            Lote = new CalkinsWS_ActualizarUbicacionLote_Result();
            Loc = new CalkinsWS_ActualizarUbicacionLoc_Result();
            codprod = "";
            Cantidad = 0;
            lotefab = "";
            localizacion = "";
            descripcion = "";
            pallet = "";
            CodProdT = "";
            LoteT = "";
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
