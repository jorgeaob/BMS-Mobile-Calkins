using Android.Content.Res;
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

namespace BMSMobile.Models
{
    [AddINotifyPropertyChangedInterface]
    internal class NuevoLoteVM
    {
        public INavigation navigaton { get; set; }
        public NuevoLoteView view { get; set; }
        public GetNuevoFolio getNuevoFolio { get; set; }
        public Mensajes MostrarMsg { get; set; }
        public AIModel aIModel { get; set; }
        public Command LimpiarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public Command txtCodProdCompleted { get; set; }
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
        private string cantidad { get; set; }
        public string Cantidad { get => cantidad; set { cantidad = value; } }
        private DateTime fecha_cad { get; set; }
        public DateTime FechaCad { get => fecha_cad; set { fecha_cad = value; } }
        private ObservableCollection<Disponible> _listaTipo { get; set; }
        public ObservableCollection<Disponible> ListaTipo { get { return _listaTipo; } set { _listaTipo = value; } }
        public int pTipoIndex { get { return _pTipoIndex; } set { _pTipoIndex = value; } }
        private int _pTipoIndex { get; set; }
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
        public ICommand ItemTappedCommand => new Command(async (item) =>
        {
            var i = (DetalleConteo)item;
            codprod = i.cod_prod.Trim();
            descripcion = i.descripcion_completa.Trim();
            _buscarVisible = false;
            TrajoProducto = true;

        });
        public NuevoLoteVM(INavigation _navigation, NuevoLoteView _view)
        {
            navigaton = _navigation;
            view = _view;
            MostrarMsg = new Mensajes();
            getNuevoFolio = new GetNuevoFolio();
            ListaTipo = new ObservableCollection<Disponible> { new Disponible { Nombre = "Entrada" }, new Disponible { Nombre = "Recepción" } };
            pTipoIndex = 0;
            Cantidad = "0";
            txtCodProdCompleted = new Command(TraerProducto);
            GuardarCommand = new Command(Guardar);
            LimpiarCommand = new Command(Limpiar);
            ProdTextChangedCommand = new Command(BuscarProdTextChanged);
            _listaFiltro = new ObservableCollection<DetalleConteo>();
            TrajoProducto = false;
            GuardarModel = new NuevoLoteModel();
            _buscarVisible = false;
        }
        private void Borrar()
        {
            TrajoProducto = false;
            descripcion = "";
            loteFab = "";
            cantidad = "";
            fecha_cad = System.DateTime.Now;
            localizacion = "";
            loteRecep = "";
            _pTipoIndex = 0;
        }
        private void Limpiar()
        {
            TrajoProducto = false;
            codprod = "";
            descripcion = "";
            loteFab = "";
            cantidad = "";
            fecha_cad = System.DateTime.Now;
            localizacion = "";
            loteRecep = "";
            _pTipoIndex = 0;
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

        private async void Guardar()
        {
            try { 
                if (!TrajoProducto) { await MostrarMsg.ShowMessage("Agregue un producto válido."); return; }
                string TransRef = "";
                if (pTipoIndex == 0) { TransRef = "631"; } else { TransRef = "44"; }

                GuardarModel.Folio = await getNuevoFolio.GenerarFolio("598", General.EstabSession);
                GuardarModel.LoteFab = LoteFab.Trim();
                GuardarModel.FechaCaducidad = FechaCad;
                GuardarModel.Cantidad = Cantidad.Trim();
                GuardarModel.Localizacion = Localizacion.Trim();
                GuardarModel.CodEstab = General.EstabSession.Trim();
                GuardarModel.CodProd = CodProd.Trim();
                GuardarModel.FolioReferencia = LoteRecep.Trim();
                GuardarModel.TransReferencia = TransRef.Trim();
                GuardarModel.LoteRecep = LoteRecep.Trim();


                RestClient client = new RestClient(null);

              
                var url = "http://" + General.urlWS + "/api/NuevoLote/GuardarNuevoLote";
                var resp = await client.Post<string>(url, null,GuardarModel);

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
                    parametros.Add("Codigo", CodProd.Trim());
                    parametros.Add("Filtro", "");
                    parametros.Add("Estab", General.userCodEstab);
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

                        CodProd = resp.Result.Select(x => x.cod_prod.Trim()).SingleOrDefault();
                        Descripcion = resp.Result.Select(x => x.descripcion_completa.Trim()).SingleOrDefault();
                    Cantidad = "1";
                    TrajoProducto = true;
                 
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
    }
}
