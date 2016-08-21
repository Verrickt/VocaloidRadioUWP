using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebContentParser
{
    [DataContract]
    public class SongInfo:IComparable<SongInfo>,IEquatable<SongInfo>
    {
        
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public Uri AlbumUri { get; set; }

        [DataMember]
        public string TimePlayed
        {
            get { return LastPlayed.ToLocalTime().ToString($"HH:mm"); }
            set { }
        }

        [DataMember]
        public string Album { get; set; }
        [DataMember]
        public DateTimeOffset LastPlayed { get; set; }

        public int CompareTo(SongInfo other)
        {
            return -LastPlayed.CompareTo(other.LastPlayed);
        }

        public bool Equals(SongInfo other)
        {
            return this.ToString() == other.ToString();
        }

        public override string ToString()
        {
            return $"SongInfo:[Title:{Title} Artist:{Artist} AlbumUri:{AlbumUri} TimePlayed:{TimePlayed} Album:{Album}]";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.ToString());
        }
    }
}
