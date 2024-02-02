using BMSMobile.Custom;
using BMSMobile.iOS.Custom;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SelectableEntry), typeof(SelectableEntryRenderer))]
namespace BMSMobile.iOS.Custom
{
    public class SelectableEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            var nativeTextField = (UITextField)Control;
            nativeTextField.EditingDidBegin += (object sender, EventArgs eIos) => {
                nativeTextField.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
            };
        }
    }
}