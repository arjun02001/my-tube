//Arjun Mukherji - Rights to distribute and modify granted.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using MyTube.Classes;
using System.Timers;
using System.Collections.Generic;

namespace MyTube.UserControls
{
    public partial class SearchResult : UserControl
    {
        Video video = new Video();
        Timer timer = null;
        List<string> imageurls = new List<string>();
        int imageiterator = -1;

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
                if (!string.IsNullOrEmpty(video.StartImageURL))
                {
                    imageurls.Add(video.StartImageURL);
                }
                if (!string.IsNullOrEmpty(video.MiddleImageURL))
                {
                    imageurls.Add(video.MiddleImageURL);
                }
                if(!string.IsNullOrEmpty(video.EndImageURL))
                {
                    imageurls.Add(video.EndImageURL);
                }
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
                timer.Stop();
                timer.Dispose();
                timer = null;
                ThumbNailImage.Source = new BitmapImage(new Uri(video.ThumbNailURL, UriKind.RelativeOrAbsolute));
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
                timer = new Timer(1000);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SearchResult/MouseEnter\n" + ex.Message);
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                    delegate()
                    {
                        imageiterator = (imageiterator + 1) % imageurls.Count;
                        ThumbNailImage.Source = new BitmapImage(new Uri(imageurls[imageiterator], UriKind.RelativeOrAbsolute));
                    }));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Inform the event subscriber of the video that has been selected for playing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoSelected != null)
            {
                VideoSelected(video);
            }
        }

        private void TopPanel_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void TopPanel_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
