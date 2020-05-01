using AudioSwitcher.AudioApi.CoreAudio;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using WpfAlarmClock.ExtensionMethods;

namespace WpfAlarmClock
{
    public class ClassListAlarmClock : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ValueAlarmClock { get; set; }
        public bool Check { get; set; }
        public byte ValueCount
        {
            get => valueCount;
            set
            {
                valueCount = value;
                OnPropertyRaised("ValueCount");
            }
        }
        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private byte valueCount;
    }

    public class ClassSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string AlarmSong
        {
            get => alarmSong;
            set
            {
                alarmSong = value;
                OnPropertyRaised("AlarmSong");
            }
        }

        public string AlarmClock
        {
            get => alarmClock;
            set
            {
                alarmClock = value;
                time = null;
                OnPropertyRaised("AlarmClock");
            }
        }

        public string Volume
        {
            get => volume;
            set
            {
                volume = value;
                OnPropertyRaised("Volume");
            }
        }

        public string PathFile { set; get;}
        public string Snooze { set; get; }
        public bool CorrectAlarm { get; set; }
        public ObservableCollection<ClassListAlarmClock> GetListAlarmClock { get => listAlarmClock; }

        public ClassSettings()
        {
            listAlarmClock = new ObservableCollection<ClassListAlarmClock>();
            mediaPlayer = new MediaPlayer();
            defaultDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        public void OnAlarmSong(in byte hour, in byte minute)
        {
            if (checkAlarm(hour: hour, minute: minute) && checkDay())
            {
                if(defaultDevice.IsMuted)
                    defaultDevice.Mute(false);
                previousVolume = (byte)defaultDevice.Volume;
                defaultDevice.Volume = getVolume();
                mediaPlayer.Play();
                setBit(ref waitSnooze, 0);
            }
            updateSnooze();
        }

        public void OffAlarmSong()
        {
            mediaPlayer.Stop();
            resetBit(ref waitSnooze, 0);
            defaultDevice.Volume = previousVolume;
        }

        public void UpdateListAlarmClockVersion(in string valuesAlarmClock)
        {
            string alarmClock = null, count = null;
            byte resultCount;
            int index = 0;
            for (byte i = 0; i < listAlarmClock.Count; i++)
                listAlarmClock[i].Check = false;
            foreach(string valueAlarm in valuesAlarmClock.Split(','))
            { 
                ClassParserSettings.SetLeftValueAndRightValue(valueAlarm, '/', leftValue: ref alarmClock, rightValue: ref count);
                byte.TryParse(count, out resultCount);
                index = listAlarmClock.FindIndex((obj) => obj.ValueAlarmClock.Equals(alarmClock));
                if (index != -1)
                { 
                    if(listAlarmClock[index].ValueCount != resultCount)
                        listAlarmClock[index].ValueCount = resultCount;
                    listAlarmClock[index].Check = true;           
                }
                else
                    listAlarmClock.Add(new ClassListAlarmClock { ValueAlarmClock = alarmClock, ValueCount = resultCount, Check = true });
            }
            listAlarmClock.RemoveAll((obj) => !obj.Check);
            listAlarmClock.Sort((element1, element2) => element1.ValueCount.CompareTo(element2.ValueCount));
            
        }

        public void SearchAlarmSong()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                PathFile = openFileDialog.FileName;
                SetMediaPlayer(PathFile);
                string alarm = PathFile.GetFileNameWithoutPath('\\');
                AlarmSong = alarm.Substring(0, alarm.IndexOf('.'));
            }
        }

        public void SetMediaPlayer(in string path)
        {
            mediaPlayer.Open(new Uri(path));
        }

        public static string FormatDisplay(in byte value)
        {
            return ((value < 10) ? "0" : "") + value.ToString();
        }

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private bool checkAlarm(in byte hour, in byte minute)
        {
            ClassParserSettings.SetLeftValueAndRightValue(AlarmClock, ':', leftValue: ref leftHour, rightValue: ref rightMinute);
            if (byte.TryParse(leftHour, out valueHour) && byte.TryParse(rightMinute, out valueMinute))
                return valueHour.Equals(hour) && valueMinute.Equals(minute);
            return false;
        }

        private bool checkDay()
        {
            if (!time.HasValue)
            {
                time = DateTime.Now;
                return true;
            }
            else
                return !time.Value.Day.Equals(DateTime.Now.Day);
        }

        private void updateSnooze()
        {
            if(checkBit(ref waitSnooze, 0) && Snooze != null && time.HasValue)
            {
                if (checkTime(timeP: time.Value, DateTime.Now) && !checkBit(ref waitSnooze, 1))
                {
                    mediaPlayer.Stop();
                    time = DateTime.Now;
                    setBit(ref waitSnooze, 1);
                }
                else if(checkBit(ref waitSnooze, 1))
                {
                    ClassParserSettings.SetLeftValueAndRightValue(Snooze, ' ', leftValue: ref rightMinute, rightValue: ref leftHour);
                    if (byte.TryParse(rightMinute, out valueMinute) && DateTime.Now.Minute - time.Value.Minute >= valueMinute)
                    {
                        mediaPlayer.Play();
                        time = DateTime.Now;
                        resetBit(ref waitSnooze, 1);
                    }
                }                   
            }
        }

        private bool checkTime(in DateTime timeP, in DateTime timeN)
        {
            if (timeP.Minute.Equals(timeN.Minute))
                return (timeN.Second - timeP.Second) >= 30;
            else
                return (60 - timeP.Second + timeN.Second) >= 30;
        }

        private void setBit(ref byte value, byte position)
        {
            value |= (byte)(1 << position);
        }

        private void resetBit(ref byte value, byte position)
        {
            value &= (byte)(~(1 << position));
        }

        private bool checkBit(ref byte value, byte position)
        {
            return (value & (1 << position)) != 0;
        }

        public byte getVolume()
        {
            string leftV = null, rightV = null;
            byte newVolume;
            ClassParserSettings.SetLeftValueAndRightValue(Volume, '%', leftValue: ref leftV, rightValue: ref rightV);
            if (byte.TryParse(leftV, out newVolume))
                return newVolume;
            return 25;
        }

        private MediaPlayer mediaPlayer;
        private string alarmSong, alarmClock, leftHour, rightMinute, volume;
        private ObservableCollection<ClassListAlarmClock> listAlarmClock;
        CoreAudioDevice defaultDevice;
        private DateTime? time;
        byte valueHour, valueMinute, waitSnooze, previousVolume; //1 bit - check Snooze is set, 2 bit - check time passed and again turn on alarm
    }
}
