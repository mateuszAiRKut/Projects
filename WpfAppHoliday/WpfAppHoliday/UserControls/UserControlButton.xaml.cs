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

namespace WpfAppHoliday.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy UserControlButton.xaml
    /// </summary>
    public partial class UserControlButton : UserControl
    {
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(String),
        typeof(UserControlButton), new FrameworkPropertyMetadata(string.Empty));

        public static readonly RoutedEvent ClickEvent =
        EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(UserControlButton));

        public UserControlButton()
        {
            InitializeComponent();
        }

        public String Text
        {
            get { return GetValue(TextProperty).ToString(); }
            set { SetValue(TextProperty, value); }
        }

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }
    }
}
