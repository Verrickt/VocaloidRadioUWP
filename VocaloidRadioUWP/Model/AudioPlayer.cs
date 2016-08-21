using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Phone.Notification.Management;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using VocaloidRadioShared;
using VocaloidRadioUWP.View;
using WebContentParser;

namespace VocaloidRadioUWP.Model
{
    public class AudioPlayer
    {
        public static AudioPlayer InstancePlayer
        {
         get
            {
                if (_audioPlayer==null)
                {
                    _audioPlayer = new AudioPlayer();
                }
                return _audioPlayer;
            }   
        }

        private static AudioPlayer _audioPlayer;

        private MediaPlayer _player = MediaManager.InstancePlayer;

        private bool _initialized = false;

        private SystemMediaTransportControls _smtc => _player.SystemMediaTransportControls;

        private ServerInfo _serverInfo;


        public void Init()
        {
            if (!_initialized)
            {
                _player.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
                _player.AutoPlay = false;
                

                _smtc.IsStopEnabled = true;
                _smtc.IsPlayEnabled = true;
                _smtc.IsPauseEnabled = true;
                _smtc.IsEnabled = true;
                _smtc.IsChannelDownEnabled = false;
                _smtc.IsChannelUpEnabled = false;
                _smtc.IsPreviousEnabled = false;
                _smtc.IsRewindEnabled = false;
                _smtc.IsRecordEnabled = false;
                _smtc.IsFastForwardEnabled = false;
                _smtc.IsNextEnabled = false;
                _smtc.ButtonPressed += _smtc_ButtonPressed;
                _initialized = true; 
            }
        }

       

        public void UnInit()
        {
            
            _player.PlaybackSession.PlaybackStateChanged -= PlaybackSession_PlaybackStateChanged;
            _smtc.ButtonPressed -= _smtc_ButtonPressed;

        }

       

        private void _smtc_ButtonPressed(SystemMediaTransportControls sender,
            SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                {
                     Debug.WriteLine("Play pressed from UVC");
                        Play();
                }

                    break;
                case SystemMediaTransportControlsButton.Pause:
                {
                        Debug.WriteLine("Pause pressed from UVC");
                        _player.Pause();
                    }
                    break;
                case SystemMediaTransportControlsButton.Stop:
                {
                        Debug.WriteLine("Stop pressed from UVC");
                        Stop();

                    }
                    break;
                case SystemMediaTransportControlsButton.Record:
                    break;
                case SystemMediaTransportControlsButton.FastForward:
                    break;
                case SystemMediaTransportControlsButton.Rewind:
                    break;
                case SystemMediaTransportControlsButton.Next:
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    break;
                case SystemMediaTransportControlsButton.ChannelUp:
                    break;
                case SystemMediaTransportControlsButton.ChannelDown:
                    break;
                default:
                    break;
            }
        }

        private void PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            Debug.WriteLine("PlaybackSession State Changed to "+sender.PlaybackState);
            switch (sender.PlaybackState)
            {
                case MediaPlaybackState.None:
                {
                    _smtc.PlaybackStatus = MediaPlaybackStatus.Stopped;
                }
                    break;
                case MediaPlaybackState.Opening:
                {
                    _smtc.PlaybackStatus = MediaPlaybackStatus.Changing;
                }
                    break;
                case MediaPlaybackState.Buffering:
                {
                    _smtc.PlaybackStatus = MediaPlaybackStatus.Changing;
                }
                    break;
                case MediaPlaybackState.Playing:
                {
                    _smtc.PlaybackStatus = MediaPlaybackStatus.Playing;
                       
                    }
                    break;

                case MediaPlaybackState.Paused:
                {
                    _smtc.PlaybackStatus = MediaPlaybackStatus.Paused;
                    ;
                        
                }
                    break;

                default:
                {
                    _smtc.PlaybackStatus = MediaPlaybackStatus.Stopped;
                }
                    break;
            }
        }

        public void SetStreamSource(ServerInfo serverinfo)
        {
            _serverInfo = serverinfo;
            
        }

        public void Play()
        {
            if (_player.Source ==null)
            {
                _player.Source = MediaSource.CreateFromUri(_serverInfo.StreamUri);
            }
            _player.Play();
        }


        public void Stop()
        {
            _player.Pause();
            _player.Source = null;
        }

        public void UpdateUVC(SongInfo songinfo)
        {
            var updater = _smtc.DisplayUpdater;
            updater.AppMediaId = "Vocaloid Radio";
            updater.Type=MediaPlaybackType.Music;
            updater.MusicProperties.AlbumTitle = songinfo.Album;
            updater.MusicProperties.Artist = songinfo.Artist;
            updater.MusicProperties.Title = songinfo.Title;
            updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(songinfo.AlbumUri);
            updater.Update();
        }

        
    }
}
