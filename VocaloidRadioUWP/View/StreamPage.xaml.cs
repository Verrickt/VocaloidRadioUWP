using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using VocaloidRadioShared;
using VocaloidRadioUWP.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VocaloidRadioUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StreamPage : Page
    {
        public StreamPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                if (Application.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    statusBar.ForegroundColor = Windows.UI.Colors.Black;

                }
            }
        }




       
        


        


        

       

        private void RetryButton_OnClick(object sender, RoutedEventArgs e)
        {
#pragma warning disable 4014
             VM.InitAsync();
#pragma warning restore 4014
        }

        

   

        private void StreamPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            VM.Uninit();
            VM = null;
            GC.Collect();
        }

        private void StreamPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (VM==null)
            {
                VM = new StreamPageViewModel {ViewPage = WebViewPage};
            }
            VM.ViewPage = WebViewPage;
#pragma warning disable 4014
            VM.InitAsync();
#pragma warning restore 4014

        }


        
    }





}

