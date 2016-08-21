using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace VocaloidRadioUWP.Util
{
    class BackgroundConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
                if (ApplicationTheme.Light==Application.Current.RequestedTheme)
                {
                    return new SolidColorBrush(Colors.DimGray);
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
