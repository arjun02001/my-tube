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
        List<Video> videos = new List<Video>();
        string previoussearchstring = string.Empty, currentsearchstring = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            SearchTextBox.Focus();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Utility.StartIndex = 1;
                    SearchVideos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/KeyDown\n" + ex.Message);
            }
        }

        private void SearchVideos()
        {
            try
            {
                if (string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    return;
                }
                previoussearchstring = currentsearchstring;
                currentsearchstring = SearchTextBox.Text;
                if (!previoussearchstring.Equals(currentsearchstring))
                {
                    Utility.StartIndex = 1;
                }
                videos = Utility.GetVideos(SearchTextBox.Text);
                if (0 == videos.Count)
                {
                    MessageBox.Show("Your search yielded no results");
                    return;
                }
                PopulateCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/SearchVideos\n" + ex.Message);
            }
        }

        private void PopulateCanvas()
        {
            try
            {
                StopPlayingVideos();
                ContentDragCanvas.Children.Clear();
                for (int i = 0; i < videos.Count; i++)
                {
                    SearchResult searchresult = new SearchResult(videos[i]);
                    searchresult.VideoSelected += new SearchResult.VideoSelectedHandler(searchresult_VideoSelected);
                    int angleMutiplier = i % 2 == 0 ? 1 : -1;
                    searchresult.RenderTransform = new RotateTransform { Angle = Utility.GetRandom(30, angleMutiplier) };
                    AddUIElementToCanvas(searchresult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/PopulateCanvas\n" + ex.Message);
            }
        }

        private void StopPlayingVideos()
        {
            try
            {
                foreach (UIElement element in ContentDragCanvas.Children)
                {
                    if (element is Browser)
                    {
                        ((Browser)element).VideoBrowser.Source = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/StopPlayingVideos\n" + ex.Message);
            }
        }

        private void AddUIElementToCanvas(UIElement control)
        {
            try
            {
                control.SetValue(Canvas.LeftProperty, Utility.GetRandomDist(ContentDragCanvas.ActualWidth - 150.0));
                control.SetValue(Canvas.TopProperty, Utility.GetRandomDist(ContentDragCanvas.ActualHeight - 150.0));
                ContentDragCanvas.Children.Add(control);
                DragCanvas.SetCanBeDragged(control, !(bool)PlayModeCheckBox.IsChecked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/AddControlToCanvas\n" + ex.Message);
            }
        }

        private void SetDragMode(bool? value)
        {
            foreach (UIElement element in ContentDragCanvas.Children)
            {
                DragCanvas.SetCanBeDragged(element, (bool)value);
            }
        }

        void searchresult_VideoSelected(Video video)
        {
            try
            {
                Browser browser = new Browser(video);
                browser.BrowserClosed += new Browser.BrowserClosedHandler(browser_BrowserClosed);
                AddUIElementToCanvas(browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/VideoSelected\n" + ex.Message);
            }
        }

        void browser_BrowserClosed(Browser browser)
        {
            try
            {
                ContentDragCanvas.Children.Remove(browser);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/BrowserClosed\n" + ex.Message);
            }
        }

        private void PlayModeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetDragMode(!PlayModeCheckBox.IsChecked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/CheckBoxClick\n" + ex.Message);
            }
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    return;
                }
                if (((Button)sender).Tag.ToString().Equals(Constants.PREVIOUS))
                {
                    if (Utility.StartIndex < (Constants.MAX_RESULTS + 1))
                    {
                        return;
                    }
                    Utility.StartIndex -= Constants.MAX_RESULTS;
                }
                else
                {
                    Utility.StartIndex += Constants.MAX_RESULTS;
                }
                SearchVideos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/NavigationButton\n" + ex.Message);
            }
        }
    }
}
