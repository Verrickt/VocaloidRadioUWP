using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VocaloidRadioShared;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VocaloidRadioUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private string[] _streamSource = new[]
        {
            "http://curiosity.shoutca.st:8019/128",
            "http://curiosity.shoutca.st:8019/stream"
        };


        private void Light_OnChecked(object sender, RoutedEventArgs e)
        {
            Tip.Visibility = Visibility.Visible;
            SettingHelper.SaveSettingsValue(SettingConstants.AppTheme,
                ApplicationTheme.Light.ToString());
        }

        private void Dark_OnChecked(object sender, RoutedEventArgs e)
        {
            Tip.Visibility = Visibility.Visible;

            SettingHelper.SaveSettingsValue(SettingConstants.AppTheme,
                ApplicationTheme.Dark.ToString());
        }

        private void System_OnChecked(object sender, RoutedEventArgs e)
        {
            Tip.Visibility = Visibility.Visible;
            SettingHelper.SaveSettingsValue(SettingConstants.AppTheme, null);
        }

        private void ThemeStackPanel_OnLoaded(object sender, RoutedEventArgs e)
        {
            var theme = SettingHelper.ReadSettingsValue(SettingConstants.AppTheme) as string;
            if (theme == null)
            {
                System.IsChecked = true;
            }

            else
            {
                var apptheme = EnumHelper.Parse<ApplicationTheme>(theme);
                if (apptheme == ApplicationTheme.Dark)
                {
                    Dark.IsChecked = true;
                }
                else
                {
                    Light.IsChecked = true;
                }
            }
        }

        private void WLANHigh_Checked(object sender, RoutedEventArgs e)
        {
            SettingHelper.SaveSettingsValue(SettingConstants.WLANPrefer, _streamSource[1]);
        }

        private void WLANLow_Checked(object sender, RoutedEventArgs e)
        {
            SettingHelper.SaveSettingsValue(SettingConstants.WLANPrefer, _streamSource[0]);
        }

        private void CellularHigh_Checked(object sender, RoutedEventArgs e)
        {
            SettingHelper.SaveSettingsValue(SettingConstants.CellularPrefer, _streamSource[1]);
        }

        private void CellularLow_Checked(object sender, RoutedEventArgs e)
        {
            SettingHelper.SaveSettingsValue(SettingConstants.CellularPrefer, _streamSource[0]);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var wlan = SettingHelper.ReadSettingsValue(SettingConstants.WLANPrefer) as string;
            var cellular =
                SettingHelper.ReadSettingsValue(SettingConstants.CellularPrefer) as string;

            if (wlan == _streamSource[0])
            {
                WLANLow.IsChecked = true;
            }
            else
            {
                WLANHigh.IsChecked = true;
            }
            if (cellular == _streamSource[0])
            {
                CellularLow.IsChecked = true;
            }
            else
            {
                CellularHigh.IsChecked = true;
            }
            Tip.Visibility = Visibility.Collapsed;
        }

        private void SettingPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }
    }
}
