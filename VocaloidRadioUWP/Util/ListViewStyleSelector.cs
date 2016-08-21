using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VocaloidRadioUWP.Util
{
    public class ListViewStyleSelector:StyleSelector
    {
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            ListView listview = ItemsControl.ItemsControlFromItemContainer(container) as ListView;
            int i = listview.IndexFromContainer(container);
            if (i%2==1)
            {
                var style =  Application.Current.Resources["ListViewTransparent"] as Style;
                return style;
            }
            else
            {
                if (Application.Current.RequestedTheme==ApplicationTheme.Dark)
                {
                    var style = Application.Current.Resources["ListViewDark"] as Style;
                    return style;
                }
                else
                {
                    var style = Application.Current.Resources["ListViewLight"] as Style;
                    return style;
                }
                
            }
        }
    }
}
