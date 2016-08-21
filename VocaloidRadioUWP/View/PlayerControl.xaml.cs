using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VocaloidRadioUWP.View
{
    public sealed partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            this.InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            VM.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            VM.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            VM.Stop();
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                VM = new ViewModel.PlayerControlViewModel();
            }
            VM.Dispatcher = this.Dispatcher;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            VM.RemoveEventHandlers();
            VM = null;
            GC.Collect();
        }

        private void UserControl_Loading(FrameworkElement sender, object args)
        {
            if (VM == null)
            {
                VM = new ViewModel.PlayerControlViewModel();
            }
            VM.Dispatcher = this.Dispatcher;

        }
    }
}
