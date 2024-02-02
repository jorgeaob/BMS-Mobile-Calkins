using BMSMobile.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMSMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : FlyoutPage
    {
        public Mensajes MostrarMsg { get; set; }
        public MenuView()
        {
            InitializeComponent();
            MostrarMsg = new Mensajes();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuViewFlyoutMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            if (string.IsNullOrEmpty(General.EstabSession) && page.Title != "Inicio")
            {
                await MostrarMsg.ShowMessage("Favor de seleccionar un establecimiento.");
                FlyoutPage.ListView.SelectedItem = null;
                return;
            }
            else
            {
                Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = Color.FromHex("0D47A1")
                };
                IsPresented = false;              
            }
            FlyoutPage.ListView.SelectedItem = null;

        }
    }
}