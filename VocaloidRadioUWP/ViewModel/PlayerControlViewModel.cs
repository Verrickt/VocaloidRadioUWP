using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace VocaloidRadioUWP.ViewModel
{
    using Model;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Windows.UI.Core;
    using Windows.UI.Popups;

    class PlayerControlViewModel : ViewModelBase
    {
        private MediaPlayer _player = MediaManager.InstancePlayer;
        private bool _isBuffering = false;
        private bool _isPlaying = false;
        private double _volume = 1;
        public CoreDispatcher Dispatcher;
        private AudioPlayer _audioPlayer = AudioPlayer.InstancePlayer;

        


        public bool IsBuffering { set { _isBuffering = value; OnPropertyChanged(); } get { return _isBuffering; } }
        public bool IsPlaying { set { _isPlaying = value; OnPropertyChanged(); } get { return _isPlaying; } }
        public double Volume
        {
            get { return _volume*100; }
            set
            {
                _volume = value/100d;
                _player.Volume = _volume;
                OnPropertyChanged();
            }
        }

        public void Play()
        {
            _isPlaying = true;
            _audioPlayer.Play();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Stop()
        {
            _audioPlayer.Stop();
        }

        public PlayerControlViewModel()
        {
            AddEventHandelers();
        }

        private void AddEventHandelers()
        {
            _player.MediaEnded += _player_MediaEnded;
            _player.MediaFailed += _player_MediaFailed;
            _player.PlaybackSession.BufferingStarted += PlaybackSession_BufferingStarted;
            _player.PlaybackSession.BufferingEnded += PlaybackSession_BufferingEnded;
        }



        private void PlaybackSession_BufferingEnded(MediaPlaybackSession sender, object args)
        {
            Debug.WriteLine("Buffering Ended");
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { IsBuffering = false; });
        }

        private void PlaybackSession_BufferingStarted(MediaPlaybackSession sender, object args)
        {
            Debug.WriteLine("Buffering Started");
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { IsBuffering = true; });

        }

        private void _player_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            Debug.WriteLine("MediaFailed");
            MessageDialog dialog = new MessageDialog("Seems we can't fetch stream content for you. Check you connection", "Oops!");
            dialog.Commands.Add(new UICommand("Got it"));
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,async ()=>await dialog.ShowAsync());
        }



        private void _player_MediaEnded(MediaPlayer sender, object args)
        {
            Debug.WriteLine("MediaEnded");
            _audioPlayer.Stop();
            _audioPlayer.Play();
        }

        public void RemoveEventHandlers()
        {
            _player.MediaEnded -= _player_MediaEnded;
            _player.MediaFailed -= _player_MediaFailed;
            _player.PlaybackSession.BufferingStarted -= PlaybackSession_BufferingStarted;
            _player.PlaybackSession.BufferingEnded -= PlaybackSession_BufferingEnded;
        }

    }
}
