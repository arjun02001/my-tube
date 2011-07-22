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
using Microsoft.Win32;
using System.IO;

namespace MyTube
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Window
    {
        Video video = new Video();
        string scrapedata = string.Empty;
        public string FilePath = string.Empty;
        WebClient client = new WebClient();
        bool allowsaving = false;

        public Download(Video video)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                TitleTextBlock.Text = video.Title + ".flv";
                this.Closing += new System.ComponentModel.CancelEventHandler(Download_Closing);
                scrapedata = Utility.ScrapeURL(Utility.FixURL(video.VideoURL));
                if (scrapedata.IndexOf("Error:") > 0)
                {
                    return;
                }
                FilePath = ShowSaveFileDialog();
                if (!string.IsNullOrEmpty(FilePath))
                {
                    allowsaving = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/Download\n" + ex.Message);
            }
        }

        private string ShowSaveFileDialog()
        {
            string filename = string.Empty;
            try
            {
                SaveFileDialog savefiledialog = new SaveFileDialog();
                savefiledialog.FileName = video.Title;
                savefiledialog.DefaultExt = ".flv";
                savefiledialog.Filter = "Flash files (.flv) | *.flv";
                if (savefiledialog.ShowDialog() == true)
                {
                    filename = savefiledialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/ShowSaveFileDialog\n" + ex.Message);
            }
            return filename;
        }

        void Download_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            client.CancelAsync();
        }

        public void ProcessScrapeData()
        {
            try
            {
                if (!allowsaving)
                {
                    this.Close();
                }
                string serverdata = Utility.GetServerURL(scrapedata);
                int separator = serverdata.IndexOf("?");
                string serverurl = serverdata.Substring(0, separator).Replace("?", "");
                NameValueCollection collection = HttpUtility.ParseQueryString(serverdata.Substring(separator));
                client.QueryString = collection;
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileAsync(new Uri(serverurl), FilePath);
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
