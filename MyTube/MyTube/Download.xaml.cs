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
using System.Windows.Shapes;
using MyTube.Classes;

namespace MyTube
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Window
    {
        Video video = new Video();

        public Download(Video video)
        {
            InitializeComponent();
            try
            {
                this.video = video;
                string url = Utility.FixURL(video.VideoURL);
                MessageBox.Show("Under Construction");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download/Download\n" + ex.Message);
            }
        }
    }
}
