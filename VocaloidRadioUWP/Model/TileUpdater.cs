using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using WebContentParser;

namespace VocaloidRadioUWP.Model
{
    

     public static class XmlManager
     {
         private static string _tileMediumFormat = @" <binding template=""TileMedium"" hint-overlay=""20"" displayname=""V-Radio"">
   <image placement=""peek"" src=""{0}"" />

      <text hint-style=""caption"">{1}</text>
      <text  hint-wrap=""true"" hint-style=""captionSubtle"">{2}</text>
      <text  hint-style=""captionSubtle"">{3}</text>
    </binding>
";
         private static string _trackGroupSmallFormat = @"<group>
       
        <subgroup hint-weight=""{0}"">
          <image src=""{1}"" hint-removeMargin = ""false"" />
        </subgroup>
        <subgroup>
          <text hint-style=""caption"">{2}</text>
          <text hint-wrap=""true"" hint-style=""captionSubtle"">{3}</text>
          <text hint-style=""captionSubtle"">{4}</text>
        </subgroup>
      </group>
";
         

        private static string TileMediumXml(SongInfo nowPlaying)
         {
             return string.Format(_tileMediumFormat, nowPlaying.AlbumUri.ToString(), nowPlaying.Title, nowPlaying.Artist,
                 nowPlaying.Album);
         }

         private static string SongGroupXml(int weight,SongInfo songInfo)
         {
             return string.Format(_trackGroupSmallFormat, weight,songInfo.AlbumUri.ToString(), songInfo.Title, songInfo.Artist,
                 songInfo.Album);
         }

         private static string TileLargeXml(List<SongInfo> songInfos)
         {
             string tileLargeFormat = @"<binding template=""TileLarge"">{0}{1}{2}</binding>";
             return string.Format(tileLargeFormat, SongGroupXml(40, songInfos[0]), SongGroupXml(20, songInfos[1]),
                 SongGroupXml(20, songInfos[2]));
         }
         
         private static string TileWideXml(SongInfo songinfo)
         {
             string tileWideFormat = @"<binding template=""TileWide"">
<image src=""Assets\Square310x310Logo.scale-100.png"" placement=""peek""/>
{0}</binding>";
             return string.Format(tileWideFormat, SongGroupXml(40, songinfo));
         }

         public static void UpdateTile(List<SongInfo> songInfos)
         {
             string tileFormat = @"<tile>
<visual branding=""name"">
{0}{1}{2}
</visual>
</tile>";
             string tileXml =  string.Format(tileFormat, TileMediumXml(songInfos[0]), TileWideXml(songInfos[0]),
                 TileLargeXml(songInfos));
             XmlDocument tileDocument = new XmlDocument();
            tileDocument.LoadXml(tileXml);
             Windows.UI.Notifications.TileNotification notification = new TileNotification(tileDocument);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
                
        }
    }
}
