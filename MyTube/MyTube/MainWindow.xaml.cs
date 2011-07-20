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
using MyTube.UserControls;

namespace MyTube
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand = new Random(50);
        List<Video> videos = new List<Video>();

        public MainWindow()
        {
            InitializeComponent();
            searchTextBox.Focus();
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (string.IsNullOrEmpty(searchTextBox.Text))
                    {
                        return;
                    }
                    videos = Utility.GetVideos(searchTextBox.Text);
                    PopulateCanvas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/KeyDown\n" + ex.Message);
            }
        }

        private void PopulateCanvas()
        {
            try
            {
                ContentDragCanvas.Children.Clear();
                for (int i = 0; i < videos.Count; i++)
                {
                    SearchResult searchresult = new SearchResult(videos[i]);
                    searchresult.VideoSelected += new SearchResult.VideoSelectedHandler(searchresult_VideoSelected);
                    int angleMutiplier = i % 2 == 0 ? 1 : -1;
                    searchresult.RenderTransform = new RotateTransform { Angle = GetRandom(30, angleMutiplier) };
                    searchresult.SetValue(Canvas.LeftProperty, GetRandomDist(ContentDragCanvas.ActualWidth - 150.0));
                    searchresult.SetValue(Canvas.TopProperty, GetRandomDist(ContentDragCanvas.ActualHeight - 150.0));
                    ContentDragCanvas.Children.Add(searchresult);
                }
                //SetDragMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/PopulateCanvas\n" + ex.Message);
            }
        }

        //private void SetDragMode()
        //{
        //    foreach (SearchResult control in ContentDragCanvas.Children)
        //    {
        //        DragCanvas.SetCanBeDragged(control, false);
        //    }
        //}

        void searchresult_VideoSelected(string embedurl)
        {
            try
            {
                Browser browser = new Browser(embedurl);
                browser.SetValue(Canvas.LeftProperty, GetRandomDist(ContentDragCanvas.ActualWidth - 150.0));
                browser.SetValue(Canvas.TopProperty, GetRandomDist(ContentDragCanvas.ActualHeight - 150.0));
                ContentDragCanvas.Children.Add(browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/VideoSelected\n" + ex.Message);
            }
        }

        private int GetRandom(double limit, int angleMutiplier)
        {
            return (int)((rand.NextDouble() * limit) * angleMutiplier);
        }

        private double GetRandomDist(double limit)
        {
            return rand.NextDouble() * limit;
        }
    }
}
