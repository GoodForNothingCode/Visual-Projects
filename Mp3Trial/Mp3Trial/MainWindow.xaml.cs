using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagLib.Id3v2;


namespace Mp3Trial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.AddExtension = true;
            openFD.DefaultExt = "*.*";
            openFD.Filter = "Media Files (*.*)|*.*";
            openFD.ShowDialog();    

            try { MusicElement.Source = new Uri(openFD.FileName); }
            catch { new NullReferenceException("Error"); }

            TagLib.File music = TagLib.File.Create(openFD.FileName);

            slideSeek.Maximum = music.Properties.Duration.TotalSeconds;

            //slideSeek.Maximum = MusicElement.NaturalDuration.TimeSpan.Seconds;
            System.Windows.Threading.DispatcherTimer musicTimer = new System.Windows.Threading.DispatcherTimer();
            musicTimer.Tick += musicTimer_Tick;
            //musicTimer.Interval = new TimeSpan(0, 0, 1);
            musicTimer.Start();


        }

        void musicTimer_Tick(object sender, EventArgs e)
        {
            slideSeek.Value = MusicElement.Position.Seconds;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            MusicElement.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            MusicElement.Stop();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            MusicElement.Pause();
        }

        private void slideSeek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan t = TimeSpan.FromSeconds(e.NewValue);
            MusicElement.Position = t;
        }

        private void slideVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MusicElement.Volume = slideVolume.Value;
        }

        private void MusicElement_MediaOpened_1(object sender, RoutedEventArgs e)
        {
            if (MusicElement.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(MusicElement.NaturalDuration.TimeSpan.TotalMilliseconds);
                slideSeek.Maximum = ts.Seconds;
            }
        }


    }
}
