using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMSMobile.Models
{
    [AddINotifyPropertyChangedInterface]
    
    public class AIModel
    {
        private Boolean isBusy { get; set; }
        public Boolean IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }

        public AIModel()
        {
            isBusy = false;
        }
    }
}
