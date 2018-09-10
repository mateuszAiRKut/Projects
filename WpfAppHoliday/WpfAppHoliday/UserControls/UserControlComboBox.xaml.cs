using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Logika interakcji dla klasy UserControlComboBox.xaml
    /// </summary>
    public partial class UserControlComboBox : UserControl
    {
        private ObservableCollection<Node> _nodeList;

        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(UserControlComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register("SelectedItems", typeof(List<string>), typeof(UserControlComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(OnSelectedItemsChanged)));

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(UserControlComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
        DependencyProperty.Register("DefaultText", typeof(string), typeof(UserControlComboBox), new UIPropertyMetadata(string.Empty));

        public UserControlComboBox()
        {
            InitializeComponent();
            _nodeList = new ObservableCollection<Node>();

        }

        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public List<string> SelectedItems
        {
            get { return (List<string>)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        private void DisplayInControl()
        {
            _nodeList.Clear();
            if (ItemsSource.Count > 0)
                _nodeList.Add(new Node("Wszystkie"));
            foreach (string s in ItemsSource)
            {
                Node node = new Node(s);
                _nodeList.Add(node);
            }
            MultiSelectCombo.ItemsSource = _nodeList;
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControlComboBox control = (UserControlComboBox)d;
            control.DisplayInControl();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;
            if (clickedBox.Content.ToString() == "Wszystkie")
            {
                foreach (Node node in _nodeList)
                {
                    node.IsSelected = true;
                }
            }
            else
            {
                int _selectedCount = 0;
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected && s.Title != "Wszystkie")
                        _selectedCount++;
                }
                if (_selectedCount == _nodeList.Count - 1)
                    _nodeList.FirstOrDefault(i => i.Title == "Wszystkie").IsSelected = true;
                else
                    _nodeList.FirstOrDefault(i => i.Title == "Wszystkie").IsSelected = false;
            }
            SetSelectedItems();
            SetText();
        }

        private void SetSelectedItems()
        {
            if (SelectedItems == null)
                SelectedItems = new List<string>();
            SelectedItems.Clear();
            foreach (Node node in _nodeList)
            {
                if (node.IsSelected && node.Title != "Wszystkie")
                {
                    if (ItemsSource.Count > 0)
                        SelectedItems.Add(node.Title);
                }
            }
        }

        private void SetText()
        {
            if (SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected == true && s.Title == "Wszystkie")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("Wszystkie");
                        break;
                    }
                    else if (s.IsSelected == true && s.Title != "Wszystkie")
                    {
                        displayText.Append(s.Title);
                        displayText.Append(", ");
                    }
                }
                Text = displayText.ToString().TrimEnd(',',' ');
            }
            if (string.IsNullOrEmpty(Text))
            {
                Text = DefaultText;
            }
        }

        private void SelectNodes()
        {
            foreach (string s in SelectedItems)
            {
                Node node = _nodeList.FirstOrDefault(i => i.Title == s);
                if (node != null)
                    node.IsSelected = true;
            }
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControlComboBox control = (UserControlComboBox)d;
            control.SelectNodes();
            control.SetText();
        }
    }

    class Node : INotifyPropertyChanged
    {
        private string _title;
        private bool _isSelected;

        #region ctor
        public Node(string title)
        {
            Title = title;
        }
        #endregion

        #region Properties
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public bool IsSelected
        {

            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }

        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
