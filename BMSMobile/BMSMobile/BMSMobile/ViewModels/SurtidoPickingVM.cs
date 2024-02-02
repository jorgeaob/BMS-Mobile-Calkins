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
using System.Data;
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
using static Android.Util.EventLogTags;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SurtidoPickingVM
    {
        SurtidoPickingView _view { get; set; }
        public INavigation Navigation { get; set; }
        public Mensajes MostrarMsg { get; set; }

        private int _pTransIndex { get; set; }
        public int pTransIndex { get { return _pTransIndex; } set { _pTransIndex = value; } }
        private int _pPickIndex { get; set; }
        public int pPickIndex { get { return _pPickIndex; } set { _pPickIndex = value; } }

        private string txtFolio;
        public string TxtFolio { get => txtFolio; set { txtFolio = value; } }

        private int Pos;
        //public int Pos { get => Pos; set { Pos = value; } }

        private string lblnomloc;
        public string lblNomLoc { get => lblnomloc; set { lblnomloc = value; } }

        private string txtDesc;
        public string TxtDesc { get => txtDesc; set { txtDesc = value; } }

        private string txtPallet;
        public string TxtPallet { get => txtPallet; set { txtPallet = value; } }
        private string _Cantidad;
        public string Cantidad { get => _Cantidad; set { _Cantidad = value; } }

        private bool _txtCodProdEnabled;
        public bool txtCodProdEnabled { get => _txtCodProdEnabled; set { _txtCodProdEnabled = value; } }
        private bool _cbTransEnabled;
        public bool cbTransEnabled { get => _cbTransEnabled; set { _cbTransEnabled = value; } }

        private bool _txtFolioEnabled;
        public bool txtFolioEnabled { get => _txtFolioEnabled; set { _txtFolioEnabled = value; } }
        private bool _txtCantEnabled;
        public bool txtCantEnabled { get => _txtCantEnabled; set { _txtCantEnabled = value; } }

        private bool txtLocEnabled;
        public bool TxtLocEnabled { get => txtLocEnabled; set { txtLocEnabled = value; } }

        private string _Localizaciones;
        public string Localizaciones { get => _Localizaciones; set { _Localizaciones = value; } }

        private string _CodProd;
        public string CodProd { get => _CodProd; set { _CodProd = value; } }
        private string _Descripcion;
        public string Descripcion { get => _Descripcion; set { _Descripcion = value; } }
        private string trans { get; set; }
        private bool Surtido { get; set; }

        private string _Localizacion;
        public string Localizacion { get => _Localizacion; set { _Localizacion = value; } }
        public UbicarModel ListaDoc { get; set; }
        private ObservableCollection<TipoDocumento> _ListaPick { get; set; }
        public ObservableCollection<TipoDocumento> ListaPick { get { return _ListaPick; } set { _ListaPick = value; } }
        public Tickets ItemSelected { get; set; }
        private ObservableCollection<Tickets> _ListaTickets { get; set; }
        public ObservableCollection<Tickets> ListaTickets { get { return _ListaTickets; } set { _ListaTickets = value; } }
        private ObservableCollection<ProductosTicket> _ListaProductos { get; set; }
        public ObservableCollection<ProductosTicket> ListaProductos { get { return _ListaProductos; } set { _ListaProductos = value; } }
        private ObservableCollection<TipoDocumento> _ListaTrans { get; set; }
        public ObservableCollection<TipoDocumento> ListaTrans { get { return _ListaTrans; } set { _ListaTrans = value; } }
        private string TransDoc = "";

        //private ObservableCollection<tLotes> Lotes { get; set; }
        public Command txtCantidadCompleted { get; set; }
        public Command pSurtirChanged { get; set; }
        public Command nudRackChanged { get; set; }
        public Command CantPickingCompleted { get; set; }
        public Command txtFolioCompleted { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command DeshacerCommand { get; set; }
        public Command RefrescarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command txtLocCompleted { get; set; }
        public Command txtCodProdCompleted { get; set; }
        public Command pTipoChanged { get; set; }
        public SurtidoPickingVM(INavigation navigation, SurtidoPickingView view)
        {
            Navigation = navigation;
            _view = view;
            MostrarMsg = new Mensajes();
            ListaDoc = new UbicarModel();
            //ListaTrans = ListaDoc.TipoDocuments;
            ListaTickets = new ObservableCollection<Tickets>();
            ListaProductos = new ObservableCollection<ProductosTicket>();
            ListaPick = new ObservableCollection<TipoDocumento> { new TipoDocumento { Nombre = "Localización" }, new TipoDocumento { Nombre = "Libre" } };
            pTransIndex = 0;
            pPickIndex = 0;
            Pos = -1;
            TxtFolio = "";
            Surtido = false;
            Localizacion = "";
            CodProd = "";
            Descripcion = "";
            Localizaciones = "Localizaciones";
            Cantidad = "0";
            txtCodProdEnabled = false;           
            txtFolioEnabled = true;
            txtCantEnabled = false;
            TxtLocEnabled = true;
            cbTransEnabled = true;
            ListaTrans = new ObservableCollection<TipoDocumento> { new TipoDocumento { Nombre = "Orden de carga" }, new TipoDocumento { Nombre = "Embarque" }, new TipoDocumento { Nombre = "Pedido a establecimiento" } };
            ItemSelected = new Tickets();
            txtCantidadCompleted = new Command(TxtCantidadCompleted);
            //pSurtirChanged = new Command(SurtirChanged);
            txtFolioCompleted = new Command(TxtFolioCompleted);
            LimpiarCommand = new Command(LimpiarProd);
            txtLocCompleted = new Command(TxtLocCompleted);
            txtCodProdCompleted = new Command(TxtCodProdCompleted);
            DeshacerCommand = new Command(Limpiar);
        }
        private async void RefrescarComm()
        {
            await Refrescar("");
            TxtLocEnabled = true;
        }
        private async void TxtCodProdCompleted()
        {
            try
            {
                if (!await TraerProducto(CodProd.Trim()))
                {
                    Entry entry = (Entry)_view.FindByName("txtCodProd");
                    entry.Focus();
                }
                else
                {
                    txtCodProdEnabled = false;
                    if(pPickIndex == 0)
                    {
                        TxtLocEnabled = true;
                        Entry entry = (Entry)_view.FindByName("txtLoc");
                        entry.Focus();
                    }
                    else
                    {
                        txtCantEnabled = true;
                        Entry entry = (Entry)_view.FindByName("txtCantidad");
                        entry.Focus();
                    }
                }


            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private async Task<bool> TraerProducto(string Prod)
        {
            if (ItemSelected == null)
            {
                await MostrarMsg.ShowMessage("No ha seleccionado Ticket.");
                return false; 
            }
            RestClient client = new RestClient(null);
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            var url = "http://" + General.urlWS + "/api/Entrada/TraerProducto";
            parametros.Add("Cod_prv", "");
            parametros.Add("Cod_estab", General.EstabSession);
            parametros.Add("Cod_prod", Prod.Trim());

            var resp = await client.Get<Producto>(url, parametros);
            if (!resp.Ok)
            {
                await MostrarMsg.ShowMessage("Código de producto inexistente o sin estatus válido.");
                return false;
            }

            var contenido = resp.Result;
            CodProd = contenido.cod_prod.Trim();

            var existeProducto = ListaProductos.Any(p => p.cod_prod.Trim() == CodProd.Trim());
            if (!existeProducto)
            {
                await MostrarMsg.ShowMessage("Este producto no se encuentra en el ticket.");
                return false;
            }
            if(pPickIndex == 0)
            {
                parametros = new Dictionary<string, string>();
                url = "http://" + General.urlWS + "/api/SurtidoPicking/LocProductoIntegracion";
                parametros.Add("cod_cte", ItemSelected.cod_cte.Trim());
                parametros.Add("numdpc", ItemSelected.numdpc.Trim());
                parametros.Add("cod_estab", General.EstabSession);
                parametros.Add("cod_prod", CodProd.Trim());

                var resp2 = await client.Get<string>(url, parametros);
                if (!resp2.Ok)
                {
                    await MostrarMsg.ShowMessage("Código de producto inexistente o sin estatus válido.");
                    return false;
                }

                Localizaciones = resp2.Result;
                if(Localizaciones == "")
                {
                    await MostrarMsg.ShowMessage("No existe mercancía ubicada para este producto.");
                    return false;
                }
            }
            Descripcion = contenido.descripcion_completa.Trim();
            return true;
        }

        private async void TxtLocCompleted()
        {
            if (! await ValidarLocalizacion(Localizacion.Trim()))
            {
                Entry entry = (Entry)_view.FindByName("txtLoc");
                entry.Focus();
            }
            else
            {
                TxtLocEnabled = false;
                Entry entry = (Entry)_view.FindByName("txtCantidad");
                entry.Focus();
            }
        }

        private async Task<bool> ValidarLocalizacion(string Loc)
        {
            Localizacion = ValidaDigitoVerificador(Loc, false);
            if (Localizacion == "-1")
            {
                await MostrarMsg.ShowMessage("Localizacion no valida");
                return false;
            }

            RestClient client = new RestClient(null);
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("loc", Localizacion.Trim());
            parametros.Add("estab", General.EstabSession.Trim());
            var url = "http://" + General.urlWS + "/api/SurtidoPicking/ValidarLocalizacion";
            var resp = await client.Get<SurtidoPickingValidaLoc>(url, parametros);

            if (!resp.Ok)
            {
                await MostrarMsg.ShowMessage("La localización no existe en el establecimiento o no es de picking");
                return false;
            }
            else
            {
                SurtidoPickingValidaLoc loc = resp.Result;
                if(loc.cod_prod.Trim().ToUpper() != CodProd.Trim().ToUpper())
                {
                    await MostrarMsg.ShowMessage("Este producto no existe en esta localización");
                    return false;
                }
            }
            return true;
        }
       
        private async void TxtFolioCompleted()
        {
            await Refrescar("");
        }

        private async Task Refrescar(string ticket)
        {
            try
            {
                ListaTickets.Clear();
                ListaTickets.Clear();

                if (pTransIndex == 0) { TransDoc = "511"; } else if (pTransIndex == 1) { TransDoc = "43"; } else if (pTransIndex == 2) { TransDoc = "29"; }
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("trans", TransDoc.Trim());
                parametros.Add("folio", TxtFolio.Trim());
                parametros.Add("cod_estab", General.EstabSession.Trim());
                var url = "http://" + General.urlWS + "/api/SurtidoPicking/Refrescar";
                var resp = await client.Get<ObservableCollection<Tickets>>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("Documento no válido");
                    return;
                }
                else
                {
                    ListaTickets = resp.Result;
                    txtFolioEnabled = false;
                    cbTransEnabled = false;
                    if (ticket == "") { ticket = ListaTickets[0].ticket.Trim(); }
                    await TraerProdTickets(ticket);
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

        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (Tickets)item;
            //Entry txtCodProd = (Entry)this._view.FindByName("txtCodProd");
            var Ticket = i.ticket.Trim();
            await TraerProdTickets(Ticket);
        });

        private async void TxtCantidadCompleted()
        {
            try
            {
                if (Cantidad != "")
                {
                    if (ItemSelected != null)
                    {
                        if (await Guardar())
                        {
                            await MostrarMsg.ShowMessage("Información guardada correctamente");
                            await TraerProdTickets(ItemSelected.ticket.Trim());
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task TraerProdTickets(string ticket)
        {
            try
            {
                LimpiarProd();
                ListaProductos = new ObservableCollection<ProductosTicket>();
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                var url = "http://" + General.urlWS + "/api/SurtidoPicking/TraerProdTickets";
                parametros.Add("folio", TxtFolio.Trim());
                parametros.Add("cod_estab", General.EstabSession.Trim());
                parametros.Add("trans", TransDoc.Trim());
                parametros.Add("ticket", ticket.Trim());

                var resp = await client.Get<ObservableCollection<ProductosTicket>>(url, parametros);
                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("Este ticket no tiene productos pendientes de integrar.");
                }

                if (resp.Result.Count <= 0) { await MostrarMsg.ShowMessage("Este ticket no tiene productos pendientes de integrar."); txtCodProdEnabled = false; }

                ListaProductos = resp.Result;
            }

            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private void LimpiarProd()
        {
            CodProd = "";
            Localizacion = "";
            Cantidad = "0";
            Descripcion = "";
            Localizaciones = "";
            txtCodProdEnabled = true;
            TxtLocEnabled = false;
            txtCantEnabled = false;
            Entry entry = (Entry)_view.FindByName("txtCodProd");
            entry.Focus();
        }

 

        public async Task<bool> Guardar()
        {
           if(ItemSelected == null)
            {
                await MostrarMsg.ShowMessage("No hay ticket seleccionado");
                return false;
            }
            if (Descripcion == "")
            {
                await MostrarMsg.ShowMessage("No hay producto seleccionado");
                return false;
            }
            if(Cantidad.Trim() == "")
            {
                await MostrarMsg.ShowMessage("No ha proporcionado cantidad");
                return false;
            }
            var existeProducto = ListaProductos.Where(p => p.cod_prod.Trim() == CodProd).FirstOrDefault();
            if (existeProducto == null)
            {
                await MostrarMsg.ShowMessage("El producto " + CodProd.Trim() + " no se encuentra en el ticket.");
                return false;
            }
            if((existeProducto.cantidad_surtida + decimal.Parse(Cantidad)) > existeProducto.cantidad)
            {
                await MostrarMsg.ShowMessage("No puede surtir mas de la cantidad pedida.");
                Cantidad = (existeProducto.cantidad - existeProducto.cantidad_surtida).ToString();
                return false;
            }
            decimal Cant = decimal.Parse(Cantidad);
            bool NoAfectado = false;
            try
            {

                RestClient client = new RestClient(null);
                var url = "";
                GuardarSurtidoPicking guardar = new GuardarSurtidoPicking();
                guardar.Folio = TxtFolio.Trim();
                guardar.Trans = TransDoc.Trim();
                guardar.Ticket = ItemSelected.ticket.Trim();
                guardar.CodCte = ItemSelected.cod_cte.Trim();
                guardar.Numdpc = ItemSelected.numdpc.Trim();
                guardar.Prod = CodProd.Trim();
                guardar.Unid = "U";
                guardar.Localizacion = Localizacion.Trim();
                guardar.Cant = decimal.Parse(Cantidad);
                guardar.Estab = General.EstabSession.Trim();
                guardar.Usuario = General.userCode.Trim();
                guardar.Contenedor = ItemSelected.contenedor.ToString().Trim();
                guardar.TipoPick = pPickIndex;

                url = "http://" + General.urlWS + "/api/SurtidoPicking/Guardar";
                var resp = await client.Post<string>(url, null, guardar);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo guardar la información");
                    return false;
                }
                else
                {
                    var mensaje = resp.Message;
                    if (mensaje != "" && mensaje != null)
                    {
                        await MostrarMsg.ShowMessage(mensaje);
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

        private void Limpiar()
        {
            ListaTickets = new ObservableCollection<Tickets>();
            ListaProductos = new ObservableCollection<ProductosTicket>();
            ItemSelected = null;
            TxtDesc = "";
            TxtFolio = "";
            TxtPallet = "";
            Localizacion = "";
            txtFolioEnabled = true;
            cbTransEnabled = true;
            pPickIndex = 0;
            pTransIndex = 0;
            Descripcion = "";
            txtCodProdEnabled = false;
            TxtLocEnabled = false;
            txtCantEnabled = false;
            Cantidad = "";
            Entry entry = (Entry)_view.FindByName("txtFolio");
            entry.Focus();
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


