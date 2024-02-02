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
    public partial class SurtidoPickingView : TabbedPage
    {
        public SurtidoPickingView()
        {
            InitializeComponent();
            BindingContext = new SurtidoPickingVM(Navigation,this);
        }

        protected override async void OnAppearing()
        {
            base.OnBindingContextChanged();
            var VM = BindingContext as SurtidoPickingVM;
            if (VM != null)
            {
                await VM.Focus();
            }
        }
    }
}