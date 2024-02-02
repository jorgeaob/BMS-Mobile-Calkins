using Android.Content.Res;
using BMSMobile.Models;
using BMSMobile.Utilities;
using BMSMobile.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Android.Renderscripts.ScriptGroup;

namespace BMSMobile.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class RetirarMercanciaVM
    {
        public INavigation navigaton { get; set; }
        public RetirarMercanciaView view { get; set; }
        public GetNuevoFolio getNuevoFolio { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command txtCodProdCompleted { get; set; }
        public Command txtLocCompleted { get; set; }   
        public Command ProdTextChangedCommand { get; set; }
        private string loteFab { get; set; }
        public string LoteFab { get => loteFab; set { loteFab = value; } }
        private string loteRecep { get; set; }
        public string LoteRecep { get => loteRecep; set { loteRecep = value; } }
        private string localizacion { get; set; }
        public string Localizacion { get => localizacion; set { localizacion = value; } }
        private string codprod { get; set; }
        public string CodProd { get => codprod; set { codprod = value; } }
        private string descripcion { get; set; }
        public string Descripcion { get => descripcion; set { descripcion = value; } }
        private bool habilitaCodProd { get; set; }
        public bool HabilitaCodProd { get => habilitaCodProd; set { habilitaCodProd = value; } }
        private string cantidad { get; set; }
        public string Cantidad { get => cantidad; set { cantidad = value; } }
        private DateTime fecha_cad { get; set; }
        public DateTime FechaCad { get => fecha_cad; set { fecha_cad = value; } }
        private ObservableCollection<Disponible> _listaTipo { get; set; }
        public ObservableCollection<Disponible> ListaTipo { get { return _listaTipo; } set { _listaTipo = value; } }
        public int pTipoIndex { get { return _pTipoIndex; } set { _pTipoIndex = value; } }
        private int _pTipoIndex { get; set; }
        private bool TrajoProducto { get; set; }
        private string Pallet { get; set; }
        private string Lote_Recepcion { get; set; }
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
        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (DetalleConteo)item;
            codprod = i.cod_prod.Trim();
            _buscarVisible = false;

        });
        public RetirarMercanciaVM(INavigation _navigation, RetirarMercanciaView _view)
        {
            navigaton = _navigation;
            view = _view;
            MostrarMsg = new Mensajes();
            getNuevoFolio = new GetNuevoFolio();
            Cantidad = "0";
            txtCodProdCompleted = new Command(TraerProducto);
            EliminarCommand = new Command(Eliminar);
            LimpiarCommand = new Command(Limpiar);
            ProdTextChangedCommand = new Command(BuscarProdTextChanged);
            txtLocCompleted = new Command(TraerLoc);
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            TrajoProducto = false;
            GuardarModel = new NuevoLoteModel();
            _buscarVisible = false;
            HabilitaCodProd = false;
            Limpiar();
        }

        private async void TraerLoc()
        {
            try
            {
                Borrar();
                if (string.IsNullOrEmpty(localizacion.Trim()))
                    return;
                //localizacion = ValidaDigitoVerificador(localizacion, false);
                if (localizacion.Trim() =="-1")
                {
                    _buscarVisible = false;
                    return;
                }
                else
                {
                    RestClient client = new RestClient(null);
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Localizacion", localizacion);
                    var url = "http://" + General.urlWS + "/api/RetirarMercancia/TraerLoc";
                    var resp = await client.Get<ObservableCollection<Localizacion>>(url, parametros);

                    if (!resp.Ok)
                    {
                        return;
                    }
                    else
                    {
                        ObservableCollection<Localizacion> info = resp.Result;

                        if (info.Count > 1)
                        {
                            HabilitaCodProd = true;
                            Entry txtLote = (Entry)this.view.FindByName("txtCodProd");
                            txtLote.Focus();
                            TrajoProducto = false;
                        }
                        else if (info.Count == 1)
                        {
                            codprod = info[0].cod_prod.Trim();
                            descripcion = info[0].descripcion.Trim() ;
                            cantidad = info[0].cantidad.ToString();
                            Pallet = info[0].lote_recepcion.ToString().Trim();
                            Lote_Recepcion = info[0].folio_referencia.ToString().Trim();
                            Entry txtLote = (Entry)this.view.FindByName("txtCantidad");
                            txtLote.Focus();
                            TrajoProducto = true;
                            _buscarVisible = false;
                        }
                        else
                        {
                            await MostrarMsg.ShowMessage("Esta localización no esta disponible o esta vacia.");
                            return;
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
        private void Borrar()
        {
            TrajoProducto = false;
            _buscarVisible = false;
            descripcion = "";
            loteFab = "";
            cantidad = "0";
            loteRecep = "";
        }
        private void Limpiar()
        {
            TrajoProducto = false;
            _buscarVisible =false;
            codprod = "";
            descripcion = "";
            loteFab = "";
            cantidad = "";
            fecha_cad = System.DateTime.Now;
            localizacion = "";
            loteRecep = "";
            _pTipoIndex = 0;
            Entry txtLote = (Entry)this.view.FindByName("txtLoc");
            txtLote.Focus();
        }

        public async void BuscarProdTextChanged()
        {
            try
            {
                Borrar();
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

        private async void Eliminar()
        {
            try
            {
                if (!TrajoProducto) { await MostrarMsg.ShowMessage("Agregue un producto válido."); return; }
               if(decimal.Parse(cantidad) <= 0) { await MostrarMsg.ShowMessage("La cántidad debe ser mayor a 0."); return; }

                ModificaInventarioLoc modificaInventarioLoc = new ModificaInventarioLoc();
                modificaInventarioLoc.CodProd = codprod.Trim();
                modificaInventarioLoc.CodEstab = General.EstabSession.Trim();
                modificaInventarioLoc.Transaccion = "624";
                modificaInventarioLoc.Unidad = "U";
                modificaInventarioLoc.Cantidad = decimal.Parse(Cantidad)*-1;
                modificaInventarioLoc.Guardar = true;
                modificaInventarioLoc.MSG = "";
                modificaInventarioLoc.Localizacion = localizacion.Trim();
                modificaInventarioLoc.AfectaNoDisponible = false;
                modificaInventarioLoc.Lote = "";
                modificaInventarioLoc.FolioRef = Lote_Recepcion;
                modificaInventarioLoc.TransRef = "624";
                modificaInventarioLoc.LoteRecepcion = Pallet;

                RestClient client = new RestClient(null);


                var url = "http://" + General.urlWS + "/api/Inventario/ModificaInventarioLoc";
                var resp = await client.Post<int>(url, null, modificaInventarioLoc);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(" No se pudo guardar la información.");
                    return;
                }
                else
                {
                    await MostrarMsg.ShowMessage("Información guardada correctamente");
                    GuardarModel = new NuevoLoteModel();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());

                throw;
            }
        }
        private async void TraerProducto()
        {

            try
            {
                descripcion = "";
                RestClient client = new RestClient(null);
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("CodProd", CodProd.Trim());
                parametros.Add("Localizacion", Localizacion.Trim());
                var url = "http://" + General.urlWS + "/api/RetirarMercancia/TraerProd";
                var resp = await client.Get<ProductoLoc>(url, parametros);

                if (!resp.Ok)
                {
                    await MostrarMsg.ShowMessage(resp.Message + " No se encontró el producto.");
                    TrajoProducto = false;
                    return;
                }
                else
                {
                    var info = resp.Result;
                    cantidad = info.cantidad.ToString();
                    descripcion = info.descripcion.ToString();
                    TrajoProducto = true;
                    Pallet = info.lote_recepcion.ToString();
                    Lote_Recepcion = info.folio_referencia.ToString();
                }

            }
            catch (Exception ex)
            {
                await MostrarMsg.ShowMessage(ex.Message);
                Console.WriteLine("Error: " + ex.Message.ToString());
                TrajoProducto = false;
                throw;
            }
            finally
            {
                //_buscarVisible = false;
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
