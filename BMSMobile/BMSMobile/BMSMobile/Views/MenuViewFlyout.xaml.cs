using BMSMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMSMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuViewFlyout : ContentPage
    {
        public ListView ListView;

        public MenuViewFlyout()
        {
            InitializeComponent();

            BindingContext = new MenuViewFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        public class MenuViewFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuViewFlyoutMenuItem> MenuItems { get; set; }
            public Command LogoutCommand { get; set; }
            public LogoutVM _logout { get; set; }

            public MenuViewFlyoutViewModel()
            {
                _logout = new LogoutVM();

                MenuItems = new ObservableCollection<MenuViewFlyoutMenuItem>(new[]
                {
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Inicio", Icon="BMSLogo.png", TargetType = typeof(InicioView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Conteo Productos", Icon="Inventario.png", TargetType = typeof(InventarioView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Entrada Mercancia", Icon="Entrada.png", TargetType = typeof(EntradaView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Ubicar", Icon="Ubicar.png", TargetType = typeof(UbicarView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Recepción de Transferencias", Icon="RecepTransf.png", TargetType = typeof(RecepcionTransferenciaView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Entrada de Devoluciones", Icon="Devolucion.png", TargetType = typeof(EntradaDevolucionesView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Consulta de Pallet", Icon="Pallet.png", TargetType = typeof(ConsultaPalletView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Entradas y Salidas de una Loc.", Icon="EntradaSalida.png", TargetType = typeof(EntradasSalidasLoc) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Nuevo Lote", Icon="NuevoLote.png", TargetType = typeof(NuevoLoteView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Retirar Mercancía", Icon="Retirar.png", TargetType = typeof(RetirarMercanciaView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Actualizar Ubicación", Icon="ActualizarUbicacion.png", TargetType = typeof(ActualizarUbicacionView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Surtir", Icon="Surtir.png", TargetType = typeof(SurtidoView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Reubicar Mercancía", Icon="Reubicar.png", TargetType = typeof(ReubicarMercanciaView) },
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Surtido Picking", Icon="Picking.png", TargetType = typeof(SurtidoPickingView) }
                });

                LogoutCommand = new Command(_logout.LogoutSession);
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}