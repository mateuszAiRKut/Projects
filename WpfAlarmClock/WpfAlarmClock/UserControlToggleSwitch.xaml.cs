using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfAlarmClock
{
    /// <summary>
    /// Logika interakcji dla klasy UserControlToggleSwitch.xaml
    /// </summary>
    public partial class UserControlToggleSwitch : UserControl
    {
        
        public UserControlToggleSwitch()
        {
            InitializeComponent();
            marginSwitch = new Thickness(0, 0, 65, 0);
            MarginSwitch = marginSwitch;
            colorOff = new SolidColorBrush(Color.FromRgb(140, 140, 140));
            colorOn = new SolidColorBrush(Color.FromRgb(15, 174, 22));
            FillSwitch = colorOff;
        }

        static UserControlToggleSwitch()
        {
            MarginSwitchProperty = DependencyProperty.Register("MarginSwitch", typeof(Thickness), typeof(UserControlToggleSwitch), new FrameworkPropertyMetadata(null));
            FillSwitchProperty = DependencyProperty.Register("FillSwitch", typeof(Brush), typeof(UserControlToggleSwitch), new FrameworkPropertyMetadata(null));
            MouseLeftButtonDownSwitchEvent = EventManager.RegisterRoutedEvent("MouseLeftButtonDownSwitch", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(Grid));
        }

        public byte SetMarginSwitchLeft
        {
            set
            {
                marginSwitch.Left = value;
                MarginSwitch = marginSwitch;
            }
        }

        public byte SetMarginSwitchRight
        {
            set
            {
                marginSwitch.Right = value;
                MarginSwitch = marginSwitch;
            }
        }

        public Thickness MarginSwitch
        {
            get { return (Thickness)GetValue(MarginSwitchProperty); }
            set { SetValue(MarginSwitchProperty, value); }
        }

        public Brush FillSwitch
        {
            get { return GetValue(FillSwitchProperty) as Brush; }
            set { SetValue(FillSwitchProperty, value); }
        }

        public event MouseButtonEventHandler MouseLeftButtonDownSwitch
        {
            add { AddHandler(MouseLeftButtonDownSwitchEvent, value); }
            remove { RemoveHandler(MouseLeftButtonDownSwitchEvent, value); }
        }

        public void SetColorButton()
        {
            if (MarginSwitch.Left != 0)
                FillSwitch = colorOn;
            else
                FillSwitch = colorOff;
        }

        /*public void AddActionMouseLeftButtonDowmSwitch(MouseButtonEventHandler action)
        {
            delegateMouseLeftButtonDownSwitch += action;
        }*/

        private void Switch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) {RoutedEvent = MouseLeftButtonDownSwitchEvent });  
            //delegateMouseLeftButtonDownSwitch?.Invoke(sender, e);
        }

        private static readonly DependencyProperty MarginSwitchProperty;
        private static readonly DependencyProperty FillSwitchProperty;
        private static readonly RoutedEvent MouseLeftButtonDownSwitchEvent;
        private Brush colorOff, colorOn;
        private Thickness marginSwitch;
        //private MouseButtonEventHandler delegateMouseLeftButtonDownSwitch;
    }
}
