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

        public SearchResult(Video video)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                ThumbNailImage.Source = new BitmapImage(new Uri(video.ThumbNailURL, UriKind.RelativeOrAbsolute));
                RankTextBlock.Text = video.Rank.ToString();
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
