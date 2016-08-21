using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using VocaloidRadioShared;
using VocaloidRadioUWP.Model;
using VocaloidRadioUWP.View;
using WebContentParser;

namespace VocaloidRadioUWP.ViewModel
{
    class StreamPageViewModel : ViewModelBase
    {
        #region private members

        private SongInfo _nowPlaying=new SongInfo();
        private ObservableCollection<SongInfo> _observable = new ObservableCollection<SongInfo>();
        private bool _cantConnect = false;
        private ServerInfo _currentServer=new ServerInfo();
        private DispatcherTimer _timer = new DispatcherTimer();
        private ListWithMaximumSize<SongInfo> _cache;
        private string _message;
        private bool _notFullyInitialized = true;
        private bool _finishedFetchCache = false;
        private AudioPlayer _player = AudioPlayer.InstancePlayer;
        private SemaphoreSlim sync = new SemaphoreSlim(1);
        private MediaPlaybackState _playbackStatus;
        private bool _initialized = false;
        #endregion

        #region Properties for data binding

        public InvisibleWebViewPage ViewPage { get; set; }


        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public bool NotFullyInitialized
        {
            get { return _notFullyInitialized; }
            set { _notFullyInitialized = value; OnPropertyChanged(); }
        }

        public ServerInfo CurrentServer
        {
            get { return _currentServer; }
            set
            {
                _currentServer = value;
                OnPropertyChanged();
            }
        }

        public SongInfo NowPlaying
        {
            get { return _nowPlaying; }
            set
            {
                _nowPlaying = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SongInfo> Observable
        {
            get { return _observable; }
            set { _observable = value; }
        }

        public bool CantConnect
        {
            get { return _cantConnect; }
            set
            {
                _cantConnect = value;
                OnPropertyChanged();
            }
        }

        public bool FinishedFetchCache
        {
            get { return _finishedFetchCache; }
            set { _finishedFetchCache = value; OnPropertyChanged(); }
        }

        public MediaPlaybackState PlaybackStatus
        {
            get { return _playbackStatus; }
            private set
            {
                _playbackStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public StreamPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                SongInfo songInfo = new SongInfo()
                {
                    Album = "L4D2 Album",
                    AlbumUri = new Uri("ms-appx:///Assets/Album.png")
                    ,
                    LastPlayed = DateTimeOffset.Now,
                    Artist = "Various",
                    Title = "Midnightride"
                };

                NowPlaying = songInfo;
                Observable = new ObservableCollection<SongInfo>()
                {
                    songInfo,
                    songInfo,
                    songInfo
                };
                Application.Current.RequestedTheme = ApplicationTheme.Dark;
                CantConnect = false;
                FinishedFetchCache = true;
                NotFullyInitialized = false;
                CurrentServer = new ServerInfo()
                {
                    Bitrate = "192Kbps",
                    CurrentListeners = "40",
                    ServerStatus = "Online"
                };
            }
        }

        private async Task ReadCacheAsync()
        {
            Message = "Reading from cache";
            var serverInfo =
                await CacheHelper.ReadCacheAsync<List<ServerInfo>>(CacheConstants.ServerInfo);
            _cache =
                await
                    CacheHelper.ReadCacheAsync<ListWithMaximumSize<SongInfo>>(
                        CacheConstants.SongInfo);
            if (_cache != null && serverInfo != null)
            {
                Uri properUri = GetProperUri();
                var properServer = serverInfo.Single(x => x.StreamUri == properUri);
                UpdatePropertys(properServer);
            }
            if (_cache == null)
            {
                _cache = new ListWithMaximumSize<SongInfo>();
            }
        }

        private void UpdatePropertys(ServerInfo serverInfo)
        {
            if (CurrentServer.Bitrate != serverInfo.Bitrate)
            {
                CurrentServer = serverInfo;
                _player.SetStreamSource(CurrentServer);

            }
            if (_observable.Count != 0)
            {
                if (_observable.First().ToString() == _cache.First().ToString())
                {
                    return;
                }
            }
            
            NowPlaying = _cache.First();
            Observable.Clear();
            _cache.ToList().ForEach(x => Observable.Add(x));

            CantConnect = false;
            _player.UpdateUVC(NowPlaying);
            var list = Observable.Take(3).ToList();
            XmlManager.UpdateTile(list);
        }

        public async Task UpdateCacheAsync()
        {
            Debug.WriteLine("Entering Critical area");
            await sync.WaitAsync();

            try
            {
                Debug.WriteLine("Successfully acquire lock.Now performing update logic");
                var serverInfo = await ViewPage.GetServerInfos();
                var songInfos = await ViewPage.GetSongInfos();

                Uri properUri = GetProperUri();
                var properServer = serverInfo.Single(x => x.StreamUri == properUri);
                _cache.AddRangeWithDistinct(songInfos);
                await CacheHelper.SaveResetCacheAsync(CacheConstants.SongInfo, _cache);
                await CacheHelper.SaveResetCacheAsync(CacheConstants.ServerInfo, serverInfo);
                UpdatePropertys(properServer);
            }
            catch (RequestTimeoutException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            finally
            {
                sync.Release();
                Debug.WriteLine("Successfully released lock.Leaving Critical area");
            }
        }

        private Uri GetProperUri()
        {
            string _uriString = String.Empty;
            try
            {
                ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile.IsWlanConnectionProfile)
                {
                    _uriString =
                        SettingHelper.ReadSettingsValue(SettingConstants.WLANPrefer) as string;

                }
                else
                {
                    _uriString =
                        SettingHelper.ReadSettingsValue(SettingConstants.CellularPrefer) as
                            string;
                }

            }
            catch (Exception e)
            {
                _uriString =
                        SettingHelper.ReadSettingsValue(SettingConstants.CellularPrefer) as
                        string;
                Debug.WriteLine(e.ToString());
            }
            return new Uri(_uriString);
        }




        public async Task InitAsync()
        {
            if (_initialized)
            {
                return;
            }
            NotFullyInitialized = true;
            CantConnect = false;

            await ReadCacheAsync();
            if (!_cache.IsEmpty)
            {
                FinishedFetchCache = true;
            }
            Message = "Communicating with server";
            int i = 0;
            bool lasttry = false;
            for (i = 0; i < 4; i++)
            {

                try
                {
                    await UpdateCacheAsync();
                    Message = "";
                    NotFullyInitialized = false;
                    FinishedFetchCache = true;
                    lasttry = true;
                    break;

                }
                catch (RequestTimeoutException e)
                {
                    Message = "It's taking longer than expected";
                    lasttry = false;
                    Debug.WriteLine(e);
                }

            }
            if (!lasttry)
            {
                Message = "Can't communicate with server. Check your connection";
                CantConnect = true;
                return;
            }
            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Tick += _timer_Tick;
            Debug.WriteLine("Initialization successed.Timer is now started");
            _initialized = true;
            _timer.Start();
            _player.Init();
        }


        private async void _timer_Tick(object sender, object e)
        {
            try
            {
                await UpdateCacheAsync();
            }
            catch (RequestTimeoutException ex)
            {
                Debug.Write(ex.ToString());
            }
        }


        public void Uninit()
        {
            Debug.WriteLine("Timer is now off");
            _timer.Tick -= _timer_Tick;
            _timer.Stop();
            _player.UnInit();
        }
    }
}
