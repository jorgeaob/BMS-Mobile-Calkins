using BMSMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMSMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new LoginView())
            {
                BarBackgroundColor = Color.FromHex("0D47A1")
                 
            };

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
