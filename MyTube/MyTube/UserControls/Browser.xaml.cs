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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyTube.Classes;

namespace MyTube.UserControls
{
    /// <summary>
    /// Interaction logic for Browser.xaml
    /// </summary>
    public partial class Browser : UserControl
    {
        Video video = new Video();

        public delegate void BrowserClosedHandler(Browser browser);
        public event BrowserClosedHandler BrowserClosed;

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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
