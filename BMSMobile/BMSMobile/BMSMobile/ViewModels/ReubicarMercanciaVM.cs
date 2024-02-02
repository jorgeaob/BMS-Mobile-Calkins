using Android.Content;
using Android.Content.Res;
using Android.Media;
using Android.Nfc.Tech;
using Android.Util;
using BMSMobile.Utilities;
using BMSMobile.ViewModels;
using BMSMobile.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Android.Hardware.Camera;
using static Android.Icu.Text.Transliterator;
using static Android.Provider.ContactsContract.CommonDataKinds;
using static Android.Webkit.WebStorage;

namespace BMSMobile.Models
{
    [AddINotifyPropertyChangedInterface]
    internal class ReubicarMercanciaVM
    {
        public INavigation navigaton { get; set; }
        public ReubicarMercanciaView view { get; set; }
        public GetNuevoFolio getNuevoFolio { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command txtCodProdCompleted { get; set; }
        public Command ProdTextChangedCommand { get; set; }
        public Command txtLocAnteriorCompleted { get; set; }
        public Command txtLocNuevaCompleted { get; set; }
        private string loteFab { get; set; }
        public string LoteFab { get => loteFab; set { loteFab = value; } }
        private string loteRecep { get; set; }
        public string LoteRecep { get => loteRecep; set { loteRecep = value; } }
        private string locNueva { get; set; }
        public string LocalizacionNueva { get => locNueva; set { locNueva = value; } }
        private string codprod { get; set; }
        public string CodProd { get => codprod; set { codprod = value; } }
        private string descripcion { get; set; }
        public string Descripcion { get => descripcion; set { descripcion = value; } }
        private string cantidad { get; set; } = "0";
        public string Cantidad { get => cantidad; set { cantidad = value; } }
        private string cantidad2 { get; set; } = "0";
        public string Cantidad2 { get => cantidad2; set { cantidad2 = value; } }
        private string _lblLocalizacion { get; set; }
        public string lblLocalizacion { get => _lblLocalizacion; set { _lblLocalizacion = value; } }
        private string _lblLocalizaciones { get; set; }
        public string lblLocalizaciones { get => _lblLocalizaciones; set { _lblLocalizaciones = value; } }
        private string _lblIntLocalizacion { get; set; }
        public string lblIntLocalizacion { get => _lblIntLocalizacion; set { _lblIntLocalizacion = value; } }
        private string _txtLocAnterior { get; set; }
        public string txtLocAnterior { get => _txtLocAnterior; set { _txtLocAnterior = value; } }
        private bool _lblSolicitudVisible { get; set; } = false;
        public bool lblSolicitudVisible { get => _lblSolicitudVisible; set { _lblSolicitudVisible = value; } }
        private bool _txtSolicitudVisible { get; set; } = false;
        public bool txtSolicitudVisible { get => _txtSolicitudVisible; set { _txtSolicitudVisible = value; } }
        private bool _lblCantVisible { get; set; } = false;
        public bool lblCantVisible { get => _lblCantVisible; set { _lblCantVisible = value; } }
        private bool _txtCant2Visible { get; set; } = false;
        public bool txtCant2Visible { get => _txtCant2Visible; set { _txtCant2Visible = value; } }
        private bool _lblRNDVisible { get; set; } = false;
        public bool lblRNDVisible { get => _lblRNDVisible; set { _lblRNDVisible = value; } }
        private bool _pRNDVisible { get; set; } = false;
        public bool pRNDVisible { get => _pRNDVisible; set { _pRNDVisible = value; } }
        private bool _lblLocDisVisible { get; set; } = false;
        public bool lblLocDisVisible { get => _lblLocDisVisible; set { _lblLocDisVisible = value; } }
        private bool _txtLocNuevaEnabled { get; set; } = true;
        public bool txtLocNuevaEnabled { get => _txtLocNuevaEnabled; set { _txtLocNuevaEnabled = value; } }
        private bool _txtLocAnteriorEnabled { get; set; } = true;
        public bool txtLocAnteriorEnabled { get => _txtLocAnteriorEnabled; set { _txtLocAnteriorEnabled = value; } }
        private bool _txtCodProdEnabled { get; set; } = false;
        public bool txtCodProdEnabled { get => _txtCodProdEnabled; set { _txtCodProdEnabled = value; } }

        private ObservableCollection<RazonND> _ListaND { get; set; }
        public ObservableCollection<RazonND> ListaND { get { return _ListaND; } set { _ListaND = value; } }
        private ObservableCollection<ReubicarProducto> tProducto { get; set; }
        private int _pNDIndex { get; set; }
        public int pNDIndex { get { return _pNDIndex; } set { _pNDIndex = value; } }
        private bool TrajoProducto { get; set; }
        private NuevoLoteModel _Guardar { get; set; }
        public NuevoLoteModel GuardarModel
        {
            get { return _Guardar; }
            set { _Guardar = value; }
        }
        private ObservableCollection<DetalleConteo> _listaFiltro { get; set; }
        public ObservableCollection<DetalleConteo> ListaFiltro
        {
            get { return _listaFiltro; }
            set
            {
                _listaFiltro = value;
            }
        }
        private bool _buscarVisible { get; set; }
        public bool BuscarVisible
        {
            get { return _buscarVisible; }
            set
            {
                _buscarVisible = value;
            }
        }

        private bool Montacargas = false;
        private string Destino = string.Empty;
        private string Origen = string.Empty;
        private bool LocalizacionValidada = false;
        private decimal? PrecioLista = 0;
        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (DetalleConteo)item;
            codprod = i.cod_prod.Trim();
            descripcion = i.descripcion_completa.Trim();
            _buscarVisible = false;
            TrajoProducto = true;

        });
        public ReubicarMercanciaVM(INavigation _navigation, ReubicarMercanciaView _view)
        {
            navigaton = _navigation;
            view = _view;
            MostrarMsg = new Mensajes();
            getNuevoFolio = new GetNuevoFolio();
            Task.Run(async () => await TraerRazones()).Wait();
            pNDIndex = 0;
            Cantidad = "0";
            txtCodProdCompleted = new Command(TraerProducto);
            GuardarCommand = new Command(Guardar);
            LimpiarCommand = new Command(Limpiar);
            txtLocAnteriorCompleted = new Command(LocAnteriorCompleted);
            ProdTextChangedCommand = new Command(BuscarProdTextChanged);
            txtLocNuevaCompleted = new Command(LocNuevaCompleted);
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            TrajoProducto = false;
            GuardarModel = new NuevoLoteModel();
            _buscarVisible = false;
            tProducto = new ObservableCollection<ReubicarProducto>();
        }

        private async void LocNuevaCompleted()
        {
            try
            {
                if(await ValidaLoc(false))
                {
                    bool Save = true;
                    Cantidad2 = Cantidad;
                    if(Destino.IndexOf("N",0) > -1 || Origen.IndexOf("N",0) > -1)
                    {
                        pRNDVisible = true;
                        lblRNDVisible = true;
                        PrecioLista = 0;
                    }
                    if ((Destino == "PD" && Origen == "PND") || (Destino == "LD" && Origen == "LND"))
                    {
                        lblSolicitudVisible = true;
                        txtSolicitudVisible = true;
                    }
                    if ((Destino == "PD" && Origen == "PND") || (Destino == "PD" && Origen == "PND"))
                    {
                        txtCant2Visible = true;
                        lblCantVisible = true;
                    }
                    else
                    {
                        if (Origen != Destino)
                        {
                            switch (Origen)
                            {
                                case "PD":
                                    if (Destino == "LD" || Destino == "LND")
                                    {
                                        if (!(await ValidaPallet()))
                                        {
                                            Save = false;
                                        }
                                    }
                                    break;
                                case "PND":
                                    if (Destino == "LD" || Destino == "LND")
                                    {
                                        if (!(await ValidaPallet()))
                                        {
                                            Save = false;
                                        }
                                    }
                                    break;
                            }
                        }
                        if (Save)
                        {
                            Guardar();
                        }
                    }
                }
                else
                {
                    LocalizacionNueva = "";
                    Entry entry = (Entry)view.FindByName("txtLocNueva");
                    entry.Focus();
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                return;
            }
        }

        private async Task<bool> ValidaPallet()
        {
            if(int.Parse(Cantidad.Trim()) != tProducto[0].prodxpall)
            {
                await MostrarMsg.ShowMessage("La cantidad que desea transferir no es equivalente a un pallet.");
                return false;
            }
            return true;
        }
        private async Task<bool> ValidaPicking()
        {
            if (int.Parse(Cantidad2.Trim()) < 1)
            {
                await MostrarMsg.ShowMessage("No existe cantidad a reubicar.");
                return false;
            }
            if(decimal.Parse(Cantidad2.Trim()) > decimal.Parse(Cantidad.Trim()))
            {
                await MostrarMsg.ShowMessage("La cantidad es mayor a la existente en la localizacion.");
                return false;
            }
            return true;
        }

        private async void LocAnteriorCompleted()
        {
            bool resulado = await ValidaLoc(true);

            if (resulado)
            {
                txtLocAnteriorEnabled = false;
                txtLocNuevaEnabled = true;
                Entry entry = (Entry)view.FindByName("txtLocNueva");
                entry.Focus();
            }
        }

        private async Task<bool> ValidaLoc(bool LocAnterior)
        {
            bool Band = false;
            try
            {
                string Loc = LocalizacionNueva.Trim();
                if (LocAnterior) { Loc = txtLocAnterior.Trim(); }

                Loc = ValidaDigitoVerificador(Loc.Trim(),false);
                if(Loc == "-1")
                {
                    await MostrarMsg.ShowMessage("Localización no válida.");
                    return false;
                }

                if (LocAnterior)
                {
                    txtLocAnterior = Loc.Trim();
                    if(lblLocalizacion.Trim() != "" && txtLocAnterior.Trim() != lblLocalizacion.Trim())
                    {                     
                        await MostrarMsg.ShowMessage("Esta localizacion no es la permitida para este producto.");
                        return false;                        
                    }
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("localizacion", Loc);
                    parametros.Add("cod_estab", General.EstabSession);
                    var url = "http://" + General.urlWS + "/api/Reubicar/ValidaLocProducto";
                    var resp = await client.Get<ObservableCollection<ReubicarProducto>>(url, parametros);

                    if (!resp.Ok)
                    {
                        tProducto = new ObservableCollection<ReubicarProducto>();
                        await MostrarMsg.ShowMessage("No existe mercancia en esta localización.");
                        return false;
                    }
                    else
                    {
                        var info = resp.Result;
                        tProducto = info;
                        if (tProducto.Count == 1)
                        {
                            Band = true;
                            LigarProducto(0);
                        }
                        else
                        {
                            txtLocAnteriorEnabled = false;
                            txtCodProdEnabled = true;
                            Entry entry = (Entry)view.FindByName("txtCodProd");
                            entry.Focus();
                        }
                    }
                }
                else
                {
                    if (Montacargas)
                    {
                        string[] locs = lblLocalizaciones.ToUpper().Split(',');
                        if (locs.Length > 0 && Array.IndexOf(locs, Loc.ToUpper()) == -1)
                        {
                            await MostrarMsg.ShowMessage("La localización no está dentro de las localizaciones disponibles");
                            Band = false;
                            return false;
                        }
                    }
                    LocalizacionNueva = Loc.Trim();
                    Band = true;
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("localizacion", Loc.Trim());
                    var url = "http://" + General.urlWS + "/api/Reubicar/NuevaLoc";
                    var resp = await client.Get<SqlNuevaLoc>(url, parametros);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage("La nueva localizacion no exite o ya esta ocupada.");
                        Band = false;
                        return false;
                    }
                    else
                    {
                        var info = resp.Result;
                        if (info.multiples_productos && info.productos > 0)
                        {
                            await MostrarMsg.ShowMessage("Esta localización ya esta ocupada y no permite multiples productos.");
                            Band = false;
                            return false;
                        }
                        if(lblIntLocalizacion.Trim() != "")
                        {
                            if(Loc.ToLower() != lblIntLocalizacion.Trim().ToLower())
                            {
                                await MostrarMsg.ShowMessage("Esta localizacion no es la permitida para este producto.");
                                Band = false;
                                return false;
                            }
                        }
                        if (info.mercancia_disponible)
                        {
                            Destino = "LD";
                            if (info.picking) { Destino = "PD"; }
                        }
                        else
                        {
                            Destino = "LND";
                            if (info.picking) { Destino = "PND"; }
                        }
                        LocalizacionValidada = Band;
                    }
                }
                return Band;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                return false;
            }

        }

        private async void LigarProducto(int Pos)
        {
            CodProd = tProducto[Pos].cod_prod.Trim();
            LoteFab = tProducto[Pos].lote.Trim();
            Descripcion = tProducto[Pos].descripcion.Trim();
            LoteRecep = tProducto[Pos].lote_recepcion.Trim();
            Cantidad = (tProducto[Pos].exist_unidades + tProducto[Pos].exist_piezas).ToString();
            if (tProducto[Pos].mercancia_disponible)
            {
                Origen = "LD";
                if (tProducto[Pos].picking) { Origen = "PD"; }
            }
            else
            {
                Origen = "LND";
                if (tProducto[Pos].picking) { Origen = "PND"; }
            }
            LocDisponibles();
            txtLocAnteriorEnabled = false;
            txtCodProdEnabled = false;
            txtLocNuevaEnabled = true;
            Entry entry = (Entry)view.FindByName("txtLocNueva");
            entry.Focus();
        }

        private async void LocDisponibles()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("cod_prod", CodProd.Trim());
                parametros.Add("cod_estab", General.EstabSession.Trim());
                parametros.Add("montacargas", Montacargas.ToString().Trim());
                var url = "http://" + General.urlWS + "/api/Reubicar/LocDisponibles";
                var resp = await client.Get<string>(url, parametros);

                if (!resp.Ok)
                {
                    return;
                }
                else
                {
                    var resultado = resp.Result;
                    if (!string.IsNullOrEmpty(resultado))
                    {
                        lblLocalizaciones = resultado.Trim();
                        lblLocDisVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private async Task TraerRazones()
        {
            try
            {
                RestClient client = new RestClient(null);
                var url = "http://" + General.urlWS + "/api/Reubicar/RazonesND";
                var resp = await client.Get<ObservableCollection<RazonND>>(url);

                if (!resp.Ok)
                {
                    ListaND = new ObservableCollection<RazonND>();
                }
                else
                {
                    var infoRecepcion = resp.Result;
                    ListaND = infoRecepcion;
                }
            }

            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private void Limpiar()
        {
            TrajoProducto = false;
            codprod = "";
            Descripcion = "";
            LoteFab = "";
            Cantidad = "0";
            Cantidad2 = "0";
            LocalizacionNueva = "";
            LoteRecep = "";
            _pNDIndex = 0;
            LocalizacionNueva = "";
            Origen = "LD";
            Destino = "LD";
            LoteFab = "";
            lblLocalizaciones = "";
            txtCodProdEnabled = false;
            txtLocAnteriorEnabled = true;
            txtLocNuevaEnabled = false;
            lblRNDVisible = false;
            pRNDVisible = false;
            lblSolicitudVisible = false;
            txtSolicitudVisible = false;
            _buscarVisible = false;
            pRNDVisible = false;
            lblLocDisVisible = false;
            LocalizacionValidada = false;
        }

        public async Task Focus()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Entry entry = (Entry)view.FindByName("txtLocAnterior");
                    entry.Focus();
                });
            });
        }

        public async void BuscarProdTextChanged()
        {
            try
            {
                if (string.IsNullOrEmpty(codprod.Trim()))
                    return;
                if (CodProd.Trim().Length < 3)
                {
                    _listaFiltro.Clear();
                    _buscarVisible = false;
                    return;
                }
                else
                {
                    _listaFiltro.Clear();
                    _buscarVisible = true;
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Codigo", "");
                    parametros.Add("Filtro", codprod.Trim());
                    parametros.Add("Estab", General.userCodEstab);
                    var url = "http://" + General.urlWS + "/api/Inventario/BuscarProductos";
                    var resp = await client.Get<ObservableCollection<DetalleConteo>>(url, parametros);

                    if (!resp.Ok)
                    {
                        return;
                    }
                    else
                    {
                        _listaFiltro = new ObservableCollection<DetalleConteo>(resp.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async void Guardar()
        {
            try
            {
                if (Origen == "PD" || Origen == "PND")
                {
                    if (!(await ValidaPicking())) { return; }
                }
                if (Destino == "PD" || Destino == "PND")
                {
                    if (!(await ValidaSolicitud())) { return; }
                }

                decimal cant = decimal.Parse(Cantidad2.Trim());
                bool OND = false, DND = false;
                if (Origen == "LND" || Origen == "PND") { OND = true; }
                if (Destino == "LND" || Destino == "PND") { OND = true; }
                string LocNva = LocalizacionNueva.Trim(), LocAnt = txtLocAnterior.Trim();
                if(LocNva == "-1" || LocAnt == "-1")
                {
                    await MostrarMsg.ShowMessage("Localizaciones no válidas.");
                    return;
                }
                string Tran = "";
                if (!(DND == OND))
                {
                    Tran = "179";
                    if (Origen.IndexOf("N", 0) > -1)
                    {
                        Tran = "522";
                        cant = cant * -1;
                    }

                    RestClient client2 = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("cod_prod", CodProd.Trim());
                    parametros.Add("unidad", "U");
                    parametros.Add("trans", Tran.Trim());
                    parametros.Add("cant", cant.ToString().Trim());
                    parametros.Add("cod_estab", General.EstabSession.Trim());
                    parametros.Add("razonND", ListaND[pNDIndex].razon.Trim());
                    parametros.Add("usuario", General.userCode.Trim());
                    var url2 = "http://" + General.urlWS + "/api/Reubicar/NoDisponible";
                    var resp2 = await client2.Post<string>(url2, parametros);

                    if (!resp2.Ok)
                    {
                        await MostrarMsg.ShowMessage(" No se pudo guardar la información.");
                        return;
                    }
                    else
                    {
                        string msg = resp2.Result;
                        if(msg != "") 
                        {
                            await MostrarMsg.ShowMessage(msg);
                            return;
                        }
                    }
                }
                ModificaInventarioLoc modificaInventarioLoc = new ModificaInventarioLoc();
                modificaInventarioLoc.CodProd = tProducto[0].cod_prod.Trim();
                modificaInventarioLoc.CodEstab = General.EstabSession.Trim();
                modificaInventarioLoc.Transaccion = "623";
                modificaInventarioLoc.Unidad = tProducto[0].unidad_compra.Trim();
                modificaInventarioLoc.Cantidad = cant * -1;
                modificaInventarioLoc.Guardar = true;
                modificaInventarioLoc.MSG = "";
                modificaInventarioLoc.Localizacion = txtLocAnterior.Trim();
                modificaInventarioLoc.AfectaNoDisponible = false;
                modificaInventarioLoc.Lote = LoteRecep.Trim();
                modificaInventarioLoc.FolioRef = tProducto[0].lote_recepcion.Trim();
                modificaInventarioLoc.TransRef = "622";
                modificaInventarioLoc.LoteRecepcion = tProducto[0].lote_recepcion.Trim();
                RestClient client = new RestClient(null);
                var url = "http://" + General.urlWS + "/api/Inventario/ModificaInventarioLoc";
                var resp = await client.Post<int>(url, null, modificaInventarioLoc);
                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(" No se pudo guardar la información.");
                    return;
                }
                modificaInventarioLoc.TransRef = "623";
                modificaInventarioLoc.Transaccion = "622";
                modificaInventarioLoc.Cantidad = cant;
                modificaInventarioLoc.Localizacion = LocalizacionNueva;
                resp = await client.Post<int>(url, null, modificaInventarioLoc);
                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(" No se pudo guardar la información.");
                    return;
                }

                //******EN EL CÓDIGO ORIGINAL SE ENCUENTRA ESTA PARTE PERO NUNCA ENTRA PORQUE LA VARIABLE SURTIDO SIEMPRE ES IGUAL A FALSE ****
                //    If Me.Surtido Then 
                //    Dim SqlC As New SqlClient.SqlCommand("Select isnull((select top 1 folio from solicitudes_surtido_pick_slot where cod_prod = '" & cod_prod & "' and status = 'V' and fecha_surtido is null order by fecha ),'') ", Me.SqlConnection, MG.MyTrans)
                //    Dim Sol As String = SqlC.ExecuteScalar
                //    If Sol.Trim<> "" Then
                //        SqlC.CommandText = "set dateformat dmy; update solicitudes_surtido_pick_slot set fecha_surtido = GETDATE(), usuario_surtido = '" & MG.UserCode & "', localizacion = '" & LocAnt & "', folio_referencia = '" & lote_recep & "' where folio = '" & Sol & "'"
                //        SqlC.ExecuteNonQuery()
                //    End If
                //End If
                await MostrarMsg.ShowMessage("Intercambio realizado con exito.");
                Limpiar();

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());

                throw;
            }
        }

       private async Task<bool> ValidaSolicitud()
        {
            try
            {
                if(txtSolicitudVisible == false) { return true; }
                string solicitud = "";
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("CodProd", CodProd.Trim());
                parametros.Add("Solicitud", LoteRecep.Trim());
                var url = "http://" + General.urlWS + "/api/Reubicar/ValidaSolicitud";
                var resp = await client.Get<string>(url, parametros);

                if (!resp.Ok)
                {
                    return false;
                }
                else
                {
                    var resultado = resp.Result;
                    if (string.IsNullOrEmpty(resultado))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                return false;
            }
        }
        private async void TraerProducto()
        {
            try
            {
                if (CodProd.Trim() == "") { await MostrarMsg.ShowMessage("Ingrese un código de producto."); TrajoProducto = false; Entry entry = (Entry)view.FindByName("txtCodProd"); entry.Focus(); return; }

                Descripcion = "";
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Codigo", CodProd.Trim());
                parametros.Add("Filtro", "");
                parametros.Add("Estab", General.EstabSession);
                var url = "http://" + General.urlWS + "/api/Inventario/BuscarProductos";
                var resp = await client.Get<List<Productos>>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " No se encontró el producto.");
                    TrajoProducto = false;
                    return;
                }
                else
                {
                    CodProd = resp.Result.Select(x => x.cod_prod.Trim()).FirstOrDefault();

                    var index = tProducto
                                .Select((item, i) => new { Item = item, Index = i })
                                .FirstOrDefault(x => x.Item.cod_prod == CodProd)?.Index ?? -1;

                    if (index == -1)
                    {
                        await MostrarMsg.ShowMessage("Producto no válido");
                        TrajoProducto = false;
                        CodProd = "";
                        Entry entry = (Entry)view.FindByName("txtCodProd");
                        entry.Focus();
                        return;
                    }

                    LigarProducto(index);

                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                TrajoProducto = false;
                throw;
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
}
