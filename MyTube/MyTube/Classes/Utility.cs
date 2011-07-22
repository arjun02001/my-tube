using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace MyTube.Classes
{
    class Utility
    {
        static Random rand = new Random(50);
        public static int StartIndex = 1;

        public static List<Video> GetVideos(string searchstring)
        {
            XNamespace media = "http://search.yahoo.com/mrss/";
            XNamespace yt = "http://gdata.youtube.com/schemas/2007";
            List<Video> videos = new List<Video>();
            int rank = StartIndex;
            try
            {
                XElement rss = XElement.Load(string.Format(Constants.SEARCH_URL, searchstring, Constants.MAX_RESULTS, StartIndex));
                videos = (from item in rss.Element("channel").Descendants("item")
                          select new Video
                          {
                              VideoURL = item.Element("link").Value.Substring(0, item.Element("link").Value.IndexOf("&")),
                              EmbedURL = item.Element("link").Value.Substring(0, item.Element("link").Value.IndexOf("&")).Replace("watch?v=", "embed/"),
                              ThumbNailURL = item.Element(media + "group").Element(media + "thumbnail").Attribute("url").Value,
                              Rank = rank++,
                              Title = item.Element("title").Value,
                              Duration = item.Element(media + "group").Element(yt + "duration").Attribute("seconds").Value
                          }).ToList<Video>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility/GetVideos\n" + ex.Message);
            }
            return videos;
        }

        public static int GetRandom(double limit, int angleMutiplier)
        {
            return (int)((rand.NextDouble() * limit) * angleMutiplier);
        }

        public static double GetRandomDist(double limit)
        {
            return rand.NextDouble() * limit;
        }

        public static string FixURL(string url)
        {
            url = url.Replace("www.youtube.com", "youtube.com");
            if (url.IndexOf("http://youtube.com/v/") >= 0)
            {
                url.Replace("http://youtube.com/v/", "http://youtube.com/watch?v=");
            }
            if (url.IndexOf("http://youtube.com/watch?v=") < 0)
            {
                url = string.Empty;
            }
            return url;
        }
    }
}
