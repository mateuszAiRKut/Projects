using System;
using System.Collections.Generic;
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
using System.Threading;
using Microsoft.Win32;
using AudioSwitcher.AudioApi.CoreAudio;

namespace WpfAlarmClock
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            byte i = 0;
            for (; i < 24; i++)
            {
                setHours.Items.Add(i.ToString());              //new ComboBoxItem() { FontSize = 12, Content = i.ToString() }
                setMinutes.Items.Add(i.ToString());
            }
            for (; i < 60; i++)
                setMinutes.Items.Add(i.ToString());
            actualT = new StringBuilder();
            mediaPlayer = new MediaPlayer();
            objVol = new CoreAudioController().DefaultPlaybackDevice;
            objVol.Volume = 44;
            objectClassClock = new ClassClock();
            objectClassClock.AddActionClock(workTimer);
            objectClassClock.StartClock();

            //RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //reg.SetValue("Application Alarm Clock", @"C:\Users\Mateusz\source\repos\WpfAlarmClock\WpfAlarmClock\bin\Debug\WpfAlarmClock.EXE");
        }

        void workTimer()
        {
            while (true)
            {
                objectClassClock.Hour = (byte)DateTime.Now.Hour;
                objectClassClock.Minute = (byte)DateTime.Now.Minute;
                objectClassClock.Second = (byte)DateTime.Now.Second;
                actualT.Clear();
                actualT.Append(objectClassClock.Hour).Append(":").Append(objectClassClock.Minute).Append(":").Append(objectClassClock.Second);
                Dispatcher.Invoke(() => 
                { 
                    actualTime.Text = actualT.ToString();
                    if (actualAlarm.Text != "")
                    {
                        if (objectClassClock.Hour == byte.Parse(setHours.Text) && objectClassClock.Minute >= byte.Parse(setMinutes.Text) && !setAnothetTime)
                            mediaPlayer.Play();
                    }
                });              
                Thread.Sleep(1000);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (setHours.Text == "" || setMinutes.Text == "")
                return;
            actualAlarm.Text = setHours.Text + ":" + setMinutes.Text;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            setAnothetTime = false;
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            setAnothetTime = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (objectClassClock != null)
            {
                objectClassClock.StopClock();
            }
        }

        private bool setAnothetTime;
        private StringBuilder actualT;
        private MediaPlayer mediaPlayer;
        private CoreAudioDevice objVol;
        private ClassClock objectClassClock;
    }
}
