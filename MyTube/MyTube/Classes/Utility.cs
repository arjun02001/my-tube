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
        public static List<Video> GetVideos(string searchstring)
        {
            XNamespace media = "http://search.yahoo.com/mrss/";
            List<Video> videos = new List<Video>();
            try
            {
                XElement rss = XElement.Load(string.Format(Constants.SEARCH_URL, searchstring));
                videos = (from item in rss.Element("channel").Descendants("item")
                          select new Video
                          {
                              EmbedURL = item.Element("link").Value.Substring(0, item.Element("link").Value.IndexOf("&")).Replace("watch?v=", "embed/"),
                              ThumbNailURL = item.Element(media + "group").Element(media + "thumbnail").Attribute("url").Value
                          }).ToList<Video>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility/GetVideos\n" + ex.Message);
            }
            return videos;
        }
    }
}
