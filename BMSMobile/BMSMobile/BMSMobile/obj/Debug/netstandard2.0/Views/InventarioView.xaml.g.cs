//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("BMSMobile.Views.InventarioView.xaml", "Views/InventarioView.xaml", typeof(global::BMSMobile.Views.InventarioView))]

namespace BMSMobile.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\InventarioView.xaml")]
    public partial class InventarioView : global::Xamarin.Forms.TabbedPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::BMSMobile.Custom.SelectableEntry txtCantUC;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Stepper _stepperUC;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::BMSMobile.Custom.SelectableEntry txtCantUA;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Stepper _stepperUA;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.CollectionView CollectionProds;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.CollectionView CollectionDifProds;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(InventarioView));
            txtCantUC = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::BMSMobile.Custom.SelectableEntry>(this, "txtCantUC");
            _stepperUC = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Stepper>(this, "_stepperUC");
            txtCantUA = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::BMSMobile.Custom.SelectableEntry>(this, "txtCantUA");
            _stepperUA = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Stepper>(this, "_stepperUA");
            CollectionProds = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.CollectionView>(this, "CollectionProds");
            CollectionDifProds = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.CollectionView>(this, "CollectionDifProds");
        }
    }
}