using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using WpfAlarmClock.ExtensionMethods;

namespace WpfAlarmClock
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    ///
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
                setHours.Items.Add(ClassSettings.FormatDisplay(i)); //new ComboBoxItem() { FontSize = 12, Content = i.ToString() }
                setMinutes.Items.Add(ClassSettings.FormatDisplay(i));
            }
            for (; i < 60; i++)
                setMinutes.Items.Add(i.ToString());
            actualT = new StringBuilder();
            objectClassClock = new ClassClock();
            objectClassClock.AddActionClock(workClock);
            objectClassClock.StartClock();
            objectClassSettings = new ClassSettings();
            gridSettings.DataContext = gridMainWindow.DataContext = objectClassSettings;
            listAlarmCLock.ItemsSource = objectClassSettings.GetListAlarmClock;
            objectClassTraySystem = new ClassTrySystem(this);
            setToolTipToButtonToggleSwitch();
            setSaveSettings();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (objectClassClock != null)
                objectClassClock.StopClock();

            if (ClassFile.ChangeDataFile)
                ClassFile.SaveFile();
        }

        private void workClock()
        {
            while (true)
            {
                objectClassClock.Hour = (byte)DateTime.Now.Hour;
                objectClassClock.Minute = (byte)DateTime.Now.Minute;
                objectClassClock.Second = (byte)DateTime.Now.Second;
                actualT.Clear();
                actualT.Append(ClassSettings.FormatDisplay(objectClassClock.Hour)).Append(":").Append(ClassSettings.FormatDisplay(objectClassClock.Minute))
                    .Append(":").Append(ClassSettings.FormatDisplay(objectClassClock.Second));
                Dispatcher.Invoke(() =>
                {
                    actualTime.Text = actualT.ToString();
                    if (textToggleSwitchState.Text.Equals("On"))
                        objectClassSettings.OnAlarmSong(hour: objectClassClock.Hour, minute: objectClassClock.Minute);
                });
                Thread.Sleep(1000);
            }
        }

        private void setToolTipToButtonToggleSwitch()
        {
            toolTipButtonToggleSwitch = new ToolTip { FontWeight = FontWeights.Bold, Foreground = Brushes.Red };
            toolTipButtonToggleSwitch.Opened += async (o, arg) =>
            {
                await Task.Delay(2500);
                toolTipButtonToggleSwitch.IsOpen = false;
                buttonToggleSwitch.ToolTip = null;
            };
        }

        private void setSaveSettings()
        {
            string saveValue = ClassFile.SearchSaveValue("Alarm song");
            if (saveValue != null)
            {
                objectClassSettings.PathFile = saveValue;
                string alarm = saveValue.GetFileNameWithoutPath('\\');
                objectClassSettings.AlarmSong = alarm.Substring(0, alarm.IndexOf('.'));
                objectClassSettings.SetMediaPlayer(saveValue);
            }

            saveValue = ClassFile.SearchSaveValue("Snooze");
            if(saveValue != null)
            {
                objectClassSettings.Snooze = saveValue;
                SnoozeCheck.IsChecked = true;
                setValueSnooze(setSnooze, saveValue);
            }

            saveValue = ClassFile.SearchSaveValue("Alarm clock");
            if(saveValue != null)
            {
                objectClassSettings.UpdateListAlarmClockVersion(saveValue);
                string lefV = null, rightV = null;
                ClassParserSettings.SetLeftValueAndRightValue(objectClassSettings.GetListAlarmClock[0].ValueAlarmClock, ':', leftValue: ref lefV, rightValue: ref rightV);
                objectClassSettings.AlarmClock = lefV + ":" + rightV;
                setValueHoursMinutes(setHours, lefV);
                setValueHoursMinutes(setMinutes, rightV);
                previousValueAlarmClock = objectClassSettings.AlarmClock;
            }
            sliderVolume.Value = 25d;
        }

        private void setValueSnooze(ComboBox box, in string value)
        {
           foreach(ComboBoxItem item in box.Items)
           {
                if (item.Content.ToString().Equals(value))
                {
                    box.SelectedItem = item;
                    return;
                }
           }
        }

        private void setValueHoursMinutes(ComboBox box, in string value)
        {
            foreach (string item in box.Items)
            {
                if (item.Equals(value))
                {
                    box.SelectedItem = item;
                    return;
                }
            }
        }

        private void Button_SaveSettings(object sender, RoutedEventArgs e)
        {
            if(objectClassSettings.AlarmSong != null)
                ClassFile.UpdateOrAddDataToList("Alarm song-" + objectClassSettings.PathFile);

            if (objectClassSettings.Snooze != null)
                ClassFile.UpdateOrAddDataToList("Snooze-" + objectClassSettings.Snooze);

            if (objectClassSettings.CorrectAlarm && newValueAlarmClock())
            {
                objectClassSettings.AlarmClock = setHours.SelectedItem + ":" + setMinutes.SelectedItem;
                previousValueAlarmClock = objectClassSettings.AlarmClock;
                ClassFile.UpdateOrAddDataToList("Alarm clock-" + objectClassSettings.AlarmClock, 3);
                objectClassSettings.UpdateListAlarmClockVersion(ClassFile.GetValuesAlarmClocks());
            }
        }

        private bool newValueAlarmClock()
        {
            return !(setHours.SelectedItem + ":" + setMinutes.SelectedItem).Equals(previousValueAlarmClock);
        }

        private void Button_SearchAlarmSong(object sender, RoutedEventArgs e)
        {
            objectClassSettings.SearchAlarmSong();
        }

        private void UserControlToggleSwitch_MouseLeftButtonDownSwitch(object sender, MouseButtonEventArgs e)
        {
            if (checkValue(objectClassSettings.AlarmClock, "You must set alarm clock"))
                return;
            if(checkValue(objectClassSettings.AlarmSong, "You must set alarm song"))
                return;

            if (textToggleSwitchState.Text.Equals("Off"))
                setValuesButtonToggleSwitch("On", marginRight: 0, marginLeft: 65);
            else
            {
                setValuesButtonToggleSwitch("Off", marginRight: 65, marginLeft: 0);
                objectClassSettings.OffAlarmSong();
            }

        }

        private bool checkValue(in string value, string message)
        {
            if (value == null)
            {
                if (!toolTipButtonToggleSwitch.IsOpen)
                {
                    buttonToggleSwitch.ToolTip = toolTipButtonToggleSwitch;
                    toolTipButtonToggleSwitch.Content = message;
                    toolTipButtonToggleSwitch.IsOpen = true;
                }
                return true;
            }
            return false;
        }

        private void setValuesButtonToggleSwitch(string text, byte marginRight, byte marginLeft)
        {
            textToggleSwitchState.Text = text;
            buttonToggleSwitch.SetMarginSwitchRight = marginRight;
            buttonToggleSwitch.SetMarginSwitchLeft = marginLeft;
            buttonToggleSwitch.SetColorButton();
        }

        private void SnoozeCheck_Checked(object sender, RoutedEventArgs e)
        {
            setSnooze.Opacity = 1.0d;
            setSnooze.IsEnabled = true;
        }

        private void SnoozeCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            setSnooze.SelectedItem = null;
            objectClassSettings.Snooze = null;
            setSnooze.Opacity = 0.5d;
            setSnooze.IsEnabled = false;
        }

        private void setSnooze_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem boxItem = setSnooze.SelectedItem as ComboBoxItem;
            if (boxItem != null)
                objectClassSettings.Snooze = boxItem.Content.ToString();
        }

        private void setHoursAndMinutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setMinutes.SelectedItem != null && setHours.SelectedItem != null)
                objectClassSettings.CorrectAlarm = true;
            else
                objectClassSettings.CorrectAlarm = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            objectClassSettings.Volume = (byte)sliderVolume.Value + "%";
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
        }

        private StringBuilder actualT;
        private ToolTip toolTipButtonToggleSwitch;
        private ClassClock objectClassClock;
        private ClassSettings objectClassSettings;
        private string previousValueAlarmClock;
        private ClassTrySystem objectClassTraySystem;
    }
}
