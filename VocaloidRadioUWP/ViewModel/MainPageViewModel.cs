using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace VocaloidRadioUWP.ViewModel
{
    public class MainPageViewModel
    {
        public static List<MenuItem> MenuItems => new List<MenuItem>()
        {
            new MenuItem { DisplayName="Radio",Glyph="\uE7F6",NavigationTarget = typeof(View.StreamPage)},
            new MenuItem { DisplayName="Setting",Glyph="\uE115",NavigationTarget = typeof(View.SettingPage)},

        };


        public MainPageViewModel()
        {
          
        }
    }
}

