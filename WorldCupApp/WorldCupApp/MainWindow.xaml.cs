using WorldCupApp.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace WorldCupApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _grupa;
        public ObservableCollection<string> Grupy { get; set; } = new ObservableCollection<string>() { "Grupa A", "Grupa B", "Grupa C", "Grupa D", "Grupa E", "Grupa F", "Grupa G", "Grupa H" };
        public string Grupa
        {
            get => _grupa;
            set
            {
                _grupa = value;
                aktywnaGrupa.WybranaGrupa = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
