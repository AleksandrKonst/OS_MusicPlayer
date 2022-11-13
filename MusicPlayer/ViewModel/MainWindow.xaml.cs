using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {
        private MediaPlayer player = new MediaPlayer();
        private bool PlayOrNot = true;
        private bool dragStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length > 1)
            {
                player.Open(new Uri(arguments[1], UriKind.Relative));
                player.Play();

                var audioFile = TagLib.File.Create(arguments[1]);             

                MusicName.Text = $"{audioFile.Tag.Title}";
                Slider_Music.Maximum = audioFile.Properties.Duration.TotalMilliseconds;

                if (audioFile.Tag.Pictures.Length > 0)
                {
                    TagLib.IPicture pic = audioFile.Tag.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data);
                    ms.Seek(0, SeekOrigin.Begin);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();

                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = bitmap;
                    ImageTrack.Source = img.Source;
                }
                
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
            }
        }

        private void timer_Tick(object sender, object e)
        {
            Slider_Music.Value = player.Position.TotalMilliseconds;
        }        

        private void cmd_Play_Click(object sender, RoutedEventArgs e)
        {
            if (PlayOrNot)
            {
                player.Pause();
                PlayButt.Content = FindResource("Stop");
                PlayOrNot = false;
            }
            else
            {
                player.Play();
                PlayButt.Content = FindResource("Play");
                PlayOrNot = true;
            }
        }

        private void cmd_Pause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }        

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {           
            this.dragStarted = false;
            player.Play();
            PlayOrNot = true;
            PlayButt.Content = FindResource("Play");
        }

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.dragStarted = true;           
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (dragStarted)
            {
                player.Pause();
                this.player.Position = TimeSpan.FromSeconds(Slider_Music.Value / 1000);
            }              
        }
    }
}