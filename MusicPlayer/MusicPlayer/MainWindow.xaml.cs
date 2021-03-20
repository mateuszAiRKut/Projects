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
        private HashSet<string> setSongs;
        private int countMax = 20;

        public MainWindow()
        {
            InitializeComponent();
            fileSongs = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\listSongs.txt"));
            setSongs = new HashSet<string>();
            LoadSongs();
            addListViewItem("1", "Braccuda");
            addListViewItem("2", "Tiesto");
            addListViewItem("3", "Music");
            addListViewItem("4", "cos");
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
                bPause.Visibility = Visibility.Visible;
            else
                bPlay.Visibility = Visibility.Visible;

            b.Visibility = Visibility.Hidden;
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
                            setSongs.Add(s);
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

        private void Button_ClickAdd(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3|MP4 Files (*.mp4)|*.mp4";
            if (openFileDialog.ShowDialog() == true)
            {
                if(setSongs.Add(openFileDialog.FileName) && setSongs.Count > countMax)
                {
                    setSongs.Remove(setSongs.ElementAt(0));
                }
            }
        }

        private void addListViewItem(string number, string nameSong)
        {
            ListViewItem lvi = new ListViewItem();
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBlock tb1 = new TextBlock() {VerticalAlignment = VerticalAlignment.Center, Text = number };
            TextBlock tb2 = new TextBlock() { VerticalAlignment = VerticalAlignment.Center, Text = nameSong, 
                Width = 100, TextTrimming = TextTrimming.CharacterEllipsis };
            Ellipse e = new Ellipse() { Margin = new Thickness(20, 0, 20, 0), Width = 30, Height = 30, VerticalAlignment = VerticalAlignment.Center };
            e.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\Assets\wk2.png")))};
            sp.Children.Add(tb1);
            sp.Children.Add(e);
            sp.Children.Add(tb2);
            lvi.Content = sp;
            listViewSongs.Items.Add(lvi);
        }
    }
}
