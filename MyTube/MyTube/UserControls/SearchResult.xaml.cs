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
        string embedurl = string.Empty;
        string thumbnailurl = string.Empty;

        public SearchResult(Video video)
        {
            InitializeComponent();
            embedurl = video.EmbedURL;
            thumbnailurl = video.ThumbNailURL;
            ThumbNailImage.Source = new BitmapImage(new Uri(thumbnailurl, UriKind.RelativeOrAbsolute));
            this.Loaded += new RoutedEventHandler(SearchResult_Loaded);
            this.MouseEnter += new MouseEventHandler(SearchResult_MouseEnter);
            this.MouseLeave += new MouseEventHandler(SearchResult_MouseLeave);
        }

        void SearchResult_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.TryFindResource("OnMouseLeave");
            storyboard.Begin(this);
        }

        void SearchResult_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.TryFindResource("OnMouseEnter");
            storyboard.Begin(this);
        }

        void SearchResult_Loaded(object sender, RoutedEventArgs e)
        {
            ThumbNailImage.SetValue(DragCanvas.CanBeDraggedProperty, true);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
