using BMSMobile.viewModels;
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
    public partial class RecepcionTransferenciaView : TabbedPage
    {
        public RecepcionTransferenciaView()
        {
            InitializeComponent();
            BindingContext = new RecepcionTransferenciaVM(Navigation, this);
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override async void OnAppearing()
        {
            base.OnBindingContextChanged();
            var VM = BindingContext as RecepcionTransferenciaVM;
            if (VM != null)
            {
                await VM.Focus();
            }
        }
    }
}