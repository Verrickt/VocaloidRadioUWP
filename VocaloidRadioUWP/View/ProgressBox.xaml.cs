using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VocaloidRadioUWP.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ProgressBox : UserControl
    {
        public ProgressBox()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //MediaPlayerElement.SetMediaPlayer(MediaManager.InstancePlayer);

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //MediaPlayerElement.Source = null;
            GC.Collect();
        }
    }
}
