using BMSMobile.Models;
using BMSMobile.Utilities;
using BMSMobile.Views;
using dotMorten.Xamarin.Forms;
using Newtonsoft.Json;
using PropertyChanged;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using static Android.Icu.Text.CaseMap;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SurtidoVM
    {
        SurtidoView _view { get; set; }
        public INavigation Navigation { get; set; }
        public Mensajes MostrarMsg { get; set; }

        private int _pDocIndex { get; set; }
        public int pDocIndex { get { return _pDocIndex; } set { _pDocIndex = value; } }
        private int _pSurtirIndex { get; set; }
        public int pSurtirIndex { get { return _pSurtirIndex; } set { _pSurtirIndex = value; } }
        private int _pTipoIndex { get; set; }
        public int pTipoIndex { get { return _pTipoIndex; } set { _pTipoIndex = value; } }
        private int _nudRackValue { get; set; }
        public int nudRackValue { get { return _nudRackValue; } set { _nudRackValue = value; } }
        private int _nudCantidad { get; set; }
        public int nudCantidad { get { return _nudCantidad; } set { _nudCantidad = value; } }
        private int _nudCantPicking { get; set; }
        public int nudCantPicking { get { return _nudCantPicking; } set { _nudCantPicking = value; } }
        private bool Loc { get; set; }
        private bool Picking { get; set; }

        private string txtDocumento;
        public string TxtDocumento { get => txtDocumento; set { txtDocumento = value; } }
        private string txtFolio;
        public string TxtFolio { get => txtFolio; set { txtFolio = value; } }
        private string TUbicacion { get; set; }

        private int Pos;
        //public int Pos { get => Pos; set { Pos = value; } }

        private string lblnomloc;
        public string lblNomLoc { get => lblnomloc; set { lblnomloc = value; } }

        private string txtDesc;
        public string TxtDesc { get => txtDesc; set { txtDesc = value; } }

        private string txtPallet;
        public string TxtPallet { get => txtPallet; set { txtPallet = value; } }

        private string txtLoc;
        public string TxtLoc { get => txtLoc; set { txtLoc = value; } }

        private bool txtpalletenabled;
        public bool txtPalletEnabled { get => txtpalletenabled; set { txtpalletenabled = value; } }

        private bool txtdocenabled;
        public bool TxtDocEnabled { get => txtdocenabled; set { txtdocenabled = value; } }

        private bool txtLocEnabled;
        public bool TxtLocEnabled { get => txtLocEnabled; set { txtLocEnabled = value; } }

        private bool txtFolioEnabled;
        public bool TxtFolioEnabled { get => txtFolioEnabled; set { txtFolioEnabled = value; } }

        private bool cantPVisible;
        public bool CantPickingVisible { get => cantPVisible; set { cantPVisible = value; } }

        private string lblpallet;
        public string lblPallet { get => lblpallet; set { lblpallet = value; } }

        private string trans { get; set; }
        private bool Surtido { get; set; }

        private string lblLocalizacion;
        public string LblLocalizacion { get => lblLocalizacion; set { lblLocalizacion = value; } }
        public UbicarModel ListaDoc { get; set; }
        private ObservableCollection<SurtidoModel> _listaSurtido { get; set; }
        public ObservableCollection<SurtidoModel> ListaSurtido { get { return _listaSurtido; } set { _listaSurtido = value; } }
        private SurtidoModel itemSelected { get; set; }
        public SurtidoModel ItemSelected { get; set; }
        private ObservableCollection<Area> _listaTipo { get; set; }
        public ObservableCollection<Area> ListaTipo { get { return _listaTipo; } set { _listaTipo = value; } }
        private ObservableCollection<Disponible> _listaDisponible { get; set; }
        public ObservableCollection<Disponible> ListaDisponible { get { return _listaDisponible; } set { _listaDisponible = value; } }
        private ObservableCollection<TipoDocumento> _listaDocs { get; set; }
        public ObservableCollection<TipoDocumento> ListaDocs { get { return _listaDocs; } set { _listaDocs = value; } }

        //private ObservableCollection<tLotes> Lotes { get; set; }
        public Command txtDocCompleted { get; set; }
        public Command pSurtirChanged { get; set; }
        public Command nudRackChanged { get; set; }
        public Command CantPickingCompleted { get; set; }
        public Command txtFolioCompleted { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command RefrescarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command txtLocCompleted { get; set; }
        public Command txtPalletCompleted { get; set; }
        public Command pTipoChanged { get; set; }
        public SurtidoVM(INavigation navigation, SurtidoView view)
        {
            Navigation = navigation;
            _view = view;
            MostrarMsg = new Mensajes();
            ListaDoc = new UbicarModel();
            ListaDocs = ListaDoc.TipoDocuments;
            ListaTipo = ListaDoc.Tipo;
            ListaDisponible = ListaDoc.Disponibles;
            _pDocIndex = 0;
            pTipoIndex = 0;
            pSurtirIndex = 0;
            nudRackValue = 1;
            nudCantPicking = 1;
            Pos = -1;
            txtDocumento = "";
            TxtPallet = "";
            TUbicacion = "";
            TxtFolio = "";
            TxtLoc = "";
            Surtido = false;
            LblLocalizacion = "";
            trans = "";
            Loc = false;
            Picking = false;
            txtPalletEnabled = true;
            TxtDocEnabled = true;
            TxtFolioEnabled = true;
            CantPickingVisible = false;
            TxtLocEnabled = true;
            lblPallet = "Pall";
            lblNomLoc = "Loc";
            ItemSelected = new SurtidoModel();
            ListaSurtido = new ObservableCollection<SurtidoModel>();
            txtDocCompleted = new Command(TraerDocumento);
            pSurtirChanged = new Command(SurtirChanged);
            pTipoChanged = new Command(TipoChanged);
            //Task.Run(async () => await Ubicacion()).Wait();
            //CantPickingCompleted = new Command(CantPCompleted);
            txtFolioCompleted = new Command(TxtFolioCompleted);
            LimpiarCommand = new Command(Limpiar);
            GuardarCommand = new Command(GuardarComm);
            txtLocCompleted = new Command(TxtLocCompleted);
            txtPalletCompleted = new Command(TxtPalletCompleted);
            RefrescarCommand = new Command(RefrescarComm);
        }
        private async void RefrescarComm()
        {
            await Refrescar();
            txtPalletEnabled = false;
            TxtLocEnabled = true;
        }
        private async void TxtPalletCompleted()
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPallet)) { return; }
                TxtPallet = ValidaDigitoVerificador(TxtPallet, true);
                if (TxtPallet == "-1")
                {
                    await MostrarMsg.ShowMessage("Folio de pallet no valido.");
                    return;
                }
                if(ItemSelected.cod_prod != "")
                {
                    if (await VerificarPallet())
                    {
                        await Guardar();
                    }
                    else
                    {
                        TxtLocEnabled = true;
                        txtPalletEnabled = false;
                        TxtPallet = "";
                        TxtLoc = "";
                    }

                }
                return;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private async Task<bool> VerificarPallet()
        {
            var pal = await PalletLoc(TxtLoc.Trim());
            if (pal == null) { return false; }
            if ((pal.Pallet.Trim().ToLower() == TxtPallet.Trim().ToLower() && pal.Cod_prod.Trim().ToUpper() == ItemSelected.cod_prod.Trim().ToUpper()) || TxtLoc.Trim().ToLower() == "piso")
            {
                return true;
            }
            return false;   
        }

        private async void TxtLocCompleted()
        {
           if(TxtLoc == "") { return; }
            Surtido = false;
            TxtLoc = ValidaDigitoVerificador(TxtLoc, false);
            if(TxtLoc == "-1")
            {
                await MostrarMsg.ShowMessage("Localización no valida.");
                return;
            }

            Pos = -1;

            Pos = await Busqueda(TxtLoc.Trim());
            if ( Pos > -1 )
            {
                if(pSurtirIndex == 2)
                {
                    //ABRIR FORMULARIO F04 
                    await Refrescar();
                    TxtLocEnabled = true;
                    txtPalletEnabled = false;
                    return;
                }
                Surtido = true;
            }
            else
            {
                await MostrarMsg.ShowMessage("No existe esta localizacion para surtir.");
                TxtLoc = "";
                return;
            }
            txtPalletEnabled = true;
            TxtLocEnabled = false;
            //txtPallet.Focus();

        }
        private async void GuardarComm()
        {
            if (Surtido && TxtLoc.Trim() != "")
            {
                await Guardar();
            }
            else
            {
                await MostrarMsg.ShowMessage("No ha seleccionado ninguna localización valida");
                return;
            }

        }
        private async void TxtFolioCompleted()
        {
            await Refrescar();
        }

        private async Task Refrescar()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("cod_estab", General.EstabSession.Trim());
                parametros.Add("folio", TxtFolio.Trim());
                parametros.Add("index", pSurtirIndex.ToString());
                var url = "http://" + General.urlWS + "/api/Surtido/Refrescar";
                var resp = await client.Get<ObservableCollection<SurtidoModel>>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No existen pallet's pendientes para surtir.");
                    return;
                }
                else
                {
                    ListaSurtido = resp.Result;
                    var itemsToRemove = ListaSurtido.Where(item => item.localizacion.Trim() == "").ToList();

                    // Eliminar elementos de la ObservableCollection
                    foreach (var item in itemsToRemove)
                    {
                        ListaSurtido.Remove(item);
                    }

                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        public async Task Focus()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Entry entry = (Entry)_view.FindByName("txtFolio");
                    entry.Focus();
                });
            });
        }

        private void SurtirChanged()
        {
            TxtFolio = "";
            TxtDocEnabled = false;

            if (pSurtirIndex == 0) 
            { trans = "511"; TxtDocEnabled = true; }
            if (pSurtirIndex == 1) 
            { trans = "43"; }
            if (pSurtirIndex == 2) 
            { trans = ""; TxtDocEnabled = false; }
            if (pSurtirIndex == 5) 
            { trans = "29"; }

            TxtFolioEnabled = true;

        }

        private void TipoChanged()
        {
            lblPallet = "Pall";
            if (pTipoIndex == 0) { lblPallet = "Prod"; }
        }
      
        private async void TraerDocumento()
        {
            try
            {
                //bool Disponible = pSurtirIndex == 0 ? true : false;
                //bool Pallet = _pDocIndex == 3 ? true : false;
                //bool Picking = pTipoIndex == 1 ? true : false;
               
                //if (pDocIndex == 0) { trans = "44"; }
                //if (pDocIndex == 1) { trans = "631"; }
                //if (pDocIndex == 2) { trans = "65"; }
                //if (pDocIndex == 3) { trans = "44"; }
                //if (pDocIndex == 4) { trans = "56"; }

                //RestClient client = new RestClient(null);
                //Dictionary<string, string> parametros = new Dictionary<string, string>();
                //parametros.Add("folio", TxtDocumento);
                //parametros.Add("trans", trans.ToString());
                //parametros.Add("disponible", Disponible.ToString());
                //parametros.Add("cod_estab", General.userCodEstab);
                //parametros.Add("pallet", Pallet.ToString());
                //parametros.Add("indexDoc", pDocIndex.ToString());
                //parametros.Add("picking", Picking.ToString());
                //var url = "http://" + General.urlWS + "/api/Ubicar/Refrescar";
                //var resp = await client.Get<ObservableCollection<tLotes>>(url, parametros);
           

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<PalletLoc> PalletLoc(string Loc)
        {
            try
            {             
                        RestClient client = new RestClient(null);
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        var url = "http://" + General.urlWS + "/api/Surtido/PalletLoc";
                        parametros.Add("Loc",Loc);

                        var resp = await client.Get<PalletLoc>(url, parametros);
                        if (!resp.Ok)
                        {
                            await MostrarMsg.ShowMessage("Este Pallet no corresponde al solicitado o el producto ubicado en la posición no corresponde al producto de la salida de hospedaje.");
                            return null;
                        }

                        var contenido = resp.Result;
                         return contenido;                 
                }
            
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        public async Task<bool> Guardar()
        {
            if (!await VerificarPallet())
            {
                return false;
            }

            try
            {
                if (ItemSelected == null) { return false; }
                if (TxtLoc.Trim().ToLower() == "piso")
                {
                    if (pTipoIndex == 1)
                    {
                        await MostrarMsg.ShowMessage("En piso tiene que escanear el pallet");
                        return false;
                    }
                }
                RestClient client = new RestClient(null);
                var url = "";
                GuardarSurtido guardar = new GuardarSurtido();
                guardar.Loc = TxtLoc.Trim().ToLower();
                guardar.Ticket = itemSelected.ticket.Trim();
                guardar.Trans = trans;
                guardar.TransSurtido = itemSelected.transaccion_surtido.Trim();
                guardar.Usuario = General.userCode.Trim();
                guardar.Documento = TxtDocumento.Trim();
                guardar.CantSurtida = itemSelected.cantidad_surtida.ToString().Trim();
                guardar.CodEstab = General.EstabSession.Trim();
                guardar.Folio = TxtFolio.Trim();
                guardar.CodProd = itemSelected.cod_prod.ToString().Trim();
                guardar.FolioSurtido = itemSelected.folio_surtido.Trim();

                url = "http://" + General.urlWS + "/api/Surtido/Guardar";
                var resp = await client.Post<string>(url, null, guardar);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo guardar la información");
                    return false;
                }
                else
                {
                    var mensaje = resp.Message;
                    if (mensaje != "")
                    {
                        await MostrarMsg.ShowMessage(mensaje);
                        return false;
                    }
                }
                TxtLoc = "";
                TxtPallet = "";
                TxtDocumento = "";
                txtPalletEnabled = false;
                TxtLocEnabled = true;
                Limpiar();
                return true;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                return false;
            }
        }

        private void Limpiar()
        {
            ListaSurtido.Clear();
            nudCantidad = 0;
            nudCantPicking = 0;
            nudRackValue = 0;
            TxtDesc = "";
            TxtDocumento = "";
            TxtPallet = "";
            LblLocalizacion = "";
            Loc = false;
            Picking = false;
            txtPalletEnabled = false;
            TxtDocEnabled = false;
            TxtFolioEnabled = true;
            lblPallet = "Pallet";
            CantPickingVisible = false;

        }

      
        public async Task<int> Busqueda(string Folio) 
        {
           int index = ListaSurtido.IndexOf(ListaSurtido.FirstOrDefault(item => item.localizacion == Folio));
            return await Task.FromResult(index);
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


