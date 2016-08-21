using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VocaloidRadioUWP.Util
{
    public class PlayerlistDataTemplateSelector:DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ListView listview  = ItemsControl.ItemsControlFromItemContainer(container) as ListView;
            double width = listview.ActualWidth;
            if (width>(Double)Application.Current.Resources["WideMinWidth"])
            {
                return Application.Current.Resources["WideListViewItemTemplate"] as DataTemplate;
            }
            else
            {
                return Application.Current.Resources["NarrowListViewItemTemplate"] as DataTemplate;

            }
        }
    }
}
