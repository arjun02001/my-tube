//Arjun Mukherji - Rights to distribute and modify granted.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using System.Net;
using System.Web;

namespace MyTube.Classes
{
    class Utility
    {
        static Random rand = new Random(50);
        //variable to hold the starting index for search query. eg results 11-20
        public static int StartIndex = 1;

        /// <summary>
        /// Takes a search string, makes a call to youtube and returns the videos found.
        /// </summary>
        /// <param name="searchstring"></param>
        /// <returns></returns>
        public static List<Video> GetVideos(string searchstring)
        {
            XNamespace media = "http://search.yahoo.com/mrss/";
            XNamespace yt = "http://gdata.youtube.com/schemas/2007";
            List<Video> videos = new List<Video>();
            int rank = StartIndex;
            try
            {
                //call to the api
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

        public static int GetRandom(double limit, int anglemutiplier)
        {
            return (int)((rand.NextDouble() * limit) * anglemutiplier);
        }

        public static double GetRandomDist(double limit)
        {
            return rand.NextDouble() * limit;
        }

        /// <summary>
        /// Takes a youtube url and fixes it.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FixURL(string url)
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility/FixURL\n" + ex.Message);
            }
            return url;
        }

        /// <summary>
        /// Takes an url and returns the view source
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ScrapeURL(string url)
        {
            try
            {
                return new WebClient().DownloadString(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility/ScrapeURL\n" + ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Takes a youtube video view source and extracts the download server url
        /// </summary>
        /// <param name="scrapedata"></param>
        /// <returns></returns>
        public static string GetServerURL(string scrapedata)
        {
            try
            {
                int start = scrapedata.IndexOf("vorbi");
                int end = scrapedata.IndexOf("flv", start) + 3;
                scrapedata = scrapedata.Substring(start, end - start);
                start = scrapedata.LastIndexOf("http");
                end = scrapedata.LastIndexOf("flv") + 3;
                scrapedata = scrapedata.Substring(start, end - start);
                return HttpUtility.UrlDecode(Uri.UnescapeDataString(scrapedata));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility/GetServerURL\n" + ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// Takes duration in secs and returns in the form hh:mm:ss
        /// </summary>
        /// <param name="durationinseconds"></param>
        /// <returns></returns>
        public static string GetDuration(string durationinseconds)
        {
            string duration = string.Empty;
            try
            {
                int seconds = int.Parse(durationinseconds);
                int hr = seconds / 3600;
                seconds = seconds % 3600;
                int min = seconds / 60;
                seconds = seconds % 60;
                int sec = seconds;
                if (hr != 0)
                {
                    duration += hr.ToString() + ":";
                }
                if (hr != 0 && min == 0)
                {
                    duration += "00:";
                }
                else if (min != 0)
                {
                    if (min <= 9)
                    {
                        duration += "0" + min.ToString() + ":";
                    }
                    else
                    {
                        duration += min.ToString() + ":";
                    }
                }
                if (sec <= 9)
                {
                    duration += "0" + sec.ToString();
                }
                else
                {
                    duration += sec.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utility/GetDuration\n" + ex.Message);
            }
            return duration;
        }
    }
}
