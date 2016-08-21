using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace VocaloidRadioUWP.Model
{
    public class MediaManager
    {
        private static MediaPlayer _player;

        public static MediaPlayer InstancePlayer
        {
            get
            {
                if (_player==null)
                {
                    _player = new MediaPlayer(); 
                       
                }
                return _player;
            }
        }


    }
}
