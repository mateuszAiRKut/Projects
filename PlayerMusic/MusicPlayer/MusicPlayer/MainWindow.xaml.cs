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

        public MainWindow()
        {
            InitializeComponent();
            fileSongs = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\listSongs.txt"));
            setSongs = new List<string>();
            LoadSongs();
            mediaPlayer = new MediaPlayer();
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
                mediaPlayer.Play();
                songPlay = true;
            }
            else
            {
                bPlay.Visibility = Visibility.Visible;
                mediaPlayer.Pause();
                songPlay = false;
            }

            b.Visibility = Visibility.Hidden;
        }

        private void Button_ClickAdd(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3|MP4 Files (*.mp4)|*.mp4";
            if (openFileDialog.ShowDialog() == true)
            {
                if (!setSongs.Contains(openFileDialog.FileName))
                {
                    if (setSongs.Count == countMax)
                    {
                        for (int i = 1; i < listViewSongs.Items.Count; i++)
                        {
                            UpdateListViewItem(listViewSongs.Items[i] as ListViewItem, newPosition: i.ToString());
                        }
                        string removeSong = setSongs.ElementAt(0);
                        setSongs.Remove(removeSong); removeSong = GetNameSongFromPath(removeSong);
                        listViewSongs.Items.Remove(listViewSongs.ItemsWhere(removeSong, (t) => t.Equals(removeSong)));
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

        private void AddListViewItem(string number, string nameSong)
        {
            ListViewItem lvi = new ListViewItem();
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock tb1 = new TextBlock() {VerticalAlignment = VerticalAlignment.Center, Text = number };
            TextBlock tb2 = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = nameSong, 
                Width = 150, TextTrimming = TextTrimming.CharacterEllipsis };
            Ellipse e = new Ellipse() { Margin = new Thickness(20, 0, 20, 0), Width = 30, Height = 30, VerticalAlignment = VerticalAlignment.Center };
            e.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\Assets\wk2.png")))};
            sp.Children.Add(tb1);
            sp.Children.Add(e);
            sp.Children.Add(tb2);
            lvi.Content = sp;
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
            setSongs = setSongs.OrderBy(a => r.Next(0,100)).ToList();
            for(int i = 0; i < setSongs.Count; i++)
            {
                string updateSong = GetNameSongFromPath(setSongs.ElementAt(i));
                UpdateListViewItem(listViewSongs.Items[i] as ListViewItem, newSong: updateSong);
            }
        }
    }

    public static class Extensions
    {
        public static ListViewItem ItemsWhere(this ListView lv, string value, Func<string, bool> func)
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
