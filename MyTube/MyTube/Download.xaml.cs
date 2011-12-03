//Arjun Mukherji - Rights to distribute and modify granted.
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MyTube.Classes;
using System.Collections.Specialized;
using System.Web;
using System.Net;
using Microsoft.Win32;
using System.IO;
using JDP;
using System.Text.RegularExpressions;

namespace MyTube
{
    public partial class Download : Window
    {
        Video video = new Video();
        string scrapedata = string.Empty, typeofdownload = string.Empty, flashfilepath = string.Empty, filepath = string.Empty;
        WebClient client = new WebClient();
        bool allowsaving = false;
        Dictionary<string, string> downloadformat = new Dictionary<string, string> { {Constants.AUDIO, ".mp3"}, {Constants.VIDEO, ".flv"} };

        /// <summary>
        /// Initialize download
        /// </summary>
        /// <param name="video"></param>
        /// <param name="typeofdownload">can be "audio" or "video"</param>
        public Download(Video video, string typeofdownload)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                this.typeofdownload = typeofdownload;
                TitleTextBlock.Text = video.Title + downloadformat[typeofdownload];
                this.Background = new ImageBrush(new BitmapImage(new Uri(video.ThumbNailURL, UriKind.RelativeOrAbsolute)));
                this.Closing += new System.ComponentModel.CancelEventHandler(Download_Closing);
                scrapedata = Utility.ScrapeURL(Utility.FixURL(video.VideoURL));
                if (scrapedata.IndexOf("Error:") > 0)
                {
                    return;
                }
                filepath = ShowSaveFileDialog();
                if (!string.IsNullOrEmpty(filepath))
                {
                    allowsaving = true;
                    //using random generator to support both audio and video download at the same time
                    flashfilepath = filepath.Substring(0, filepath.LastIndexOf(".")) + ((int)Utility.GetRandomDist(Constants.MAX_RESULTS)).ToString() + ".flv";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/Download\n" + ex.Message);
            }
        }

        /// <summary>
        /// Get file path from user
        /// </summary>
        /// <returns></returns>
        private string ShowSaveFileDialog()
        {
            string filename = string.Empty;
            try
            {
                SaveFileDialog savefiledialog = new SaveFileDialog();
                //remove invalid chars in filename
                Regex regex = new Regex(@"[\/?:*""><|]+");
                savefiledialog.FileName = regex.Replace(video.Title, "");
                savefiledialog.DefaultExt = downloadformat[typeofdownload];
                savefiledialog.Filter =  "(" + downloadformat[typeofdownload] + ") | *" + downloadformat[typeofdownload];
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
            client.Dispose();
        }

        /// <summary>
        /// Use web client to start downloading the file
        /// </summary>
        public void ProcessScrapeData()
        {
            try
            {
                if (!allowsaving)
                {
                    this.Close();
                    return;
                }
                string serverdata = Utility.GetServerURL(scrapedata);
                int separator = serverdata.IndexOf("?");
                string serverurl = serverdata.Substring(0, separator).Replace("?", "");
                NameValueCollection collection = HttpUtility.ParseQueryString(serverdata.Substring(separator));
                client.QueryString = collection;
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileAsync(new Uri(serverurl), flashfilepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/ProcessScrapeData\n" + ex.Message);
                client.Dispose();
                this.Close();
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressBar.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// If the flv was downloaded successfully, extract the audio / video from it. Otherwise delete the partially downloaded flv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            client.Dispose();
            if (!e.Cancelled)
            {
                if (typeofdownload.Equals(Constants.AUDIO))
                {
                    ExtractAudioVideo();
                }
                else
                {
                    DownloadTextBlock.Text = Constants.EXTRACTION_COMPLETE;
                }
            }
            else
            {
                File.Delete(flashfilepath);
            }
        }

        /// <summary>
        /// Extract avi or mp3 from flv
        /// </summary>
        private void ExtractAudioVideo()
        {
            try
            {
                using (FLVFile flvfile = new FLVFile(flashfilepath))
                {
                    flvfile.ExtractStreams(true, false, false, null);
                }
                File.Delete(flashfilepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/ExtractAudioVideo\n" + ex.Message);
            }
            DownloadTextBlock.Text = Constants.EXTRACTION_COMPLETE;
        }
    }
}
