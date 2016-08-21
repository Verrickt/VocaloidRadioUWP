using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VocaloidRadioUWP.Model;
using WebContentParser;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VocaloidRadioUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvisibleWebViewPage : Page
    {
        private bool playlistLoaded = false;
        private bool serverHighLoaded = false;
        private bool serverLowLoaded = false;


        public async Task<List<SongInfo>> GetSongInfos()
        {
           


            List<SongInfo> songInfos = new List<SongInfo>(5);
            string content=String.Empty;
            int count = 0;
            do
            {
                if (playlistLoaded)
                {
                    content =

                      await
                          PlaylistWebView.InvokeScriptAsync("eval",
                              new string[] { "document.documentElement.outerHTML;" });

                    count++;
                    if (count == 5)
                    {
                        break;
                    }


                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                  
            } while (!HTMLParser.TryGetSongInfos(content, out songInfos));
            if (songInfos.Count!=5)
            {
                throw new RequestTimeoutException("The page didn't load in 5 seconds");
            }
            return songInfos;
        }

        public async Task<List<ServerInfo>> GetServerInfos()
        {
            List<ServerInfo> serverInfos = new List<ServerInfo>(5);
            string[] strings=new string[2];
            int count = 0;
            do
            {
                if (serverLowLoaded&&serverHighLoaded)
                {

                    var myList = new List<IAsyncOperation<string>>(2)
                {
                    HighQualityServerWebView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" }),
                    LowQualityServerWebView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" }),
                };

                    strings = await Task.WhenAll(myList.Select(x => x.AsTask()));


                    count++;
                    if (count == 5)
                    {
                        break;
                    }


                }
               
                await Task.Delay(TimeSpan.FromSeconds(1));
            } while (!HTMLParser.TryGetServerInfos(strings[0],strings[1], out serverInfos));
            if (serverInfos.Count!=2)
            {
                throw new RequestTimeoutException("The page didn't load in 5 seconds");
            }

            return serverInfos;
        }


        public InvisibleWebViewPage()
        {
            this.InitializeComponent();
            PlaylistWebView.Source = new Uri("ms-appx-web:///HTMLCode/PreviousSongs_Larger.html");
            LowQualityServerWebView.Source = new Uri("ms-appx-web:///HTMLCode/SongInformation_128.html");
            HighQualityServerWebView.Source = new Uri("ms-appx-web:///HTMLCode/SongInformation_192.html");
        }


        private void InvisibleWebViewPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        private void PlaylistWebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            playlistLoaded=args.IsSuccess;
        }

        private void HighQualityServerWebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            serverHighLoaded = args.IsSuccess;
        }

        private void LowQualityServerWebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            serverLowLoaded = args.IsSuccess;
        }
    }
}
