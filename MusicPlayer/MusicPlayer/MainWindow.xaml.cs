using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileSongs;
        private List<string> setSongs;
        private int countMax = 20, selectedSong = -1;
        private MediaPlayer mediaPlayer;
        private bool songPlay;
        private DispatcherTimer timer;

        public MainWindow()
        { 
            InitializeComponent();
            fileSongs = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\listSongs.txt"));
            setSongs = new List<string>();
            LoadSongs();
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += (o, e) => Button_ClickNext(null, null);
            timer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(0.3) };
            timer.Tick += Timer_Tick;
        }

        private void Button_ClickClose(object sender, RoutedEventArgs e)
        {
            SaveSongs();
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_ClickPlayPause(object sender, RoutedEventArgs e)
        {
            Button b = (sender as Button);

            if (b.Name.Equals("bPlay"))
            {
                bPause.Visibility = Visibility.Visible;
                if (mediaPlayer.Source != null)
                {
                    mediaPlayer.Play();
                    Timer_Tick(null, null);
                    timer.Start();
                    songPlay = true;
                }
            }
            else
            {
                bPlay.Visibility = Visibility.Visible;
                mediaPlayer.Pause();
                timer.Stop();
                songPlay = false;
            }

            b.Visibility = Visibility.Hidden;
        }

        private void Button_ClickAdd(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
            if (openFileDialog.ShowDialog() == true)
            {
                if (!setSongs.Contains(openFileDialog.FileName))
                {
                    if (setSongs.Count == countMax)
                    {
                        RemoveSong(0);
                    }
                    setSongs.Add(openFileDialog.FileName);
                    AddListViewItem(setSongs.Count.ToString(), GetNameSongFromPath(openFileDialog.FileName));
                }
            }
        }

        private void listViewSongs_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem lvi = ((sender as ListView)?.SelectedItem) as ListViewItem;
            if (lvi != null && lvi.Content is StackPanel sp && sp.Children.Count == 3 && sp.Children[2] is TextBlock tb)
            {
                selectedSong = listViewSongs.SelectedIndex;
                UpdateUI(tb.Text);
            }
        }

        private void Button_ClickShuffle(object sender, RoutedEventArgs e)
        {
            ShuffleListSongs();
        }

        private void Button_ClickNext(object sender, RoutedEventArgs e)
        {
            if(++selectedSong == setSongs.Count)
            {
                selectedSong = 0;
            }
            if (listViewSongs.Items[selectedSong] is ListViewItem lvi && lvi.Content is StackPanel sp && sp.Children.Count == 3 && sp.Children[2] is TextBlock tb)
            {
                listViewSongs.SelectedIndex = selectedSong;
                UpdateUI(tb.Text);
            }
        }

        private void Button_ClickPrevious(object sender, RoutedEventArgs e)
        {
            if (--selectedSong == -1)
            {
                selectedSong = setSongs.Count - 1;
            }
            if (listViewSongs.Items[selectedSong] is ListViewItem lvi && lvi.Content is StackPanel sp && sp.Children.Count == 3 && sp.Children[2] is TextBlock tb)
            {
                listViewSongs.SelectedIndex = selectedSong;
                UpdateUI(tb.Text);
            }
        }

        private void Button_ClickStepBackward(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Source == null)
                return;
            TimeSpan ts = new TimeSpan(0, mediaPlayer.Position.Minutes, mediaPlayer.Position.Seconds - 1);
            mediaPlayer.Position = ts;
            Timer_Tick(null, null);
        }

        private void Button_ClickStepForward(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Source == null)
                return;
            TimeSpan ts = new TimeSpan(0, mediaPlayer.Position.Minutes, mediaPlayer.Position.Seconds + 1);
            mediaPlayer.Position = ts;
            Timer_Tick(null, null);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            textSongTime.Text = mediaPlayer.Position.ToString("mm\\:ss");
        }

        private void UpdateUI(string text)
        {
            textSongName.Text = text;
            mediaPlayer.Open(new Uri(setSongs[selectedSong]));
            if (songPlay)
            {
                mediaPlayer.Play();
            }
        }

        private void LoadSongs()
        {
            try
            {   
                if (File.Exists(fileSongs))
                {
                    using (StreamReader sr = File.OpenText(fileSongs))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (File.Exists(s))
                            {
                                setSongs.Add(s);
                                AddListViewItem(setSongs.Count.ToString(), GetNameSongFromPath(s));
                            }
                        }
                    }
                }
                else
                {
                    using (FileStream fs = File.Create(fileSongs))
                    {
                    }
                }            
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void SaveSongs()
        {
            using (TextWriter tw = new StreamWriter(fileSongs))
            {
                foreach (String s in setSongs)
                {
                    tw.WriteLine(s);
                }
            }
        }

        private void RemoveSong(int index)
        {
            for (int i = index + 1; i < listViewSongs.Items.Count; i++)
            {
                UpdateListViewItem(listViewSongs.Items[i] as ListViewItem, newPosition: i.ToString());
            }
            string removeSong = setSongs.ElementAt(index);
            setSongs.Remove(removeSong); removeSong = GetNameSongFromPath(removeSong);
            listViewSongs.Items.Remove(listViewSongs.ItemsWhere(t => t.Equals(removeSong)));
            if (listViewSongs.Items.Count == 0)
                ClearPlayer();
            else if (selectedSong == index)
            {
                if (selectedSong == listViewSongs.Items.Count)
                    listViewSongs.SelectedIndex = --selectedSong;
                UpdateUI(GetNameSongFromPath(setSongs[selectedSong]));
            }
        }

        private void ClearPlayer()
        {
            textSongName.Text = textSongTime.Text = null;
            if(songPlay)
            {
                bPlay.Visibility = Visibility.Visible;
                bPause.Visibility = Visibility.Hidden;
                mediaPlayer.Pause();
                mediaPlayer.Close();
                timer.Stop();
                songPlay = false;
            }
        }

        private void AddListViewItem(string number, string nameSong)
        {
            ListViewItem lvi = new ListViewItem();
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock tb1 = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = number };
            TextBlock tb2 = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = nameSong, 
                Width = 150, TextTrimming = TextTrimming.CharacterEllipsis };
            Ellipse e = new Ellipse() { Margin = new Thickness(20, 0, 20, 0), Width = 30, Height = 30, VerticalAlignment = VerticalAlignment.Center };
            e.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\Assets\wk2.png")))};

            MenuItem mi = new MenuItem();
            mi.Click += (o, a) => { RemoveSong(listViewSongs.SelectedIndex); };
            mi.Header = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "Delete",
                FontSize = 13,
                Foreground = Brushes.Red
            };

            ContextMenu cm = new ContextMenu() { };
            cm.Items.Add(mi);

            sp.Children.Add(tb1);
            sp.Children.Add(e);
            sp.Children.Add(tb2);
            lvi.Content = sp;
            lvi.ContextMenu = cm;
            listViewSongs.Items.Add(lvi);
        }

        private string GetNameSongFromPath(string path)
        {
            string name = path.Substring(path.LastIndexOf('\\') + 1, path.LastIndexOf('.') - path.LastIndexOf('\\') - 1);
            return name;
        }

        private void UpdateListViewItem(ListViewItem lvi, string newPosition = null, string newSong = null)
        {
            if (lvi != null && lvi.Content is StackPanel sp && sp.Children.Count == 3 && sp.Children[0] is TextBlock tb && sp.Children[2] is TextBlock tb1)
            {
                if(!string.IsNullOrEmpty(newPosition))
                {
                    tb.Text = newPosition;
                }
                if(!string.IsNullOrEmpty(newSong))
                {
                    tb1.Text = newSong;
                }
            }
        }

        private void ShuffleListSongs()
        {
            Random r = new Random();
            string nameS = null;
            if (selectedSong != -1)
                nameS = setSongs[selectedSong];
            setSongs = setSongs.OrderBy(a => r.Next(0,100)).ToList();
            for(int i = 0; i < setSongs.Count; i++)
            {
                string updateSong = GetNameSongFromPath(setSongs.ElementAt(i));
                UpdateListViewItem(listViewSongs.Items[i] as ListViewItem, newSong: updateSong);
            }
            if (selectedSong != -1)
                selectedSong = listViewSongs.SelectedIndex = setSongs.FindIndex(t => t.Equals(nameS));
        }
    }

    public static class Extensions
    {
        public static ListViewItem ItemsWhere(this ListView lv, Func<string, bool> func)
        {
            foreach(object o in lv.Items)
            {
                if (o != null && o is ListViewItem lvi && lvi.Content is StackPanel sp && sp.Children.Count == 3 && sp.Children[2] is TextBlock tb)
                {
                    if(func(tb.Text))
                    {
                        return lvi;
                    }
                }
            }
            return null;
        }
    }
}
