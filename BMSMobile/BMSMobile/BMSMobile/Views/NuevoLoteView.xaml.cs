using BMSMobile.Models;
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
    public partial class NuevoLoteView : TabbedPage
    {
        public NuevoLoteView()
        {
            InitializeComponent();
            BindingContext = new NuevoLoteVM(Navigation,this);
        }
    }
}