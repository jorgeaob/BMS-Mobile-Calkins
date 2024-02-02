using BMSMobile.ViewModels;
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
    public partial class EntradaView : TabbedPage
    {
        public EntradaView()
        {
            InitializeComponent();
            BindingContext = new EntradaVM(Navigation, this);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void chEntrada_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                chEntradaOrden.IsChecked = false;
            }
        }

        private void chEntradaOC_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                chEntrada.IsChecked = false;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnBindingContextChanged();
            var VM = BindingContext as EntradaVM;
            if (VM != null)
            {
                await VM.Focus();
            }
        }
    }

}

