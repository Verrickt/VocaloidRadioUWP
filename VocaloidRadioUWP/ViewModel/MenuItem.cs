using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocaloidRadioUWP.ViewModel
{

    public class MenuItem
    {
        public string DisplayName { get; set; }
        public string Glyph { get; set; }
        public Type NavigationTarget { get; set; }
    }
}
