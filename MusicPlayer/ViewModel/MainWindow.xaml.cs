using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Text.Json;
using System.Collections.Generic;
using MPlayWebAPI.Models.Data;
using System.Windows.Controls;

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {
        private MediaPlayer player = new MediaPlayer();
        private bool PlayOrNot = true;
        private bool dragStarted = false;
        private string name;
        private int id;

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

                name = audioFile.Tag.Title;
                GetText(name);
                GetComment(name);
            }
        }

        public async Task GetText(string name)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"https://localhost:7119/api/Music/{name}");
                TextSong.Text = await result.Content.ReadAsStringAsync();
            }
        }

        public async Task GetComment(string name, int page = 1)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"https://localhost:7119/api/Comment/{name}/{page}");
                var stream = await result.Content.ReadAsStreamAsync();
                List<Comment> comment = await JsonSerializer.DeserializeAsync<List<Comment>>(stream);
                foreach (Comment el in comment)
                {
                    Grid grid = new Grid();
                    grid.Style = (Style)Resources["Grid1"];
                    TextBlock textBlock = new TextBlock();
                    textBlock.Style = (Style)Resources["Text1"];
                    textBlock.Text = el.text;
                    grid.Children.Add(textBlock);
                    CommentGrid.Children.Add(grid);
                }
                id = comment.First().musicId;
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

        private async Task Button_Click_Async()
        {
            using (var client = new HttpClient())
            {
                HttpContent content = new StringContent(SendText.Text);

                var result = await client.PostAsync($"https://localhost:7119/api/Comment/{id}/{SendText.Text}", content); //body problem

                CommentGrid.Children.Clear();
                GetComment(name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click_Async();
        }
    }
}