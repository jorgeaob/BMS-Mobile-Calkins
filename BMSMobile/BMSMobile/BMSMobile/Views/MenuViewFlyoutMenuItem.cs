using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSMobile.Views
{
    public class MenuViewFlyoutMenuItem
    {
        public MenuViewFlyoutMenuItem()
        {
            TargetType = typeof(MenuViewFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}