using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace BMSMobile.Models
{
    [AddINotifyPropertyChangedInterface]

    public class ColorsModel
    {
        private Color _missingColor { get; set; }
        
        public Color MissingColor
        {
            get { return _missingColor; }
            set { _missingColor = value; }
        }

        public ColorsModel()
        {
            _missingColor = Color.Red;
        }

        public Color MissingValue()
        {
            _missingColor = Color.Red;
            return _missingColor;
        }

        public Color AdvertValue()
        {
            _missingColor = Color.Yellow;
            return _missingColor;
        }

        public Color CheckValue()
        {
            _missingColor = Color.FromArgb(248,248,255);
            return _missingColor;
        }

    }
}
