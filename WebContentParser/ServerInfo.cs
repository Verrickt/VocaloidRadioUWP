using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebContentParser
{
    [DataContract]
    public class ServerInfo
    {
        [DataMember]
        public string ServerStatus { get; set; }
        [DataMember]

        public string CurrentListeners { get; set; }
        [DataMember]

        public string Bitrate { get; set; }
        public Uri StreamUri
            =>
                Bitrate == "128Kbps"
                    ? new Uri("http://curiosity.shoutca.st:8019/128")
                    : new Uri("http://curiosity.shoutca.st:8019/stream");

        public override string ToString()
        {
            return
                $"ServerInfo[{ServerStatus} {CurrentListeners} {Bitrate} StreamUri:{StreamUri}]";
        }
    }
}
