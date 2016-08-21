using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using WebContentParser;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VocaloidRadioUWP.ViewModel;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VocaloidRadioUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            
            MyFrame.Navigate(typeof(View.StreamPage));

        }
        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = MyFrame;

            if (rootFrame != null && rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
        private void MyFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void HumburButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }


        private void NavigationListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (MySplitView.DisplayMode==SplitViewDisplayMode.CompactOverlay)
            {
                MySplitView.IsPaneOpen = false;
            }
            var clicked = e.ClickedItem as MenuItem;
            if (clicked != null)
            {
                MyFrame.Navigate(clicked.NavigationTarget);
                TitleTextBlock.Text = clicked.DisplayName;
            }
        }


        private void MainPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigated -= MyFrame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;


            GC.Collect();

        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigated += MyFrame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }
    }
}
