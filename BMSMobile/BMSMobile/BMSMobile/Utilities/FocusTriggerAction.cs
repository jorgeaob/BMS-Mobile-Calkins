using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BMSMobile.Utilities
{
    [AddINotifyPropertyChangedInterface]
    public class FocusTriggerAction : TriggerAction<Entry>
    {
        public bool Focused { get; set; }

        protected override async void Invoke(Entry entry)
        {
            await Task.Delay(500);

            if (Focused)
                entry.Focus();
            else
                entry.Focus();
        }
    }
}
