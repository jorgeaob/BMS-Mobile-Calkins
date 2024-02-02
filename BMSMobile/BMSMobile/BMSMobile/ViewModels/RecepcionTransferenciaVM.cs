using Android.Text;
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
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace BMSMobile.viewModels
{
    [AddINotifyPropertyChangedInterface]
    public class RecepcionTransferenciaVM
    {
        public INavigation navigaton { get; set; }
        public RecepcionTransferenciaView view { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public GetNuevoFolio getNuevoFolio { get; set; }
        //public CheckCameraPermissions chkCamara { get; set; }

        private RecepcionTransferenciaModel _recepcion { get; set; }
        public RecepcionTransferenciaModel Recepcion
        {
            get { return _recepcion; }
            set { _recepcion = value; }
        }

        private ObservableCollection<RecepcionTransferenciaDetalleModel> _detalle { get; set; }
        public ObservableCollection<RecepcionTransferenciaDetalleModel> Detalle
        {
            get { return _detalle; }
            set { _detalle = value; }
        }

        private ObservableCollection<ProductosSurtidosModel> _productosSurtidos { get; set; }

        private RecepcionTransferenciaDetalleModel _productoTransf { get; set; }
        public RecepcionTransferenciaDetalleModel ProductoTransf
        {
            get { return _productoTransf; }
            set { _productoTransf = value; }
        }

        private ObservableCollection<RecepcionTransferenciaDetalleModel> _listaProds { get; set; }
        public ObservableCollection<RecepcionTransferenciaDetalleModel> ListaProds
        {
            get { return _listaProds; }
            set { _listaProds = value; }
        }

        private decimal _cantTranferir { get; set; }
        public decimal CantTransferir
        {
            get { return _cantTranferir; }
            set { _cantTranferir = value; }
        }

        private TotalesTransferenciaModel _totales { get; set; }
        public TotalesTransferenciaModel Totales
        {
            get { return _totales; }
            set { _totales = value; }
        }

        private string _notasGeneral { get; set; }
        public string NotasGeneral
        {
            get { return _notasGeneral; }
            set { _notasGeneral = value; }
        }
        private List<GuardarRecepcionDetalleModel> _guardarDetalle { get; set; }

        private string _cod_prod { get; set; }
        public string Cod_prod
        {
            get { return _cod_prod; }
            set { _cod_prod = value; }
        }

        private string _folio { get; set; }
        public string Folio
        {
            get { return _folio; }
            set { _folio = value; }
        }

        private bool palletsVisible { get; set; }
        public bool PalletsVisible { get { return palletsVisible; } set { palletsVisible = value; } }
        private int _pDispIndex { get; set; }
        public int pDispIndex { get { return _pDispIndex; } set { _pDispIndex = value; } }
        private int _pUnidadesIndex { get; set; }
        public int pUnidadesIndex { get { return _pUnidadesIndex; } set { _pUnidadesIndex = value; } }
        private ObservableCollection<Disponible> _listaDisponible { get; set; }
        public ObservableCollection<Disponible> ListaDisponible { get { return _listaDisponible; } set { _listaDisponible = value; } }
        private ObservableCollection<Disponible> _listaUnidades { get; set; }
        public ObservableCollection<Disponible> ListaUnidades { get { return _listaUnidades; } set { _listaUnidades = value; } }



        public Command FolioCompletedCommand { get; set; }
        public Command ProductoCompletedCommand { get; set; }
        public Command AgregarProductoCommand { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command EscanerCommand { get; set; }
        public Command pUnidadesChanged { get; set; }
        public Command nudPalletsChanged { get; set; }

        public RecepcionTransferenciaVM(INavigation _navigation, RecepcionTransferenciaView _view)
        {
            navigaton = _navigation;
            view = _view;
            MostrarMsg = new Mensajes();
            getNuevoFolio = new GetNuevoFolio();
            //chkCamara = new CheckCameraPermissions();

            _recepcion = new RecepcionTransferenciaModel();
            _detalle = new ObservableCollection<RecepcionTransferenciaDetalleModel>();
            _productosSurtidos = new ObservableCollection<ProductosSurtidosModel>();
            _productoTransf = new RecepcionTransferenciaDetalleModel();
            _cantTranferir = 0m;
            _listaProds = new ObservableCollection<RecepcionTransferenciaDetalleModel>();
            _totales = new TotalesTransferenciaModel();
            _notasGeneral = "";
            _guardarDetalle = new List<GuardarRecepcionDetalleModel>();
            _cod_prod = "";
            _folio = "";
            _pDispIndex = 0;
            _pUnidadesIndex = 0;
            palletsVisible = false;
            ListaDisponible = new ObservableCollection<Disponible> { new Disponible { Nombre = "Disponible" }, new Disponible { Nombre = "No Disponible" } };
            ListaUnidades = new ObservableCollection<Disponible> { new Disponible { Nombre = "Unidades" }, new Disponible { Nombre = "Pallets" } };
            FolioCompletedCommand = new Command(InfoTransferencia);
            ProductoCompletedCommand = new Command(ProductoTransferencia);
            AgregarProductoCommand = new Command(AgregarProducto);
            LimpiarCommand = new Command(LimpiarGeneral);
            GuardarCommand = new Command(GuardarRecepcion);
            pUnidadesChanged = new Command(UnidadesChanged);
            nudPalletsChanged = new Command(NudPalletsChanged);
        }

        public async Task Focus()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Entry entry = (Entry)view.FindByName("txtTransferencia");
                    entry.Focus();
                });
            });
        }

        public ICommand DeleteCommand => new Command(async (prod) =>
        {
            if (await MostrarMsg.ShowQuestionMsg("¿Desea eliminar el producto de la lista"))
            {
                var elemento = (RecepcionTransferenciaDetalleModel)prod;
                ListaProds.Remove(elemento);

                var total = from item in _listaProds
                            group item by new
                            {
                                item.cantidad_ingresar
                            } into g
                            select new TotalesTransferenciaModel()
                            {
                                cantidadTotal = g.Sum(i => i.cantidad_ingresar),
                                volumenTotal = g.Sum(i => i.volumen),
                                pesoTotal = g.Sum(i => i.peso),
                                costoTotal = g.Sum(i => i.costo)
                            };
                _totales = total.FirstOrDefault();
            }
        });
        private async void NudPalletsChanged()
        {

            try
            { 
                if (_productoTransf is null || string.IsNullOrEmpty(_productoTransf.cod_prod) || palletsVisible == false)
                { return; }

                CantTransferir = (decimal)((_productoTransf.altura * _productoTransf.cama) * _productoTransf.pallets);
                if (CantTransferir <= 0)
                    CantTransferir = 1;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }

           
        }
        private async void UnidadesChanged()
        {
            try
            {
                if (_productoTransf is null || string.IsNullOrEmpty(_productoTransf.cod_prod))
                {   palletsVisible = false;
                    return; }

                if (_pUnidadesIndex == 0)
                {
                    CantTransferir = 1;
                    ProductoTransf.pallets = 1;
                    palletsVisible = false;
                }
                else
                {
                    ProductoTransf.pallets = 1;
                    CantTransferir = (decimal)((_productoTransf.altura * _productoTransf.cama) * _productoTransf.pallets);
                    if (CantTransferir <= 0)
                        CantTransferir = 1;
                    palletsVisible = true;
                    Stepper nudPallets = (Stepper)this.view.FindByName("nudPallets");
                    nudPallets.Value = 1;
                }

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private async void InfoTransferencia()
        {
            try
            {
                Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");
                Entry txtTransferencia = (Entry)this.view.FindByName("txtTransferencia");

                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Folio", _recepcion.folio.Trim());
                var url = "http://" + General.urlWS + "/api/RecepcionTransferencia/InfoTransferencia";
                var resp = await client.Get<RecepcionTransferenciaModel>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage("No se pudo encontrar información con el folio ingresado.");
                    return;
                }
                else
                {
                    var infoRecepcion = resp.Result;

                    if (infoRecepcion.cod_estab_alterno.Trim() != General.EstabSession)
                    {
                        await MostrarMsg.ShowMessage("Está transferencia no es para este establecimiento.");
                        txtTransferencia.Focus();
                        return;
                    }
                    else if (infoRecepcion.status.Trim() == "C")
                    {
                        await MostrarMsg.ShowMessage("Está transferencia esta cancelada.");
                        txtTransferencia.Focus();
                        return;
                    }
                    else if (infoRecepcion.status_recepcion.Trim() == "T")
                    {
                        await MostrarMsg.ShowMessage("Está transferencia ya esta recepcionada en su totalidad.");
                        txtTransferencia.Focus();
                        return;
                    }
                    else
                    {
                        var detalleRecepcion = await GetDetalleTransferencia();
                        var prodSurtido = await GetProductosSurtidos();

                        var sumaDetalle = detalleRecepcion.Sum(x => x.cantidad);
                        var sumaSurtido = prodSurtido.Sum(x => x.cantidad);

                        if (sumaDetalle == sumaSurtido)
                        {
                            await MostrarMsg.ShowMessage("La transferencia ya no cuenta con cantidad pendiente a recepcionar.");
                            txtTransferencia.Focus();
                            return;
                        }
                        else
                        {
                            _recepcion = infoRecepcion;
                            _detalle = detalleRecepcion;
                            _productosSurtidos = prodSurtido;

                            txtCodProd.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private async Task<ObservableCollection<RecepcionTransferenciaDetalleModel>> GetDetalleTransferencia()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Folio", _recepcion.folio.Trim());
                var url = "http://" + General.urlWS + "/api/RecepcionTransferencia/DetalleRecepcionTransferencia";
                var resp = await client.Get<ObservableCollection<RecepcionTransferenciaDetalleModel>>(url, parametros);

                if (!resp.Ok)
                    return null;
                else
                    return resp.Result;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        private async Task<ObservableCollection<ProductosSurtidosModel>> GetProductosSurtidos()
        {
            try
            {
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Folio", _recepcion.folio.Trim());
                var url = "http://" + General.urlWS + "/api/RecepcionTransferencia/ProductosSurtidosTransferencia";
                var resp = await client.Get<ObservableCollection<ProductosSurtidosModel>>(url, parametros);

                if (!resp.Ok)
                    return null;
                else
                    return resp.Result;
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        private async void ProductoTransferencia()
        {
            try
            {
                Entry txtCantProd = (Entry)this.view.FindByName("txtCantProd");

                if (!string.IsNullOrEmpty(_recepcion.folio.Trim()))
                {
                    var exist = _detalle.Any(x => x.cod_prod.Trim() == _productoTransf.cod_prod.Trim() ||
                                            x.cod_prod.Trim() == _productoTransf.codigoEscaneado.Trim() ||
                                            x.codigo_barras_unidad.Trim() == _productoTransf.codigoEscaneado.Trim() ||
                                            x.codigo_barras_pieza.Trim() == _productoTransf.codigoEscaneado.Trim());

                    if (exist)
                    {
                        var prod = _detalle.Where(x => x.cod_prod.Trim() == _productoTransf.cod_prod.Trim() ||
                                    x.cod_prod.Trim() == _productoTransf.codigoEscaneado.Trim()).FirstOrDefault();

                         if (prod is null)
                            prod = _detalle.Where(x => x.codigo_barras_unidad.Trim() == _productoTransf.codigoEscaneado.Trim() ||
                                    x.codigo_barras_pieza.Trim() == _productoTransf.codigoEscaneado.Trim()).FirstOrDefault();



                        _cod_prod = prod.cod_prod.Trim();

                        var cantPedida = _detalle.Where(x => x.cod_prod.Trim() == prod.cod_prod.Trim()).Select(i => i.cantidad).FirstOrDefault();
                        var cantSurtida = _productosSurtidos.Where(x => x.cod_prod.Trim() == prod.cod_prod.Trim()).Sum(i => i.cantidad);

                        if (cantPedida == cantSurtida)
                        {
                            await MostrarMsg.ShowMessage("El producto " + prod.cod_prod.Trim() + " ha sido surtido en su totalidad.");
                            return;
                        }
                        else
                        {
                            _productoTransf = _detalle.Where(x => x.cod_prod.Trim() == prod.cod_prod.Trim()).FirstOrDefault();
                            txtCantProd.Focus();
                        }
                    }
                    else
                    {
                        await MostrarMsg.ShowMessage("El código de producto ingresado no se encuentra en la transferencia.");
                        return;
                    }
                }
                else
                {
                    await MostrarMsg.ShowMessage("No ha ingresado un folio a recepcionar.");
                    _cod_prod = "";
                    return;
                }



            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        private async void AgregarProducto()
        {
            try
            {
                Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");
                Entry txtCantProd = (Entry)this.view.FindByName("txtCantProd");

                if (string.IsNullOrEmpty(_productoTransf.cod_prod.Trim()))
                {
                    await MostrarMsg.ShowMessage("Ingrese un producto.");
                    txtCodProd.Focus();
                    return;
                }
                if (_cantTranferir <= 0)
                {
                    await MostrarMsg.ShowMessage("La cantidad no puede ser menor o igual a 0.");
                    txtCantProd.Focus();
                    return;
                }

                TimeSpan diferencia = _productoTransf.fecha_caducidad - DateTime.Now ;
                int diferenciaDias = diferencia.Days; // Obtiene la diferencia en días

                if ( _productoTransf.caducidad_fabricacion < diferenciaDias && _productoTransf.caducidad_fabricacion > 0)
                {
                    await MostrarMsg.ShowMessage("La fecha de caducidad es menor a la permitida.");
                    txtCantProd.Focus();
                    return;
                }
               
                    var yaAgregado = ListaProds.Where(x => x.cod_prod.Trim() == _productoTransf.cod_prod.Trim()).Select(i => i.cantidad_ingresar);

                    var cantPedida = _productoTransf.cantidad;
                    var cantSurtida = _productosSurtidos.Where(x => x.cod_prod.Trim() == _productoTransf.cod_prod.Trim()).Sum(i => i.cantidad);
                    var cantidadPendiente = cantPedida - cantSurtida;


                    if (yaAgregado.Count() >= 1)
                    {
                        var cantAgregada = yaAgregado.Sum();

                        if (_cantTranferir > (cantidadPendiente - cantAgregada))
                        {
                            await MostrarMsg.ShowMessage("La cantidad no puede ser mayor a la cantidad pendiente." + "\r\n" + "Cantidad pendiente: " + (cantidadPendiente - cantAgregada));
                            txtCantProd.Focus();
                        }
                        else
                        {
                            ListaProds
                                      .Where(x => x.cod_prod.Trim() == _productoTransf.cod_prod.Trim() && x.id == _productoTransf.id)
                                      .ToList()
                                      .ForEach(i =>
                                      {
                                          i.cantidad_ingresar += _cantTranferir;
                                          i.agregar_lote = ProductoTransf.agregar_lote;
                                          i.pallets = ProductoTransf.pallets;
                                          i.lote_fabricacion = ProductoTransf.lote_fabricacion;
                                          i.fecha_caducidad = ProductoTransf.fecha_caducidad;
                                      });
                            LimpiarProducto();
                        }
                    }
                    else
                    {
                        if (_cantTranferir > cantidadPendiente)
                        {
                            await MostrarMsg.ShowMessage("La cantidad no puede ser mayor a la cantidad pendiente." + "\r\n" + "Cantidad pendiente: " + cantidadPendiente);
                            txtCantProd.Focus();
                            return;
                        }
                        else
                        {
                            _productoTransf.cantidad_ingresar = _cantTranferir;

                            ListaProds.Add(_productoTransf);
                            LimpiarProducto();
                        }
                    }

                    var total = from item in _listaProds
                                group item by new
                                {
                                    item.cantidad_ingresar
                                } into g
                                select new TotalesTransferenciaModel()
                                {
                                    cantidadTotal = g.Sum(i => i.cantidad_ingresar),
                                    volumenTotal = g.Sum(i => i.volumen),
                                    pesoTotal = g.Sum(i => i.peso),
                                    costoTotal = g.Sum(i => i.costo)
                                };


                    _totales = total.FirstOrDefault();
                    txtCodProd.Focus();
                }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                throw;
            }
        }

        private void LimpiarProducto()
        {
            _productoTransf = new RecepcionTransferenciaDetalleModel();
            _cantTranferir = 1m;
            _cod_prod = "";
            Stepper nudPallets = (Stepper)this.view.FindByName("nudPallets");
            if (nudPallets != null)
            {
                nudPallets.Value = 1;
            }

        }

        private void LimpiarPantalla()
        {
            Entry txtTransferencia = (Entry)this.view.FindByName("txtTransferencia");
            _recepcion = new RecepcionTransferenciaModel();
            _detalle.Clear();
            _productosSurtidos.Clear();
            _listaProds.Clear();
            _totales = new TotalesTransferenciaModel();
            _notasGeneral = "";
            _guardarDetalle.Clear();
            LimpiarProducto();
            txtTransferencia.Focus();
            pDispIndex = 0;
            pUnidadesIndex = 0; 
        }

        private void LimpiarGeneral()
        {
            LimpiarPantalla();
            _folio = "";
        }

        private async void GuardarRecepcion()
        {
            try
            {
                Entry txtTransferencia = (Entry)this.view.FindByName("txtTransferencia");
                Entry txtCodProd = (Entry)this.view.FindByName("txtCodProd");

                if (string.IsNullOrEmpty(_recepcion.folio))
                {
                    await MostrarMsg.ShowMessage("Ingrese un folio");
                    txtTransferencia.Focus();
                    return;
                }
                else if (_listaProds.Count <= 0)
                {
                    await MostrarMsg.ShowMessage("No ha agregado productos");
                    txtCodProd.Focus();
                    return;
                }
                else
                {
                    var pregunta = await MostrarMsg.ShowQuestionMsg("¿Desea guardar la recepción?");

                    if (!pregunta)
                        return;
                    else
                    {
                        _guardarDetalle.Clear();

                        var FolioRecepcion = await getNuevoFolio.GenerarFolio("65", General.EstabSession);

                        foreach (var item in _listaProds)
                        {
                            decimal costo_operativo = 0m;

                            if (item.unidad.Trim() == "U")
                                costo_operativo = (decimal)(item.costo_promedio / 1);
                            else
                                costo_operativo = (decimal)(item.costo_promedio / item.contenido);


                            var detalle = new GuardarRecepcionDetalleModel
                            {
                                cod_prod = item.cod_prod.Trim(),
                                unid = item.unidad.Trim(),
                                cod_estab = item.cod_estab.Trim(),
                                folioTransferencia = FolioRecepcion,
                                transaccion = "65",
                                backorder = false,
                                afecta_no_disponible = (_pDispIndex == 0) ? false : true,
                                cant = item.cantidad_ingresar,
                                costo_operativo_unitario = costo_operativo,
                                costo_promedio_as = 0m,
                                cod_prv = "",
                                Cantidad_adicional = 0m,
                                unidad = item.unidad,
                                cantidad = item.cantidad_ingresar,
                                precio_lista = item.precio_lista,
                                tipo_precio_venta = item.tipo_precio_venta,
                                importe = item.importe,
                                iva = item.iva,
                                ieps = item.ieps,
                                costo = item.costo,
                                peso = item.peso,
                                volumen = item.volumen,
                                id_origen = item.id,
                                abreviatura_unidad = item.abreviatura_unidad,
                                fecha_caducidad = item.fecha_caducidad,
                                lote_fabricacion = item.lote_fabricacion,
                                agregar_lote = item.agregar_lote,
                                pallets = item.pallets
                               };

                            _guardarDetalle.Add(detalle);
                        }

                        var cabecero = new GuardarRecepcionModel
                        {
                            folio = FolioRecepcion,
                            transaccion = "65",
                            transaccion_origen = _recepcion.transaccion,
                            cod_estab = _recepcion.cod_estab_alterno,
                            cod_estab_alterno = _recepcion.cod_estab,
                            folio_transferencia = _recepcion.folio,
                            importeGeneral = _recepcion.importe,
                            ivaGeneral = _recepcion.iva,
                            iepsGeneral = _recepcion.ieps,
                            costoGeneral = _recepcion.costo,
                            unidadesGeneral = _recepcion.unidades,
                            piezasGeneral = _recepcion.piezas,
                            pesoGeneral = _recepcion.peso,
                            volumenGeneral = _recepcion.volumen,
                            usuario = General.userCode,
                            razon_aod_inventario = _recepcion.razon_aod_inventario,
                            folio_referencia = "",
                            orden_embarque = 0,
                            recoge_mercancia = false,
                            notasGeneral = _notasGeneral,
                            clave_afectacion_inventario = _recepcion.clave_afectacion_inventario,
                            Operacion = "RT",
                            listaDetalle = _guardarDetalle,
                        };

                        RestClient client = new RestClient(null);
                        var url = "http://" + General.urlWS + "/api/RecepcionTransferencia/GuardarRecepcionTransferencia";
                        var resp = await client.Post<GuardarRecepcionModel>(url, null, cabecero);

                        if (!resp.Ok)
                        {
                            await MostrarMsg.ShowMessage("No se pudo guardar la información");
                            return;
                        }
                        else
                        {
                            await MostrarMsg.ShowMessage("Folio: " + FolioRecepcion.Trim() + "\r\n" +
                                "La información se guardó correctamente.");
                            LimpiarGeneral();
                        }
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

        public ICommand ProdTextChangedCommand => new Command(() =>
        {
            if (_cod_prod.Trim().Length < 3)
            {
                _productoTransf = new RecepcionTransferenciaDetalleModel();
                _cantTranferir = 0m;
            }

            _productoTransf.cod_prod = _cod_prod;
        });

        public ICommand FolioTextChangedCommand => new Command(() =>
        {
            if (_folio.Trim().Length < 3)
            {
                LimpiarPantalla();
            }
            _recepcion.folio = _folio;
        });


      

    }
}
