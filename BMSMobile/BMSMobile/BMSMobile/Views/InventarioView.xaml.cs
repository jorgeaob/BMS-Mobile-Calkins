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
    public partial class InventarioView : TabbedPage
    {
        public InventarioView()
        {
            InitializeComponent();
            BindingContext = new InventarioVM(Navigation, this);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }        
    }
}