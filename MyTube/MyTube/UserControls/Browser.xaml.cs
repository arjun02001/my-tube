//Arjun Mukherji - Rights to distribute and modify granted.
using System;
using System.Windows;
using System.Windows.Controls;
using MyTube.Classes;

namespace MyTube.UserControls
{
    public partial class Browser : UserControl
    {
        Video video = new Video();
        string url = string.Empty;

        public delegate void BrowserClosedHandler(Browser browser);
        public event BrowserClosedHandler BrowserClosed;

        /// <summary>
        /// Assign the url as source to the web-browser
        /// </summary>
        /// <param name="video"></param>
        public Browser(Video video)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                VideoBrowser.Source = new Uri(video.EmbedURL, UriKind.RelativeOrAbsolute);
                RankTextBlock.Text = video.Rank.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Browser/Browser\n" + ex.Message);
            }
        }

        /// <summary>
        /// Inform the subscriber that this browser window is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            VideoBrowser.Source = null;
            try
            {
                if (BrowserClosed != null)
                {
                    BrowserClosed(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Browser/CloseButton\n" + ex.Message);
            }
        }

        /// <summary>
        /// Opens a download window, sends the video object, and the extraction type - avi or mp3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Download download = new Download(video, ((Button)sender).Tag.ToString());
                download.Show();
                download.ProcessScrapeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Browser/DownloadButton\n" + ex.Message);
            }
        }
    }
}
