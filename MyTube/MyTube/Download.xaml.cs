using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyTube.Classes;
using System.Collections.Specialized;
using System.Web;
using System.Net;

namespace MyTube
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Window
    {
        Video video = new Video();
        string scrapedata = string.Empty;

        public Download(Video video)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                TitleTextBlock.Text = video.Title + ".flv";
                scrapedata = Utility.ScrapeURL(Utility.FixURL(video.VideoURL));
                if (scrapedata.IndexOf("Error:") > 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/Download\n" + ex.Message);
            }
        }

        public void ProcessScrapeData()
        {
            try
            {
                string serverdata = Utility.GetServerURL(scrapedata);
                int separator = serverdata.IndexOf("?");
                string serverurl = serverdata.Substring(0, separator).Replace("?", "");
                NameValueCollection collection = HttpUtility.ParseQueryString(serverdata.Substring(separator));
                WebClient client = new WebClient();
                client.QueryString = collection;
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileAsync(new Uri(serverurl), video.Title + ".flv");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/ProcessScrapeData\n" + ex.Message);
                this.Close();
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressBar.Value = e.ProgressPercentage;
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadTextBlock.Text = Constants.DOWNLOAD_COMPLETE;
        }
    }
}
