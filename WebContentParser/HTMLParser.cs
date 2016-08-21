using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;

namespace WebContentParser
{
    public class HTMLParser
    {
        private static string[] songinfo_patterns = new[]
        {
              @"<img src="".*?"".*?>|<img.*?src="".*?"">",
             @"<div class=""cctime.*?"">.*?<\/div>",
             @"<div class=""cctitle.*?"">.*?<\/div>",
             @"<div class=""ccartist"">.*?<\/div>",
             @"<div class=""ccalbum"">.*?<\/div>"
    };

        private static string[] serverinfo_patterns = new[] {  @"Current listeners\w*?.*?<.*?>.*>",
            @"Server\w*?.*?<.*?>.*>"};
        public static bool TryGetServerInfos(string highQuality, string lowQuality, out List<ServerInfo> serverInfos)
        {
            serverInfos = new List<ServerInfo>();
            try
            {
                serverInfos.Add(SingleServerInfo(highQuality, 192));
                serverInfos.Add(SingleServerInfo(lowQuality, 128));
            }
            catch (Exception)
            {
                return false;
            }
            if (serverInfos.TrueForAll(x => x.ServerStatus != "Server "))
            {
                return true;

            }
            return false;
        }

        private static ServerInfo SingleServerInfo(string source, int bitrate)
        {
            List<Regex> regexes = new List<Regex>();
            List<Match> matches = new List<Match>();
            for (int i = 0; i < serverinfo_patterns.Length; i++)
            {
                regexes.Add(new Regex(serverinfo_patterns[i]));
            }

            regexes.ForEach(regex => matches.Add(regex.Match(source)));

            string listener = GetValueBetweenTags(matches[0].Value);
            string status = GetValueBetweenTags(matches[1].Value);
            return new ServerInfo() { CurrentListeners = "CurrentListener " + listener, ServerStatus = "Server " + status, Bitrate = bitrate + "Kbps" };
        }

        public static bool TryGetSongInfos(string source, out List<SongInfo> songInfos)
        {

            songInfos = new List<SongInfo>();

            List<Regex> regs = new List<Regex>();
            List<List<Match>> matches = new List<List<Match>>();

            for (int i = 0; i < songinfo_patterns.Length; i++)
            {
                regs.Add(new Regex(songinfo_patterns[i], RegexOptions.None));
            }


            foreach (Regex regex in regs)
            {
                var non_generic = (IEnumerable)(regex.Matches(source));
                matches.Add(non_generic.Cast<Match>().ToList());
            }

            if (matches.TrueForAll(x => x.Count >= 3))
            {
                var notfull = matches.Where(x => x.Count != 5);
                notfull.ToList().ForEach(_list =>
                {
                    while (_list.Count < 5)
                    {
                        _list.Add(Match.Empty);
                    }
                });


                for (int i = matches[0].Count - 1; i >= 0; i--)
                {
                    songInfos.Add(ParseSingle(match_img: matches[0][i].Value, match_time: matches[1][i].Value,
                        match_title: matches[2][i].Value, match_artist: matches[3][i].Value,
                        match_album: matches[4][i].Value))
                        ;
                }
                return true;
            }

            return false;

        }

        private static SongInfo ParseSingle(string match_time, string match_title, string match_artist, string match_album, string match_img)
        {

            string title = TrimBrackets(GetValueBetweenTags(match_title));
            string time = GetValueBetweenTags(match_time);
            string artist = GetValueBetweenTags(match_artist);
            string album = GetValueBetweenTags(match_album);
            string imgWithQuote = match_img.Substring(match_img.IndexOf("http", StringComparison.Ordinal));
            string img = imgWithQuote.Substring(0, imgWithQuote.Length - 2);

            var datetime = DateTimeOffset.Parse(time + " -04:00");

            return new SongInfo() { Title = title, AlbumUri = new Uri(img), Artist = artist, LastPlayed = datetime, Album = album };
        }

        private static string GetValueBetweenTags(string tag)
        {
            string value;
            try
            {
                value = tag.Substring(tag.IndexOf(">") + 1).Split('<').FirstOrDefault().Trim();
            }
            catch (Exception)
            {
                value = string.Empty;
            }
            return value;
        }

        public static string TrimBrackets(string toTrim)
        {
            int start = toTrim.IndexOf('【');
            int end = toTrim.IndexOf('】');
            if (start == -1 && end == -1)
            {
                return toTrim;
            }
            else if (start != -1 && end != -1)
            {
                return toTrim.Substring(0, start);
            }
            else if (start == -1)
            {
                return toTrim.Substring(end + 1);
            }
            else if (end == -1)
            {
                return toTrim.Substring(0, start);
            }
            return toTrim;
        }
    }
}
