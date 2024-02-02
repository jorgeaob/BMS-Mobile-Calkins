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
    public class InventarioVM
    {
        #region Variables

        public INavigation Navigation { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public ColorsModel colorModel { get; set; }
        public ColorsModel colorProdEntry { get; set; }

        private InventarioModel _invModel { get; set; }
        public InventarioModel InvModel
        {
            get { return _invModel; }
            set { _invModel = value; }
        }

        private ObservableCollection<DetalleConteo> _listProds { get; set; }
        public ObservableCollection<DetalleConteo> ListProds
        {
            get { return _listProds; }
            set 
            {
                if(_listProds != value)
                {
                    _listProds = value;
                }                
            }
        }

        private bool _enableControls { get; set; }
        public bool EnableControls
        {
            get { return _enableControls; }
            set { _enableControls = value; }
        }

        private DetalleConteo _producto { get; set; }
        public DetalleConteo Producto
        {
            get { return _producto; }
            set
            {
                _producto = value;
            }
        }
        private FocusTriggerAction _entryCodProd { get; set; }
        public FocusTriggerAction EntryCodProd
        {
            get { return _entryCodProd; }
            set
            {
                _entryCodProd = value;
            }
        }

        private bool _chkProdAdic { get; set; }
        public bool chkProdAdic
        {
            get { return _chkProdAdic; }
            set { _chkProdAdic = value; }
        }

        private ObservableCollection<DetalleConteo> _listDifProds { get; set; }
        public ObservableCollection<DetalleConteo> ListDifProds
        {
            get { return _listDifProds; }
            set
            {
                if (_listDifProds != value)
                {
                    _listDifProds = value;
                }
            }
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

        private DateTime _fechaConteo { get; set; }
        public DateTime FechaConteo
        {
            get { return _fechaConteo; }
            set { _fechaConteo = value; }
        }

        private decimal inventarioUCFecha { get; set; }
        private decimal inventarioUAFecha { get; set; }

        public CultureInfo _cultureInfo { get; set; }
        public DateTimeFormatInfo _formatInfo { get; set; }

        #endregion

        public Command CompletedFolioCommand { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command ProdCompletedCommand { get; set; }
        public Command UCValueChangedCommand { get; set; }
        public Command AddProdCommand { get; set; }
        //public Command BuscarProdCommand { get; set; }
        public Command ScannerProductoCommand { get; set; }
        public Command ProdTextChangedCommand { get; set; }
        public Command AbrirNotasProductoCommand { get; set; }
       
        private DetalleConteo _difSelected { get; set; }
        public DetalleConteo DifSelected
        {
            get { return _difSelected; }
            set
            {
                _difSelected = value;
            }
        }

        InventarioView _view { get; set; }

        //////CONSTRUCTOR/////
        public InventarioVM(INavigation navigation, InventarioView view)
        {            
            Navigation = navigation;
            _view = view;
            MostrarMsg = new Mensajes();
            colorModel = new ColorsModel();
            colorProdEntry = new ColorsModel();

            _invModel = new InventarioModel();
            _listProds = new ObservableCollection<DetalleConteo>();
            _enableControls = false;
            _producto = new DetalleConteo();
            _entryCodProd = new FocusTriggerAction();
            _entryCodProd.Focused = false;
            _chkProdAdic = false;
            _listDifProds = new ObservableCollection<DetalleConteo>();
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            _buscarVisible = false;
            _difSelected = new DetalleConteo();
            _fechaConteo = DateTime.Now;
            inventarioUCFecha = 0m;
            inventarioUAFecha = 0m;

            _cultureInfo = new CultureInfo("es-MX");
            _formatInfo = new DateTimeFormatInfo();
            _formatInfo = _cultureInfo.DateTimeFormat;

            colorModel.MissingColor = Color.FromRgb(248,248,255);
            colorProdEntry.MissingColor = Color.FromRgb(248,248,255);

            CompletedFolioCommand = new Command(InformacionConteo);
            LimpiarCommand = new Command(Limpiar);
            ProdCompletedCommand = new Command(BuscarProducto);
            UCValueChangedCommand = new Command(ConteoUC);
            AddProdCommand = new Command(AgregarGuardarCantidades);
            //BuscarProdCommand = new Command(OpenBuscardorProds);
            ScannerProductoCommand = new Command(AbrirEscaner);
            ProdTextChangedCommand = new Command(BuscarProdTextChanged);
            AbrirNotasProductoCommand = new Command(OpenNotasProducto);

            MessagingCenter.Subscribe<string>(this, "NotasGuardadas", (item) =>
            {
                string notaOk = item.Trim();

                if(notaOk.Trim() == "Ok")
                {
                    _listDifProds.Clear();
                    _listProds.Clear();
                    InformacionConteo();
                }
            });
        }

        private async Task<DateTime> ConsultarFechaServidor()
        {
            try
            {
                RestClient client = new RestClient(null);
                var url = "http://" + General.urlWS + "/api/Inventario/FechaHoraServer";
                var resp = await client.Get<DateTime>(url);

                if (!resp.Ok)
                {
                    return DateTime.Now;
                }
                else
                {
                    return resp.Result;
                }

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async void IniciarConteo()
        {
            if (!string.IsNullOrEmpty(_producto.cod_prod.Trim()))
            {
                do
                {
                    if (_producto.unidades_compra > 0 || _producto.unidades_alternativas > 0 || _producto.contado == true)
                    {
                        var resp = await MostrarMsg.ShowQuestionMsg("El producto " + _producto.cod_prod.Trim() + " ya fue contado, ¿Desea actualizar la cantidad?");
                        if (!resp)
                        {
                            return;
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
                while (false);

                _enableControls = true;
               

                if (_producto.unidades_compra <= 0 && _producto.unidades_alternativas <= 0)
                {
                    var fechaServer = await ConsultarFechaServidor();
                    _producto.fecha = Convert.ToDateTime(fechaServer.ToString("dd/MM/yyyy HH:mm", _formatInfo));
                    _fechaConteo = Convert.ToDateTime(_producto.fecha.ToString("dd/MM/yyyy HH:mm", _formatInfo));
                }
                else
                    _fechaConteo = Convert.ToDateTime(_producto.fecha.ToString("dd/MM/yyyy HH:mm", _formatInfo));

                await InventarioInicial(_producto.cod_prod.Trim(), _fechaConteo);
            }
            else
            {
                await MostrarMsg.ShowMessage("No ha seleccionado algún producto.");
                _enableControls = false;
            }
        }

        private async Task InventarioInicial(string CodProd, DateTime Fecha)
        {
            RestClient client = new RestClient(null);
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("CodProd", CodProd);
            parametros.Add("CodEstab", General.EstabSession);
            parametros.Add("Fecha", Fecha.ToString("dd/MM/yyyy HH:mm"));
            var url = "http://" + General.urlWS + "/api/Inventario/InventarioInicial";
            var resp = await client.Get<InventarioProductoFechaModel>(url, parametros);

            if (!resp.Ok)
            {
                //await MostrarMsg.ShowMessage("Error al mostrar inventario inicial. " + resp.Message);
                return;
            }
            else
            {
                inventarioUCFecha = resp.Result.uc;
                inventarioUAFecha = resp.Result.ua;
            }

            //HttpClient client = new HttpClient();            
            //string RestUrl = "http://" + General.urlWS + "/api/Inventario?CodProd="+ CodProd +"&CodEstab=" + General.EstabSession +"&Fecha=" + Fecha.ToString("dd/MM/yyyy HH:mm");
            //var uri = new Uri(RestUrl);
            //var response = await client.GetAsync(uri);

            //if (response.IsSuccessStatusCode)
            //{
            //    var resultado = response.Content.ReadAsStringAsync().Result;
            //    var inv = JsonConvert.DeserializeObject<InventarioProductoFechaModel>(resultado);

            //    inventarioUCFecha = inv.uc;
            //    inventarioUAFecha = inv.ua;
            //}
            //else
            //{
            //    return;
            //}
        }

        private async void OpenNotasProducto()
        {
            try
            {
                if (!string.IsNullOrEmpty(_invModel.folio))
                {
                    var i = _difSelected;
                    await PopupNavigation.Instance.PushAsync(new NotasConteoView());
                    MessagingCenter.Send<DetalleConteo>(i, "DifProdSelected");
                }
                else
                {
                    return;
                }
                
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async void InformacionConteo()
        {
            try
            {
                if (string.IsNullOrEmpty(_invModel.folio.Trim()))
                {
                    colorModel.MissingValue();
                    return;
                }
                else
                {
                    colorModel.CheckValue();
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Folio", _invModel.folio.Trim());
                    var url = "http://" + General.urlWS + "/api/Inventario/ConsultaInventario";
                    var resp = await client.Get<InventarioModel>(url, parametros);
                    if (!resp.Ok)
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " No se encontro información.");
                        return;
                    }
                    if(resp.Result.registrado == true)
                    {
                        await MostrarMsg.ShowMessage("El folio ingresado ya fue registrado como inventario físico.");
                        _invModel = new InventarioModel();
                        return;
                    }
                    else
                    {
                        _invModel = resp.Result;
                        _listProds = _invModel.ProductosConteo;
                        _enableControls = true;
                        ExpresaDiferencia();
                        _fechaConteo = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm", _formatInfo));
                    }
                }
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private void Limpiar()
        {
            colorModel.CheckValue();
            _enableControls = false;
            _invModel = new InventarioModel();
            _listProds = new ObservableCollection<DetalleConteo>();
            _producto = new DetalleConteo();
            _chkProdAdic = false;
            _listDifProds = new ObservableCollection<DetalleConteo>();
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            _buscarVisible = false;
            _fechaConteo = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm", _formatInfo));
            inventarioUCFecha = 0m;
            inventarioUAFecha = 0m;
        }

        private async void BuscarProducto()
        {
            try
            {
                if (!string.IsNullOrEmpty(_producto.cod_prod.Trim()))
                {
                    
                    _producto.cod_prod = await CheckCodigoBarras();

                    if (string.IsNullOrEmpty(_producto.cod_prod.Trim()))
                    {
                        await MostrarMsg.ShowMessage("No se encontró información");
                        return;
                    }
                    else
                    {
                        if (_chkProdAdic == true)
                        {
                            ConsultarProdXCodigoPA();
                        }
                        else
                        {
                            ProdDentroFolio();
                        }
                    }
                }
                else
                {
                    _producto = new DetalleConteo();
                    colorProdEntry.MissingValue();
                }
                
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async void ConteoUC()
        {
            try
            {
                var i = _producto.unidades_compra;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        public async void ConsultarProdXCodigoPA()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Codigo", _producto.cod_prod.Trim());
                parametros.Add("Filtro", "");
                parametros.Add("Estab", General.userCodEstab);
                var url = "http://" + General.urlWS + "/api/Inventario/BuscarProductos";
                var resp = await client.Get<List<Productos>>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " No se encontro el producto.");
                    return;
                }
                else
                {
                    //Cambiar el código de barras por el código del producto 

                    var fechaServer = await ConsultarFechaServidor();

                    _producto.cod_prod = resp.Result.Select(x => x.cod_prod.Trim()).SingleOrDefault();

                    await InventarioInicial(_producto.cod_prod.Trim(), fechaServer);

                    var busqueda = (from item in resp.Result
                                    where item.cod_prod.Trim() == _producto.cod_prod.Trim() ||
                                    item.codigo_barras_unidad.Trim() == _producto.cod_prod.Trim() ||
                                    item.codigo_barras_pieza.Trim() == _producto.cod_prod.Trim() 
                                    select new DetalleConteo()
                                    {
                                        folio = _invModel.folio.Trim(),
                                        cod_prod = item.cod_prod.Trim(),
                                        fecha =  Convert.ToDateTime(fechaServer.ToString("dd/MM/yyyy HH:mm", _formatInfo)),
                                        usuario = General.userCode,
                                        unidades_compra = 0m,
                                        unidades_alternativas = 0m,
                                        exist_unidades_compra = inventarioUCFecha,
                                        exist_unidades_alternativas = inventarioUAFecha,
                                        programacion = "",                                        
                                        descripcion_completa = item.descripcion_completa.Trim(),
                                        NombreUC = item.NombreUC.Trim(),
                                        NombreUA = item.NombreUA.Trim(),
                                        forma_expresar_inventario = item.forma_expresar_inventario.Trim(),
                                        contenido = item.contenido,
                                        AbrevUC = item.AbrevUC.Trim(),
                                        AbrevUA = item.AbrevUA.Trim(),
                                        codigo_barras_unidad = item.codigo_barras_unidad.Trim(),
                                        codigo_barras_pieza = item.codigo_barras_pieza.Trim(),
                                        notas = "",
                                        contado = false
                                    }).FirstOrDefault();

                    if(busqueda != null)
                    {
                        _producto = busqueda;
                        colorProdEntry.CheckValue();
                        ExpresaDiferencia();
                        _buscarVisible = false;
                        _fechaConteo = fechaServer;
                    }
                    else
                    {
                        await MostrarMsg.ShowMessage(resp.Message + " No se encontro el producto.");                        
                        _producto = new DetalleConteo();
                        _entryCodProd.Focused = true;
                        return;
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
                _buscarVisible = false;
            }
        }

        public async void ProdDentroFolio()
        {
            try
            {
                DetalleConteo busqueda = new DetalleConteo();
                busqueda = (from item in _listProds
                                where item.cod_prod.Trim() == _producto.cod_prod.Trim() ||
                                item.codigo_barras_unidad.Trim() == _producto.cod_prod.Trim() ||
                                item.codigo_barras_pieza.Trim() == _producto.cod_prod.Trim()
                                select item).FirstOrDefault<DetalleConteo>();

                if (busqueda != null)
                {
                    if (_enableControls == true)
                    {
                        do
                        {
                            if (busqueda.unidades_compra > 0 || busqueda.unidades_alternativas > 0 || busqueda.contado == true)
                            {
                                var resp = await MostrarMsg.ShowQuestionMsg("El producto " + _producto.cod_prod.Trim() + " ya fue contado, ¿Desea actualizar la cantidad?");
                                if (!resp)
                                {
                                    _producto = new DetalleConteo();
                                    _entryCodProd.Focused = true;
                                    return;
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        while (false);

                        if (busqueda.unidades_compra <= 0 && busqueda.unidades_alternativas <= 0)
                        {
                            var fechaServer = await ConsultarFechaServidor();

                            busqueda.fecha = Convert.ToDateTime(fechaServer.ToString("dd/MM/yyyy HH:mm", _formatInfo));
                            _fechaConteo = Convert.ToDateTime(busqueda.fecha.ToString("dd/MM/yyyy HH:mm", _formatInfo));
                        }
                        else
                            _fechaConteo = Convert.ToDateTime(busqueda.fecha.ToString("dd/MM/yyyy HH:mm", _formatInfo));
                    }
                    else
                        _fechaConteo = Convert.ToDateTime(busqueda.fecha.ToString("dd/MM/yyyy HH:mm", _formatInfo));

                    await InventarioInicial(busqueda.cod_prod.Trim(), busqueda.fecha);

                    var prods = new DetalleConteo
                    {
                        folio = busqueda.folio,
                        cod_prod = busqueda.cod_prod,
                        fecha = busqueda.fecha,
                        usuario = busqueda.usuario,
                        unidades_compra = busqueda.unidades_compra,
                        unidades_alternativas = busqueda.unidades_alternativas,
                        exist_unidades_compra = inventarioUCFecha,
                        exist_unidades_alternativas = inventarioUAFecha,
                        programacion = busqueda.programacion,
                        descripcion_completa = busqueda.descripcion_completa,
                        NombreUC = busqueda.NombreUC,
                        NombreUA = busqueda.NombreUA,
                        forma_expresar_inventario = busqueda.forma_expresar_inventario,
                        difUC = busqueda.difUC,
                        difUA = busqueda.difUA,
                        contenido = busqueda.contenido,
                        AbrevUC = busqueda.AbrevUC,
                        AbrevUA = busqueda.AbrevUA,
                        codigo_barras_unidad = busqueda.codigo_barras_unidad,
                        codigo_barras_pieza = busqueda.codigo_barras_pieza,
                        notas = busqueda.notas,
                        contado = busqueda.contado
                    };

                    _producto = prods;                    
                    colorProdEntry.CheckValue();                    
                }
                else
                {
                    await MostrarMsg.ShowMessage("El producto ingresado no se encuentra dentro del folio.");
                    _producto = new DetalleConteo();
                    _entryCodProd.Focused = true;
                    return;
                }

                //if (busqueda != null)
                //{
                //    if (busqueda.unidades_compra <= 0 && busqueda.unidades_alternativas <= 0)
                //    {

                //        busqueda.fecha = DateTime.Now;
                //    }
                //    if (busqueda.unidades_compra > 0 || busqueda.unidades_alternativas > 0)
                //    {
                //        var resp = await MostrarMsg.ShowQuestionMsg("El producto " + busqueda.cod_prod.Trim() +
                //            " ya fue contado, ¿Desea actualizar la cantidad?");
                //        if (!resp)
                //        {
                //            _producto = new DetalleConteo();
                //            _entryCodProd.Focused = true;
                //            return;
                //        }

                //        else
                //        {
                //            _producto = busqueda;
                //            colorProdEntry.CheckValue();
                //        }
                //    }
                //    else
                //    {
                //        _producto = busqueda;
                //        colorProdEntry.CheckValue();
                //    }
                //}
                //else
                //{
                //    await MostrarMsg.ShowMessage("El producto ingresado no se encuentra dentro del folio.");
                //    _producto = new DetalleConteo();
                //    _entryCodProd.Focused = true;                    
                //    return;
                //}
            }
            catch(Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
            finally
            {
                _buscarVisible = false;
            }
        }

        public async void AgregarGuardarCantidades()
        {
            try
            {
                var save = new DetalleConteo()
                {
                    folio = _invModel.folio.Trim(),
                    cod_prod = _producto.cod_prod.Trim(),
                    fecha = Convert.ToDateTime(_producto.fecha.ToString("dd/MM/yyyy HH:mm", _formatInfo)),
                    usuario = _producto.usuario.Trim(),
                    unidades_compra = _producto.unidades_compra,
                    unidades_alternativas = _producto.unidades_alternativas,
                    exist_unidades_compra = _producto.exist_unidades_compra,
                    exist_unidades_alternativas = _producto.exist_unidades_alternativas,
                    programacion = _producto.programacion,                    
                    descripcion_completa = _producto.descripcion_completa,
                    NombreUC = _producto.NombreUC.Trim(),
                    NombreUA = _producto.NombreUA.Trim(),
                    forma_expresar_inventario = _producto.forma_expresar_inventario.Trim(),
                    contenido = _producto.contenido,
                    AbrevUC = _producto.AbrevUC.Trim(),
                    AbrevUA = _producto.AbrevUA.Trim(),
                    codigo_barras_unidad = _producto.codigo_barras_unidad.Trim(),
                    codigo_barras_pieza = _producto.codigo_barras_pieza.Trim(),
                    notas = _producto.notas,
                    contado = true
                };

                RestClient client = new RestClient(null);
                var url = "http://" + General.urlWS + "/api/Inventario/GuardarCantidadesConteo";
                var resp = await client.Post<DetalleConteo>(url, null, save);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " No se pudo guardar las cantidades.");
                    return;
                }
                else
                {
                    await MostrarMsg.ShowMessage("Se actualizaron las cantidades.");
                    _producto = new DetalleConteo();
                    _entryCodProd.Focused = true;

                    InformacionConteo(); //CARGAR NUEVAMNETE LOS PRODUCTOS YA CON LA ACTUALIZACIÓN DE LAS CANTIDADES 
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private Tuple<decimal,decimal> Expresar_Inventario(decimal Contenido, string FormaExpInv, decimal UCs, decimal UAs) //BASADO EN EL METODO DEL MG EN EL BMS LLAMADO DE LA MISMA MANERA
        {
            try
            {
                decimal cant_temp = 0m;
                decimal divisor = 0m;
                
                if(FormaExpInv.Trim() == "U")
                {
                    UCs = UCs + (UAs / Contenido);
                    UAs = 0;
                }
                else if(FormaExpInv.Trim() == "P")
                {
                    UAs = UAs + (UCs * Contenido);
                    UCs = 0;
                }
                else
                {
                    if(Contenido >= 1)
                    {
                        divisor = 1;
                        UAs += (UCs - (int)(UCs)) * Contenido;
                        UCs = (int)(UCs);
                        cant_temp = (int)(UAs / Contenido);
                        UCs = UCs + cant_temp;
                        UAs = UAs - (cant_temp * Contenido);
                    }
                    else
                    {
                        divisor = Contenido;
                        UCs += (UAs - (int)(UAs)) / Contenido;
                        UAs = (int)(UAs);
                        cant_temp = (int)(UCs * Contenido);
                        UAs = UAs + cant_temp;
                        UCs = UCs - (cant_temp / Contenido);
                    }
                    if(UCs > 0 && UAs < 0)
                    {
                        UAs = UAs + (Contenido / divisor);
                        UCs = UCs - (1 / divisor);
                    }
                    if(UCs < 0 && UAs > 0)
                    {
                        UAs = UAs - (Contenido / divisor);
                        UCs = UCs + (1 / divisor);
                    }                                        
                }

                return Tuple.Create(UCs , UAs);
            }
            catch (Exception ex)
            {                
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// En el metodo expresa diferencia se manda llamar el metodo expresar inventario, el cual estaba basado en el médoto del MG del BMS 
        /// con el mismo nombre, al llamarse el método y realizar los respectivos calculos, los valores de existencia de unidades cambian, por lo tanto
        /// los tuve que volver a asignar con los nuevos valores, con esos valores se pudieron realizar los calculos para las variables de diferencia, 
        /// dentro del metodo del BMS se vuelve a realizar la llamada al método expresar inventario y una vez más tuve que volver asignar los valores de
        /// diferencia con los nuevos resultados. De esta manera los calculos de la app como de la opcion del BMS coinciden 
        /// </summary>

        private async void ExpresaDiferencia()
        {
            try
            {

                var _listaInvFecha = new ObservableCollection<InventarioProductoFechaModel>();

                foreach (var item in _listProds)
                {
                    await InventarioInicial(item.cod_prod, item.fecha);

                    var nuevo = new InventarioProductoFechaModel
                    {
                        uc = inventarioUCFecha,
                        ua = inventarioUAFecha,
                        codprod = item.cod_prod.Trim()
                    };

                    _listaInvFecha.Add(nuevo);
                }

                var _listDifAux = new ObservableCollection<DetalleConteo>();

                foreach (var item in _listProds)
                {
                    var valorUC = (from x in _listaInvFecha
                                   where x.codprod.Trim() == item.cod_prod.Trim()
                                   select x.uc).SingleOrDefault();

                    var valorUA = (from x in _listaInvFecha
                                   where x.codprod.Trim() == item.cod_prod.Trim()
                                   select x.ua).SingleOrDefault();

                    //Item1 significa UC e Item2 significa UA
                    var result = Expresar_Inventario(item.contenido, item.forma_expresar_inventario, valorUC, valorUA);
                    item.exist_unidades_compra = result.Item1;
                    item.exist_unidades_alternativas = result.Item2;
                    item.difUC = item.unidades_compra - result.Item1;
                    item.difUA = item.unidades_alternativas - result.Item2;                    
                    var result2 = Expresar_Inventario(item.contenido, item.forma_expresar_inventario, item.difUC, item.difUA);
                    item.difUC = result2.Item1;
                    item.difUA = result2.Item2;

                    _listDifAux.Add(item);
                }

                //Mostrar solamente los que tienen cantidad en existencia o en diferencia
                var dif = (from item in _listDifAux
                           where item.exist_unidades_compra != 0 || item.exist_unidades_alternativas != 0 
                           || item.difUC != 0 || item.difUA != 0
                           select new DetalleConteo
                           {
                               folio = item.folio.Trim(),
                               cod_prod = item.cod_prod.Trim(),
                               fecha = item.fecha,
                               usuario = item.usuario,
                               unidades_compra = item.unidades_compra,
                               unidades_alternativas = item.unidades_alternativas,
                               exist_unidades_compra = item.exist_unidades_compra,
                               exist_unidades_alternativas = item.exist_unidades_alternativas,
                               programacion = item.programacion,                               
                               descripcion_completa = item.descripcion_completa.Trim(),
                               NombreUC = item.NombreUC.Trim(),
                               NombreUA = item.NombreUA.Trim(),
                               forma_expresar_inventario = item.forma_expresar_inventario.Trim(),
                               contenido = item.contenido,
                               difUC = item.difUC,
                               difUA = item.difUA,
                               AbrevUC = item.AbrevUC.Trim(),
                               AbrevUA = item.AbrevUA.Trim(),
                               codigo_barras_unidad = item.codigo_barras_unidad.Trim(),
                               codigo_barras_pieza = item.codigo_barras_pieza.Trim(),
                               notas = item.notas,
                               contado = item.contado
                           }).ToList();

                _listDifProds = new ObservableCollection<DetalleConteo>(dif);
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
            try
            {
                var status = await CheckCameraStatus();
                 
                if (status)
                {
                    var options = new MobileBarcodeScanningOptions();
                    options.PossibleFormats = new List<BarcodeFormat>
                        {
                            BarcodeFormat.QR_CODE,
                            BarcodeFormat.CODE_128,
                            BarcodeFormat.EAN_13,
                            BarcodeFormat.CODE_39,
                            BarcodeFormat.CODE_93,
                            BarcodeFormat.UPC_A,
                            BarcodeFormat.UPC_E
                    };

                    var page = new ZXingScannerPage(options) { Title = "Escaner" };
                    var closeItem = new ToolbarItem { Text = "Cerrar" };
                    closeItem.Clicked += (object sender1, EventArgs e1) =>
                    {
                        page.IsScanning = false;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PopModalAsync();
                        });
                    };
                    page.ToolbarItems.Add(closeItem);
                    page.OnScanResult += (result) =>
                    {
                        page.IsScanning = false;

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Application.Current.MainPage.Navigation.PopModalAsync();
                            if (string.IsNullOrEmpty(result.Text))
                            {
                                await MostrarMsg.ShowMessage("El código no es valido.");
                                return;
                            }
                            else
                            {
                                _producto.cod_prod = result.Text.Trim();
                                _producto.cod_prod = await CheckCodigoBarras();

                                if(!string.IsNullOrEmpty(_producto.cod_prod))
                                    BuscarProducto();
                                else
                                {
                                    await MostrarMsg.ShowMessage("No se encontro información con ese código de barras.");
                                    return;
                                }
                            }
                        });
                    };
                    await Navigation.PushModalAsync(new NavigationPage(page) { BarTextColor = Color.White, BarBackgroundColor = Color.CadetBlue }, true);
                }                              
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                throw;
            }
        }

        private async Task<bool> CheckCameraStatus()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    await MostrarMsg.ShowMessage("Se necesitan permisos de la cámara para esta acción.");

                    var check = await Permissions.RequestAsync<Permissions.Camera>();
                    
                    if(check == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else
                    {
                        var checkAgain = await Permissions.RequestAsync<Permissions.Camera>();
                        if(checkAgain == PermissionStatus.Granted)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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

        public async void BuscarProdTextChanged()
        {
            try
            {
                if(_chkProdAdic == true)
                {
                    if (string.IsNullOrEmpty(_producto.cod_prod.Trim()))
                        return;
                    if (_producto.cod_prod.Trim().Length < 3)
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
                        parametros.Add("Filtro", _producto.cod_prod.Trim());
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
                else
                {
                    if (string.IsNullOrEmpty(_producto.cod_prod.Trim()))
                        return;
                    if (_producto.cod_prod.Trim().Length < 3)
                    {
                        _listaFiltro.Clear();
                        _buscarVisible = false;
                        return;
                    }
                    else
                    {
                        _listaFiltro.Clear();
                        _buscarVisible = true;
                        
                        var busqueda = (from item in _listProds
                                        where item.descripcion_completa.ToLower().Contains(_producto.cod_prod.ToLower()) //En este caso el cod_prod funciona como la descripcion que van tecleando (Filtro)
                                        select item).ToList<DetalleConteo>();

                        _listaFiltro = new ObservableCollection<DetalleConteo>(busqueda);
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

        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (DetalleConteo)item;

            if (i.unidades_compra <= 0 && i.unidades_alternativas <= 0)
            {
                i.fecha = DateTime.Now;
            }
            if (i.unidades_compra > 0 || i.unidades_alternativas > 0)
            {
                var resp = await MostrarMsg.ShowQuestionMsg("El producto " + i.cod_prod.Trim() +
                    " ya fue contado, ¿Desea actualizar la cantidad?");
                if (!resp)
                {
                    _producto = new DetalleConteo();
                    _entryCodProd.Focused = true;
                    _buscarVisible = false;
                    return;
                }
                else
                {
                    _producto = i;
                    _buscarVisible = false;
                    colorProdEntry.CheckValue();
                }
            }
            else
            {
                _producto = i;
                _buscarVisible = false;
                colorProdEntry.CheckValue();
            }            
        });

        private async Task<string> CheckCodigoBarras()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("CodigoBarras", _producto.cod_prod.Trim());                
                parametros.Add("Estab", General.userCodEstab);
                var url = "http://" + General.urlWS + "/api/Inventario/CheckCodigoBarras";
                var resp = await client.Get<string>(url, parametros);

                if (!resp.Ok)
                {
                    return "";
                }
                else
                {
                    return resp.Result.Trim();
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
