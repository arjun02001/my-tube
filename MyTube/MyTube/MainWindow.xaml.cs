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
            SearchTextBox.Focus();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (string.IsNullOrEmpty(SearchTextBox.Text))
                    {
                        return;
                    }
                    videos = Utility.GetVideos(SearchTextBox.Text);
                    if (videos.Count == 0)
                    {
                        MessageBox.Show("Your search yielded no results");
                        return;
                    }
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
                    AddUIElementToCanvas(searchresult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MainWindow/PopulateCanvas\n" + ex.Message);
            }
        }

        private void AddUIElementToCanvas(UIElement control)
        {
            try
            {
                control.SetValue(Canvas.LeftProperty, GetRandomDist(ContentDragCanvas.ActualWidth - 150.0));
                control.SetValue(Canvas.TopProperty, GetRandomDist(ContentDragCanvas.ActualHeight - 150.0));
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

        private int GetRandom(double limit, int angleMutiplier)
        {
            return (int)((rand.NextDouble() * limit) * angleMutiplier);
        }

        private double GetRandomDist(double limit)
        {
            return rand.NextDouble() * limit;
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
    }
}
