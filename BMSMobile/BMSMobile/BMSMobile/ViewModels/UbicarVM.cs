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

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class UbicarVM
    {
        UbicarView _view { get; set; }
        public INavigation Navigation { get; set; }
        public Mensajes MostrarMsg { get; set; }

        private int _pDocIndex { get; set; }
        public int pDocIndex { get { return _pDocIndex; } set { _pDocIndex = value; } }
        private int _pDispIndex { get; set; }
        public int pDispIndex { get { return _pDispIndex; } set { _pDispIndex = value; } }
        private int _pAreaIndex { get; set; }
        public int pAreaIndex { get { return _pAreaIndex; } set { _pAreaIndex = value; } }
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
        private string TUbicacion { get; set; }

        private int Pos;
        //public int Pos { get => Pos; set { Pos = value; } }

        private string lblnomloc;
        public string lblNomLoc { get => lblnomloc; set { lblnomloc = value; } }

        private string txtDesc;
        public string TxtDesc { get => txtDesc; set { txtDesc = value; } }

        private string txtPallet;
        public string TxtPallet { get => txtPallet; set { txtPallet = value; } }

        private bool txtpalletenabled;
        public bool txtPalletEnabled { get => txtpalletenabled; set { txtpalletenabled = value; } }
        private bool txtdocenabled;
        public bool txtDocEnabled { get => txtdocenabled; set { txtdocenabled = value; } }

        private bool cantPVisible;
        public bool CantPickingVisible { get => cantPVisible; set { cantPVisible = value; } }

        private string lblpallet;
        public string lblPallet { get => lblpallet; set { lblpallet = value; } }

        private string lblLocalizacion;
        public string LblLocalizacion { get => lblLocalizacion; set { lblLocalizacion = value; } }
        public UbicarModel ListaDoc { get; set; }
        private ObservableCollection<tLotes> _listaLotes { get; set; }
        public ObservableCollection<tLotes> ListaLotes { get { return _listaLotes; } set { _listaLotes = value; } }
        private ObservableCollection<Area> _listaAreas { get; set; }
        public ObservableCollection<Area> ListaAreas { get { return _listaAreas; } set { _listaAreas = value; } }
        private ObservableCollection<Disponible> _listaDisponible { get; set; }
        public ObservableCollection<Disponible> ListaDisponible { get { return _listaDisponible; } set { _listaDisponible = value; } }
        private ObservableCollection<TipoDocumento> _listaDocs { get; set; }
        public ObservableCollection<TipoDocumento> ListaDocs { get { return _listaDocs; } set { _listaDocs = value; } }

        //private ObservableCollection<tLotes> Lotes { get; set; }
        public Command txtDocumentoCompleted { get; set; }
        public Command pAreaChanged { get; set; }
        public Command nudRackChanged { get; set; } 
        public Command CantPickingCompleted { get; set; }
        public Command txtPalletCompleted { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command GuardarCommand { get; set; }

        private TransTipoDoc TransDod;
        public UbicarVM(INavigation navigation, UbicarView view)
        {
            Navigation = navigation;
            _view = view;
            MostrarMsg = new Mensajes();
            ListaDoc = new UbicarModel();
            ListaDocs = ListaDoc.TipoDocuments;
            ListaAreas = ListaDoc.Areas;
            ListaDisponible = ListaDoc.Disponibles;
            _pDocIndex = 0;
            _pAreaIndex = 0;
            _pDispIndex = 0;
            nudRackValue = 1;
            nudCantPicking = 1;
            Pos = -1;
            txtDocumento = "";
            TxtPallet = "";
            TUbicacion = "";
            LblLocalizacion = "";
            Loc = false;
            Picking = false;
            txtPalletEnabled = true;
            txtDocEnabled = true;
            CantPickingVisible = false;
            lblpallet = "Pallet";
            lblNomLoc = "Loc";
            ListaLotes = new ObservableCollection<tLotes>();
            txtDocumentoCompleted = new Command(TraerDocumento);
            pAreaChanged = new Command(AreaChanged);
            Task.Run(async () => await Ubicacion()).Wait();
            nudRackChanged = new Command(NudRackChanged);
            CantPickingCompleted = new Command(CantPCompleted);
            txtPalletCompleted = new Command(TxtPalletCompleted);
            LimpiarCommand = new Command(Limpiar);
            GuardarCommand = new Command(GuardarComm);
        }


        private async void GuardarComm()
        {
            await Guardar();
        }
        private async void TxtPalletCompleted()
        {
            await PalletLoc();
        }
        private async void CantPCompleted()
        {
            await Guardar();
        }
        private void NudRackChanged()
        {
            lblPallet = "Pallet";
            LblLocalizacion = "";
            TxtPallet = "";
            Loc = false;
        }

        private async Task Ubicacion()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Estab", General.EstabSession.Trim());
                var url = "http://" + General.urlWS + "/api/Ubicar/UbicacionSugerida";
                var resp = await client.Get<string>(url,parametros);

                if (!resp.Ok)
                {
                    TUbicacion = "O";
                }
                else
                {
                    var info = resp.Result;
                    TUbicacion = info.ToString();
                }
            }

            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }
        private void AreaChanged()
        {
            Picking = false;
            lblnomloc = "Loc";
            CantPickingVisible = false;
            if(pAreaIndex == 1)
            {
                Picking=true;
                lblPallet = "Producto";
                CantPickingVisible = true;
            }

        }

        private async void TraerDocumento()
        {
            try
            {
                bool Disponible = _pDispIndex == 0 ? true : false;
                bool Pallet = _pDocIndex == 3 ? true : false;
                bool Picking = pAreaIndex == 1 ? true : false;
                string trans = "";
               if (pDocIndex == 0) { trans = "44"; } if (pDocIndex == 1) { trans = "631"; }if (pDocIndex == 2) { trans = "65"; } if (pDocIndex == 3) {trans = "44"; } if(pDocIndex == 4) { trans = "56"; }

                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("folio", TxtDocumento);
                parametros.Add("trans", trans.ToString());
                parametros.Add("disponible", Disponible.ToString());
                parametros.Add("cod_estab", General.userCodEstab);
                parametros.Add("pallet", Pallet.ToString());
                parametros.Add("indexDoc", pDocIndex.ToString());
                parametros.Add("picking", Picking.ToString());
                var url = "http://" + General.urlWS + "/api/Ubicar/Refrescar";
                var resp = await client.Get<ObservableCollection<tLotes>>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo obtener la información");
                    txtPalletEnabled = false;
                    txtDocEnabled = true;
                    return;
                }
                else
                {
                    txtDocEnabled = false; txtPalletEnabled = true; 
                    
                    ListaLotes = resp.Result;

                    if (ListaLotes[0].cod_estab.Trim() != General.EstabSession.Trim() && Pallet)
                    {
                        await MostrarMsg.ShowMessage("El pallet no pertenece a este establecimiento");
                        txtPalletEnabled = false;
                        txtDocEnabled = true;
                        return;
                    }
                    if (_pAreaIndex == 1)
                    {
                        for (int i = ListaLotes.Count - 1; i >= 0; i--)
                        {
                            tLotes elemento = ListaLotes[i];
                            elemento.cant_pendiente = elemento.picking - elemento.cant_ubicada;

                            if (elemento.cant_pendiente <= 0)
                            {
                                ListaLotes.RemoveAt(i);
                            }
                        }
                        if (ListaLotes.Count <= 0)
                        {
                            await MostrarMsg.ShowMessage("No existen productos pendientes por ubicar");
                            txtPalletEnabled = false;
                            txtDocEnabled = true;
                            return;
                        }
                    }
                    if (Pallet)
                    {
                        TxtPallet = txtDocumento;
                        await PalletLoc();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task PalletLoc()
        {
            try
            {
                string locAsign, locInter;

                if (!Loc)
                {
                    if (_pAreaIndex == 1)
                    {
                        RestClient client = new RestClient(null);
                        Dictionary<string, string> parametros = new Dictionary<string, string>();
                        var url = "http://" + General.urlWS + "/api/Entrada/TraerProducto";
                        //Cod_prv,string Cod_estab,string Cod_prod
                        parametros.Add("Cod_prv", "");
                        parametros.Add("Cod_estab", General.EstabSession);
                        parametros.Add("Cod_prod", TxtPallet.Trim());

                        var resp = await client.Get<Producto>(url, parametros);
                        if (!resp.Ok)
                        {
                            await MostrarMsg.ShowMessage("Código de producto inexistente o sin estatus válido.");
                            return;
                        }

                        var contenido = resp.Result;
                        TxtPallet = contenido.cod_prod.Trim();

                    }
                    else
                    {
                        TxtPallet = ValidaDigitoVerificador(TxtPallet, true);
                        if (TxtPallet == "-1")
                        {
                            await MostrarMsg.ShowMessage("Pallet no válido.");
                            return;
                        }
                    }

                    Pos = await Busqueda(txtPallet);

                    if (Pos > -1)
                    {
                        if (!await Localizacion(Pos)) { return; }
                        if (LblLocalizacion.Trim() != "")
                        {
                            lblpallet = "Ubicación";
                            TxtPallet = "";
                            Loc = true;

                        }
                        else
                        {
                            await MostrarMsg.ShowMessage("Pallet no valido para esta recepción.");
                            TxtPallet = "";
                            return;
                        }
                    } 
                }
                else
                {
                    if (Picking)
                    {
                        if (ListaLotes[Pos].cant_ubicada >= ListaLotes[Pos].picking)
                        {
                            await MostrarMsg.ShowMessage("Este producto no tiene mas producto pendiente de ubicar.");
                            TxtPallet = "";
                            return;
                        }
                        CantPickingVisible = true;
                        nudCantPicking = ListaLotes[Pos].picking - ListaLotes[Pos].cant_ubicada;
                    }
                    else
                    {
                        TxtPallet = ValidaDigitoVerificador(TxtPallet.Trim(), false);
                        if (TxtPallet == "-1")
                        {
                            await MostrarMsg.ShowMessage("Localización no valida.");
                            txtPallet = "";
                            return;
                        }
                    }

                    await Guardar();

                }                                  

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        public async Task<bool> Guardar()
        {
            try
            {
                if(ListaLotes.Count <= 0) { return false; }
                if (TxtPallet == "") {return false; }
                if (!await ValidaPicking()) { return false; }
                var cant = ListaLotes[Pos].cant_pendiente;
                if (Picking) { cant = nudCantPicking; }

                if (TUbicacion.ToUpper().Trim() != "O")
                {
                    if(TxtPallet.Trim().ToLower() != LblLocalizacion.Trim().ToLower() & LblLocalizacion.Trim() != "")
                    {
                        await MostrarMsg.ShowMessage("Esta localización no es para este pallet.");
                        return false;
                    }
                }
                RestClient client = new RestClient(null);
                var url = "";
                    bool Disponible = true;
                    if(pDispIndex == 1) { Disponible = false; }
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("folio", ListaLotes[Pos].folio.Trim());
                    parametros.Add("disponible", Disponible.ToString());
                    parametros.Add("cod_estab", General.userCodEstab);
                    parametros.Add("loc", TxtPallet.Trim());
                    parametros.Add("picking", Picking.ToString());
                    url = "http://" + General.urlWS + "/api/Ubicar/Guardar";
                    var resp2 = await client.Post<bool>(url, parametros);

                    if (!resp2.Ok)
                    {
                        await MostrarMsg.ShowMessage("No se pudo guardar la información");
                        return false;
                    }
                    else
                    {
                    ModificaInventarioLoc modificaInventarioLoc = new ModificaInventarioLoc();
                    modificaInventarioLoc.CodProd = ListaLotes[Pos].cod_prod.Trim();
                    modificaInventarioLoc.CodEstab = General.EstabSession.Trim();
                    modificaInventarioLoc.Transaccion = "622";
                    modificaInventarioLoc.Unidad = ListaLotes[Pos].unidad.Trim();
                    modificaInventarioLoc.Cantidad = cant;
                    modificaInventarioLoc.Guardar = true;
                    modificaInventarioLoc.MSG = "";
                    modificaInventarioLoc.Localizacion = txtPallet.Trim();
                    modificaInventarioLoc.AfectaNoDisponible = false;
                    modificaInventarioLoc.Lote = ListaLotes[Pos].lote_fabricacion.Trim();
                    modificaInventarioLoc.FolioRef = ListaLotes[Pos].folio_referencia.Trim();
                    modificaInventarioLoc.TransRef = "44";
                    modificaInventarioLoc.LoteRecepcion = ListaLotes[Pos].folio.Trim();

                    url = "http://" + General.urlWS + "/api/Inventario/ModificaInventarioLoc";
                    var resp = await client.Post<int>(url, null, modificaInventarioLoc);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage(" No se pudo guardar la información.");
                        return false;
                    }
                }
                //}

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
            ListaLotes.Clear();
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
            txtDocEnabled = true;
            lblPallet = "Pallet";   
            CantPickingVisible = false;

        }

        public async Task<bool> ValidaPicking()
        {
            try
            {
                if (Picking) 
                {
                    if(nudCantPicking > ListaLotes[Pos].picking)
                    {
                        await MostrarMsg.ShowMessage("La cantidad es mayor a la permitida");
                        return false;
                    }
                    if (nudCantPicking < 1)
                    {
                        await MostrarMsg.ShowMessage("No se a seleccionado cantidad para guardar");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                return false;
            }
        }

        public async Task<bool> Localizacion(int pos)
        {
            try
            {
                bool Picking = pAreaIndex == 1 ? true : false;
                bool Disponible = _pDispIndex == 0 ? true : false;
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                var url = "http://" + General.urlWS + "/api/ubicar/Localizacion";
                //Cod_prv,string Cod_estab,string Cod_prod
                parametros.Add("cod_prod", ListaLotes[pos].cod_prod.Trim());
                parametros.Add("folio", ListaLotes[pos].folio.Trim());
                parametros.Add("cod_estab", General.EstabSession);
                parametros.Add("rack", nudRackValue.ToString());
                parametros.Add("disponible", Disponible.ToString());
                parametros.Add("pickslot", Picking.ToString());

                var resp = await client.Get<string>(url, parametros);
                if (!resp.Ok)
                {
                    LblLocalizacion = "";
                    return false;
                }
                var contenido = resp.Result;
                LblLocalizacion = contenido.ToString().Trim();
                if(TUbicacion != "O") { LblLocalizacion = Loc.ToString(); }
                return true;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<int> Busqueda(string Folio)
        {

            var item = ListaLotes.FirstOrDefault(o => o.folio.Trim() == TxtPallet.Trim());

            if (item != null)
            {
                int indice = ListaLotes.IndexOf(item);
                TxtDesc = ListaLotes[indice].descripcion.Trim();
                nudCantidad = (int)ListaLotes[indice].cant_pendiente;
                return indice;
            }
            else
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("folio", Folio);
                parametros.Add("documento", TxtDocumento.Trim());
                var url = "http://" + General.urlWS + "/api/Ubicar/Busqueda";
                var resp = await client.Get<ObservableCollection<tLotes>>(url, parametros);

                if (!resp.Ok)
                {
                    return -1;
                }
                else
                {
                    ListaLotes = resp.Result;
                    TxtDesc = ListaLotes[ListaLotes.Count - 1].descripcion.Trim();
                    nudCantidad = (int)ListaLotes[ListaLotes.Count - 1].cant_pendiente;
                    return ListaLotes.Count - 1;
                }
            }
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

    public enum TransTipoDoc
    {
        Recep = 44,
        EntradaXdev = 631,
        RecepcionTransf = 65,
        Entrada = 56
    }



}


