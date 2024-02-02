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
    public partial class InicioView : ContentPage
    {
        public InicioView()
        {
            InitializeComponent();
            BindingContext = new InicioVM(this);
        }
        
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as InicioVM;
            if (viewModel != null)
                await viewModel.MostrarListaEstab();
        }
    }
}