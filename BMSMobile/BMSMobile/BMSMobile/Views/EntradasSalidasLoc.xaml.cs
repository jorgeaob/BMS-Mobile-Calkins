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
    public partial class EntradasSalidasLoc : TabbedPage
    {
        public EntradasSalidasLoc()
        {
            InitializeComponent();
            BindingContext = new EntradasSalidasLocVM(Navigation,this);
        }
    }
}