//Arjun Mukherji - Rights to distribute and modify granted.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using MyTube.Classes;

namespace MyTube.UserControls
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl
    {
        Video video = new Video();

        public delegate void VideoSelectedHandler(Video video);
        public event VideoSelectedHandler VideoSelected;

        /// <summary>
        /// Assign the meta data to this control
        /// </summary>
        /// <param name="video"></param>
        public SearchResult(Video video)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                ThumbNailImage.Source = new BitmapImage(new Uri(video.ThumbNailURL, UriKind.RelativeOrAbsolute));
                RankTextBlock.Text = video.Rank.ToString();
                TitleTextBlock.Text = video.Title + " " + Utility.GetDuration(video.Duration);
                this.MouseEnter += new MouseEventHandler(SearchResult_MouseEnter);
                this.MouseLeave += new MouseEventHandler(SearchResult_MouseLeave);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SearchResult/SearchResult\n" + ex.Message);
            }
        }

        void SearchResult_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Storyboard storyboard = (Storyboard)this.TryFindResource("OnMouseLeave");
                storyboard.Begin(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SearchResult/MouseLeave\n" + ex.Message);
            }
        }

        void SearchResult_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Storyboard storyboard = (Storyboard)this.TryFindResource("OnMouseEnter");
                storyboard.Begin(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SearchResult/MouseEnter\n" + ex.Message);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoSelected != null)
            {
                VideoSelected(video);
            }
        }
    }
}
