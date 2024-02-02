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
    public class EntradaVM
    {
        #region Variables

        public INavigation navigaton { get; set; }
        public EntradaView view { get; set; }
        public Mensajes MostrarMsg { get; set; }
        private Producto producto { get; set; }
        public AIModel aIModel { get; set; }

        public GetNuevoFolio GetNuevoFolio { get; set; }
        //public CheckCameraPermissions chkCamara { get; set; }

        private bool _enableControls { get; set; }
        private bool _chkEntradaOC { get; set; }
        private bool _chkEntrada { get; set;  }
        private string _proveedor { get; set; }
        private string _nombreProveedor { get; set; }

        private ObservableCollection<UnidadesModel> _listaUnidades { get; set; }
        public ObservableCollection<UnidadesModel> ListaUnidades
        {
            get { return _listaUnidades; }
            set { _listaUnidades = value; }
        }
        private UnidadesModel _unidadSelected { get; set; }
        public UnidadesModel UnidadSelected
        {
            get { return _unidadSelected; }
            set { _unidadSelected = value; }
        }

        public bool EnableControls
        {
            get { return _enableControls; }
            set { _enableControls = value; }
        }
        public bool ChkEntradaOC
        {
            get { return _chkEntradaOC; }
            set { _chkEntradaOC = value; }
        }
        public bool ChkEntrada
        {
            get { return _chkEntrada; }
            set { _chkEntrada = value; }
        }

        public string Proveedor
        {
            get { return _proveedor; }
            set { _proveedor = value; }
        }
        public string NombreProveedor
        {
            get { return _nombreProveedor; }
            set { _nombreProveedor = value; }
        }
        private int _pUnidadIndex { get; set; }
        public int pUnidadIndex
        {
            get { return _pUnidadIndex; }
            set { _pUnidadIndex = value; }
        }
        private int cantidad;
        public int Cantidad { get => cantidad; set { cantidad = value; } }

        private string nombreProducto;
        public string NombreProducto { get => nombreProducto; set { nombreProducto = value; } }
        private ObservableCollection<OrdenCompraDetalleModel> OrdenCompraDetalle { get; set; }
        private OrdenCompraModel OrdenCompra { get; set; }

        private Entrada _entrada { get; set; }
        public Entrada Entrada { get => _entrada; set { _entrada = value; } }
        private bool habilitaPallet { get; set; }
        public bool HabilitaPallet { get => habilitaPallet; set { habilitaPallet = value; } }

        private decimal uc;
        public decimal UC
        {
            get { return uc; }
            set { uc = value; }
        }

        private decimal ua;
        public decimal UA
        {
            get { return ua; }
            set { ua = value; }
        }

        private int pl;
        public int PL
        {
            get { return pl; }
            set { pl = value; }
        }

        private decimal pe;
        public decimal PE
        {
            get { return pe; }
            set { pe = value; }
        }

        private decimal vo;
        public decimal VO
        {
            get { return vo; }
            set { vo = value; }
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
        private ObservableCollection<proveedore> _listaProveedores { get; set; }
        public ObservableCollection<proveedore> ListaProveedores
        {
            get { return _listaProveedores; }
            set
            {
                _listaProveedores = value;
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
        private EntradaDetalle _selectedProd { get; set; }
        public EntradaDetalle SelectedProd { get => _selectedProd; set { _selectedProd = value; } }
        private proveedore _datosPrv { get; set; }
        public proveedore DatosPrv { get => _datosPrv; set { _datosPrv = value; } }


        public Command CompletedOCCommand { get; set; }
        public Command CompletedProveedorCommand { get; set; }
        public Command txtCodProdCompleted { get ; set; }
        public Command AgregarProducto { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command EscanerCommand { get; set; }
        public Command ProdTextChangedCommand { get; set; }
        public Command ProveedorTextChangedCommand { get; set; }
        public Command TipoEntradaCheckCommand { get; set; }
        public Command TipoEntradaOCCheckCommand { get; set; }
        public Command TxtOCChanged { get; set; }
        public Command chkPalletChanged { get; set; }
        

        #endregion


        public EntradaVM(INavigation _navigaton, EntradaView _view)
        {
            view = _view;
            navigaton = _navigaton;
            MostrarMsg = new Mensajes();
            aIModel = new AIModel();
            aIModel.IsBusy = false;
            ChkEntradaOC = true;
            Proveedor = "";
            NombreProveedor = "";
            pUnidadIndex = -1;
            Cantidad = 0;
            NombreProducto = "";
            _buscarVisible = false;
            EnableControls = true;
            HabilitaPallet = true;
            DatePicker dtpCaducidad = (DatePicker)this.view.FindByName("dpCaducidad");
            DateTime fecha2078 = new DateTime(2078, 12, 31);
            dtpCaducidad.Date = fecha2078;
            producto = new Producto();
            ListaUnidades = new ObservableCollection<UnidadesModel>();
            UnidadSelected = new UnidadesModel();
            OrdenCompraDetalle = new ObservableCollection<OrdenCompraDetalleModel>();
            Entrada = new Entrada();
            OrdenCompra = new OrdenCompraModel();
            SelectedProd = new EntradaDetalle();
            DatosPrv = new proveedore();
            CompletedOCCommand = new Command(TraerOC);
            txtCodProdCompleted = new Command(TraerProducto);
            AgregarProducto = new Command(AddProducto);
            LimpiarCommand = new Command(Limpia);
            GuardarCommand = new Command(GuardarEntrada);
            EscanerCommand = new Command(AbrirEscaner);
            ProdTextChangedCommand = new Command(BuscarProdTextChanged);
            ProveedorTextChangedCommand = new Command(BuscarProveedorTextChanged);
            GetNuevoFolio = new GetNuevoFolio();
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            _listaProveedores = new ObservableCollection<proveedore>();
            TipoEntradaCheckCommand = new Command(CheckEntradaOC);
            CompletedProveedorCommand = new Command(TraerProveedor);
            TxtOCChanged = new Command(TextOCChanged);
            chkPalletChanged = new Command(ChkPalletChanged);


        }

        private void ChkPalletChanged()
        {
            if (producto is null)
                return;

            CheckBox chkPallet = (CheckBox)this.view.FindByName("chPallet");
            Stepper nudPallets = (Stepper)this.view.FindByName("nudPallets");
            Entry txtLote = (Entry)this.view.FindByName("txtLote");

            if (chkPallet != null)
            {
                pUnidadIndex = 0;
                Cantidad = 1;
                nudPallets.Value = 1;
                txtLote.Text = "";
                if (chkPallet.IsChecked)
                {
                    HabilitaPallet = false;
                    Cantidad = producto.caducidad_fabricacion; //Se usa esta variable ya que no tiene otro uso y para no modificar el modelo, actualizar a futuro con el campo correspondiente.
                }
                else
                {
                    HabilitaPallet = true;
                }
            }
        }

        private void TextOCChanged()
        {
            LimpiarGeneral(true);
        }

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            if (await MostrarMsg.ShowQuestionMsg("¿Desea eliminar el producto de la lista"))
            {
                var elemento = (EntradaDetalle)item;
                Entrada.entradaDetalles.Remove(elemento);
                Totales();
            }
});

        private async void Limpia()
        {
            if(await MostrarMsg.ShowQuestionMsg("¿Desea limpiar toda la información?"))
            {
                LimpiarGeneral();
            }

            
        }

        private void LimpiarGeneral(bool OC = false,bool Libre = false)
        {
            //LimpiarPantalla();
            //_folio = "";
            Entrada = new Entrada();
            Cantidad = 0;
            ListaUnidades.Clear();
            NombreProducto = "";
            pUnidadIndex = -1;
            if(!Libre)
                Proveedor = "";
            NombreProveedor = "";
            DatePicker dtpCaducidad = (DatePicker)this.view.FindByName("dpCaducidad");
            Stepper nudPallets = (Stepper)this.view.FindByName("nudPallets");
            Entry txtLote = (Entry)this.view.FindByName("txtLote");
            Editor txtNotas = (Editor)this.view.FindByName("txtNotasProducto");
            CheckBox chkPallet = (CheckBox)this.view.FindByName("chPallet");
            Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");
            Entry txtOC = (Entry)this.view.FindByName("txtOrdenCompra");
            nudPallets.Value = 1;
            DateTime fecha2078 = new DateTime(2078, 12, 31);
            dtpCaducidad.Date = fecha2078;
            txtLote.Text = "";
            txtNotas.Text = "";
            txtCodProd.Text = "";
            if(!OC)
                txtOC.Text = "";
            txtOC.Focus();
            Totales();
            view.CurrentPage = view.Children[0];
        }

        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (DetalleConteo)item;
            Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");
            txtCodProd.Text = i.cod_prod;
            _buscarVisible = false;
            txtCodProd.Focus();

        });
        public ICommand ProveedorTappedCommand => new Command(async (proveedor) =>
        {
            var i = (proveedore)proveedor;
            Proveedor = i.cod_prv;
            NombreProveedor = i.razon_social;
            _buscarVisible = false;
        });
        public async void BuscarProdTextChanged()
        {
            try
            {
 
                DatePicker dtpCaducidad = (DatePicker)this.view.FindByName("dpCaducidad");
                Stepper nudPallets = (Stepper)this.view.FindByName("nudPallets");
                Entry txtLote = (Entry)this.view.FindByName("txtLote");
                Editor txtNotas = (Editor)this.view.FindByName("txtNotasProducto");
                CheckBox chkPallet = (CheckBox)this.view.FindByName("chPallet");
                Limpiar();
                nudPallets.Value = 1;
                dtpCaducidad.Date = DateTime.Now;
                txtLote.Text = "";
                txtNotas.Text = "";

                Entry _producto = (Entry)this.view.FindByName("txtCodProd");
                if (string.IsNullOrEmpty(_producto.Text.Trim()))
                    return;
                if (_producto.Text.Trim().Length < 3)
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
                    parametros.Add("Filtro", _producto.Text.Trim());
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
        public async void BuscarProveedorTextChanged()
        {
            try
            {
                Entrada = new Entrada();
                Totales();
                if (ChkEntradaOC)
                    return; _buscarVisible = false;
                Entry txtCodPrv = (Entry)this.view.FindByName("txtProveedor");              
                NombreProveedor = "";
                
                if (string.IsNullOrEmpty(txtCodPrv.Text.Trim()))
                    return;
                if (txtCodPrv.Text.Trim().Length < 3)
                {
                    _listaProveedores.Clear();
                    _buscarVisible = false;
                    return;
                }
                else
                {
                    _listaProveedores.Clear();
                    _buscarVisible = true;
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("CodPrv", txtCodPrv.Text.Trim());
                    var url = "http://" + General.urlWS + "/api/Entrada/BuscarProveedores";
                    var resp = await client.Get<ObservableCollection<proveedore>>(url, parametros);

                    if (!resp.Ok)
                    {
                        return;
                    }
                    else
                    {
                        _listaProveedores = new ObservableCollection<proveedore>(resp.Result);
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

        private async void AbrirEscaner()
        {
            //    try
            //    {

            //        if (string.IsNullOrEmpty(_recepcion.folio))
            //        {
            //            await MostrarMsg.ShowMessage("No puede escanear productos sin un folio de transferencia.");
            //            return;
            //        }
            //        else
            //        {
            //            var status = await chkCamara.CheckCameraStatus();

            //            if (status)
            //            {
            //                var options = new MobileBarcodeScanningOptions();
            //                options.PossibleFormats = new List<BarcodeFormat>
            //                {
            //                    BarcodeFormat.QR_CODE,
            //                    BarcodeFormat.CODE_128,
            //                    BarcodeFormat.EAN_13,
            //                    BarcodeFormat.CODE_39,
            //                    BarcodeFormat.CODE_93,
            //                    BarcodeFormat.UPC_A,
            //                    BarcodeFormat.UPC_E
            //            };

            //                var page = new ZXingScannerPage(options) { Title = "Escaner" };
            //                var closeItem = new ToolbarItem { Text = "Cerrar" };
            //                closeItem.Clicked += (object sender1, EventArgs e1) =>
            //                {
            //                    page.IsScanning = false;
            //                    Device.BeginInvokeOnMainThread(() =>
            //                    {
            //                        Application.Current.MainPage.Navigation.PopModalAsync();
            //                    });
            //                };
            //                page.ToolbarItems.Add(closeItem);
            //                page.OnScanResult += (result) =>
            //                {
            //                    page.IsScanning = false;

            //                    Device.BeginInvokeOnMainThread(async () =>
            //                    {
            //                        await Application.Current.MainPage.Navigation.PopModalAsync();
            //                        if (string.IsNullOrEmpty(result.Text))
            //                        {
            //                            await MostrarMsg.ShowMessage("El código no es valido.");
            //                            return;
            //                        }
            //                        else
            //                        {
            //                            _productoTransf.codigoEscaneado = result.Text.Trim();
            //                            ProductoTransferencia();

            //                        }
            //                    });
            //                };
            //                await navigaton.PushModalAsync(new NavigationPage(page) { BarTextColor = Color.White, BarBackgroundColor = Color.CadetBlue }, true);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        await MostrarMsg.ShowMessage(ex.Message);
            //        Console.WriteLine("Error: " + ex.Message.ToString());
            //        throw;
            //    }
            //}
             }
        private async void GuardarEntrada()
        {
            try
            {
                var FolioEntrada = "";
                Entry txtOrden = (Entry)this.view.FindByName("txtOrdenCompra");
                Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");


                if (string.IsNullOrEmpty(txtOrden.Text) && ChkEntradaOC)
                {
                    await MostrarMsg.ShowMessage("Ingrese una orden de compra.");
                    txtOrden.Focus();
                    return;
                }
                else if (Entrada.entradaDetalles.Count <= 0)
                {
                    await MostrarMsg.ShowMessage("No ha agregado productos.");
                    txtCodProd.Focus();
                    return;
                }
                else
                {                
                    if (!await MostrarMsg.ShowQuestionMsg("¿Desea guardar la entrada de mercancia?"))
                        return;
                    else
                    {
                         FolioEntrada = await GetNuevoFolio.GenerarFolio("56", General.EstabSession);
                    }

                    Entrada.OC = (ChkEntradaOC) ? txtOrden.Text : "";
                    Entrada.Proveedor = Proveedor;
                    Entrada.Usuario = General.userCode;
                    Entrada.Estab = General.EstabSession;
                    Entrada.IdMeos = 0;
                    Entrada.IdMped = 0;
                    Entrada.Folio = FolioEntrada;

                    RestClient client = new RestClient(null);
                    var url = "http://" + General.urlWS + "/api/Entrada/GuardarEntrada";
                    var resp = await client.Post<Entrada>(url, null, Entrada);

                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage("No se pudo guardar la información");
                        return;
                    }
                    else
                    {
                        await MostrarMsg.ShowMessage("Folio: " + FolioEntrada.Trim() + "\r\n" +
                            "La información se guardó correctamente.");
                            LimpiarGeneral();
                        view.CurrentPage = view.Children[0];
                    }


                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        public async Task Focus()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Entry entry = (Entry)view.FindByName("txtOrdenCompra");
                    entry.Focus();
                });
            });
        }

        private async void AddProducto()
        {
            Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");
            DatePicker dtpCaducidad = (DatePicker)this.view.FindByName("dpCaducidad");
            Stepper nudPallets = (Stepper)this.view.FindByName("nudPallets");
            Entry txtLote = (Entry)this.view.FindByName("txtLote");
            Editor txtNotas = (Editor)this.view.FindByName("txtNotasProducto");
            CheckBox chkPallet = (CheckBox)this.view.FindByName("chPallet");
           

            if (producto == null || string.IsNullOrEmpty(producto.cod_prod))
                {

                    await MostrarMsg.ShowMessage("Ingrese un código de producto válido.");
                    return;
                }
            

            if (Cantidad <= 0)
            {
                await MostrarMsg.ShowMessage("Ingrese una cántidad mayor a 0.");
                return;
            }

            if (chkPallet.IsChecked)
            {
                if (producto.cama == 0 || producto.altura == 0)
                {
                    await MostrarMsg.ShowMessage("No puede agregar pallets a este producto ya que no se ha configurado correctamente la cama y la altura.");
                    return;
                }
            }

            var Contenido = producto.contenido;

            var fact = (UnidadSelected.Unidad == "P") ? Contenido : 1;

            decimal Cant = (decimal)(Cantidad * nudPallets.Value);

            if (!await ValidarProducto(Cant))
            {
                return;
            }


            if (ChkEntradaOC)
            {
                if (!await ValidarCantidadOC(producto.cod_prod, UnidadSelected.Unidad, Cant))
                    return;
               
            }


            EntradaDetalle var = new EntradaDetalle
            {
                cod_prod = producto.cod_prod,
                descripcion = NombreProducto,
                cantidad = (int)Cant,
                unidad = UnidadSelected.Unidad, // Reemplaza con el valor correcto
                peso = producto.peso_total * Cant * fact,
                volumen = (decimal)producto.volumen * Cant * fact,
                abreviatura_unidad = UnidadSelected.Abreviatura,
                notas = string.IsNullOrEmpty(txtNotas.Text) ? "" : txtNotas.Text ,
                fecha_caducidad = dtpCaducidad.Date,
                pallet = chkPallet.IsChecked,
                lote_fab = string.IsNullOrEmpty(txtLote.Text) ? "" : txtLote.Text,
                guardado = false,
                guardando = true,
                id_meos = 0,
                id_mped = 0,
                _fecha_caducidad = dtpCaducidad.Date.ToShortDateString()

            };

            Entrada.entradaDetalles.Add(var);
            Limpiar();
            nudPallets.Value = 1;
            dtpCaducidad.Date = DateTime.Now;
            txtLote.Text = "";
            txtNotas.Text = "";
            txtCodProd.Text = "";
            Totales();
            txtCodProd.Focus();



        }

        private async Task<bool> ValidarCantidadOC(string Cod_prod, string Unidad, decimal Cantidad)
        {
            try
            {
                //decimal CantidadPendiente = OrdenCompraDetalle
                //                 .Where(detalle => detalle.cod_prod == Cod_prod && detalle.unidad == Unidad)
                //                 .Sum(detalle => detalle.cantidad_autorizada - detalle.cantidad_surtida);

                decimal CantidaSurtir = Entrada.entradaDetalles
                                 .Where(detalle => detalle.cod_prod == Cod_prod && (string)detalle.unidad == Unidad)
                                 .Sum(detalle => detalle.cantidad) + Cantidad;

                //if (CantidadPendiente != 0)
                //{
                    if( CantidaSurtir > Cantidad)
                    {
                    return await ValidarProducto(CantidaSurtir);
                    }
                //}
                return true;
            }
            catch (Exception)
            {
                return false;   
            }
        }
        private async void TraerProducto()
        {
            try
            {
                BuscarVisible = false;
                if (string.IsNullOrEmpty(Proveedor))
                {
                    await MostrarMsg.ShowMessage("Alimente un proveedor para continuar.");
                    return;
                }
                Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");
                if (txtCodProd != null)
                {
                    if (string.IsNullOrEmpty(txtCodProd.Text))
                    {
                        await MostrarMsg.ShowMessage("Ingrese un código de producto válido.");
                        return;
                    }
                }
                else
                    return;

                CheckBox chkPallet = (CheckBox)this.view.FindByName("chPallet");
                chkPallet.IsChecked = false;

                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                var url = "http://" + General.urlWS + "/api/Entrada/TraerProducto";
                //Cod_prv,string Cod_estab,string Cod_prod
                parametros.Add("Cod_prv", Proveedor.Trim());
                parametros.Add("Cod_estab", General.EstabSession);
                parametros.Add("Cod_prod", txtCodProd.Text.Trim());

                var resp = await client.Get<Producto>(url, parametros);
                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("Código de producto inexistente o sin estatus válido.");                
                    return;
                }

                var contenido = resp.Result;
                producto = resp.Result;
                ListaUnidades.Clear();
                ListaUnidades.Add(new UnidadesModel { Abreviatura = contenido.abrevUC, Unidad = "U" });
                ListaUnidades.Add(new UnidadesModel { Abreviatura = contenido.abrevUA, Unidad = "P" });
                pUnidadIndex = 0;
                NombreProducto = contenido.descripcion_completa.ToString();
                Cantidad = 1;

            }
            catch (Exception)
            {

                throw;
            }
        }

    private void Limpiar()
        {
            Cantidad = 0;
            ListaUnidades.Clear();
            NombreProducto = "";
            pUnidadIndex = -1;
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            _listaProveedores = new ObservableCollection<proveedore>();
        }

        private void Totales()
        {
            VO = Entrada.VO;
            UC = Entrada.UC;
            UA = Entrada.UA;
            PL = Entrada.PL;
            PE = Entrada.PE;
        }

        private async Task<bool> ValidarProducto(decimal Cant)
        {
            RestClient client = new RestClient(null);
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            Entry txtOrden = (Entry)this.view.FindByName("txtOrdenCompra");
            CheckBox chkPallet = (CheckBox)this.view.FindByName("chPallet");
            DatePicker dtpCaducidad = (DatePicker)this.view.FindByName("dpCaducidad");
            var OC= "";
            if (ChkEntradaOC)
                OC = txtOrden.Text.Trim();

          

            parametros.Add("Cod_prod", producto.cod_prod.Trim());
            parametros.Add("Cod_estab", General.EstabSession);
            parametros.Add("OC", OC);
            parametros.Add("Caducidad", dtpCaducidad.Date.ToString());
            parametros.Add("EsLote", chkPallet.IsChecked.ToString());
            parametros.Add("Cant", Cant.ToString());
            parametros.Add("unidad", UnidadSelected.Unidad);
           
            var url = "http://" + General.urlWS + "/api/Entrada/ValidarProducto";
            var resp = await client.Get<string>(url, parametros);
            var mensaje = resp.Result;

            if (!string.IsNullOrEmpty(mensaje))
            {
                if (mensaje.StartsWith("@"))
                {
                    return await MostrarMsg.ShowQuestionMsg(mensaje + " ¿Desea continuar y agregar el producto?");
                }
                else
                {
                    await MostrarMsg.ShowMessage(mensaje);
                    return false;
                }                            
            }

            return true;
        }

        private void CheckEntradaOC()
        {
            LimpiarGeneral();
            Entry txtOrden = (Entry)this.view.FindByName("txtOrdenCompra");
            Entry txtPrv = (Entry)this.view.FindByName("txtProveedor");
            if(ChkEntradaOC)
                txtOrden.Focus();
            else
                txtPrv.Focus();
        }

        private async void TraerProveedor()
        {
            try
            {
                DatosPrv = new proveedore();
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
               
                if (string.IsNullOrEmpty(Proveedor))
                {
                    await MostrarMsg.ShowMessage("Ingrese un folio de orden de compra.");
                    return;
                }
                parametros.Add("Prv", Proveedor);              
                var url = "http://" + General.urlWS + "/api/Entrada/TraerProveedor";
                var resp = await client.Get<proveedore>(url, parametros);
                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " Proveedor no válido.");
                    return;
                }
                else
                {

                    DatosPrv = resp.Result;
                    Proveedor = DatosPrv.cod_prv.Trim();
                    NombreProveedor = DatosPrv.razon_social.Trim();
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
        Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }


}
        private async void TraerOC()
        {
            try
            {
                OrdenCompra = new OrdenCompraModel();
                OrdenCompraDetalle = new ObservableCollection<OrdenCompraDetalleModel>();

                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                Entry txtOrden = (Entry)this.view.FindByName("txtOrdenCompra");
                if(string.IsNullOrEmpty(txtOrden.Text))
                {
                    await MostrarMsg.ShowMessage("Ingrese un folio de orden de compra.");
                    return;
                }
                parametros.Add("Folio", txtOrden.Text.Trim());
                parametros.Add("Transaccion", "30");
                parametros.Add("Estab", General.EstabSession);
                parametros.Add("Usuario", General.userCode);
                var url = "http://" + General.urlWS + "/api/Entrada/TraerOrdenCompra";
                    var resp = await client.Get<EntradaModel>(url, parametros);
                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " Orden de compra no válida o no tiene productos para su establecimiento.");
                        return;
                    }
                    else
                    {

                    var OC = resp.Result.cabecero;
                    var Productos = resp.Result.listaDetalle;
                    if (OC.status.Trim().ToUpper() == "C")
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " Esta orden de compra está cancelada.");
                        return;
                    }
                    if (OC.status.Trim().ToUpper() == "V")
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " Esta orden de compras no está autorizada.");
                        return;
                    }
                    if (OC.back_order == false)
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " Esta orden de compra no está en back order.");
                        return;
                    }
                    if (OC.fecha_expiracion < DateTime.Today)
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " Esta orden de compra ya está expirada.");
                        return;
                    }
                    if (OC.comprador.Trim().ToUpper() == "OCNV")
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " Esta orden de compra es no valorizada, use la opción libre para recibirla.");
                        return;
                    }
                    if (OC.mensaje.Trim().ToUpper() != "")
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " " + OC.mensaje.Trim());
                        return;
                    }

                    Proveedor = OC.cod_prv.Trim();
                    NombreProveedor = OC.razon_social.Trim();
                    OrdenCompra = resp.Result.cabecero;
                    OrdenCompraDetalle = resp.Result.listaDetalle;

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
