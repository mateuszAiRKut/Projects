using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
using AppWorldCup.Manager;

namespace AppWorldCup.Widok
{
    /// <summary>
    /// Interaction logic for kontrolkaUzytkownika.xaml
    /// </summary>
    public partial class kontrolkaUzytkownika : UserControl
    {
        private WyborKoloru wybierzKolor;

        public ObservableCollection<Uzytkownik> ListaUzytkownikow { get; set; } = new ObservableCollection<Uzytkownik>();
        public Uzytkownik WybranyUzytkownik { get; set; }

        public kontrolkaUzytkownika()
        {
            InitializeComponent();
            DataContext = this;

            colorList.ItemsSource = typeof(Brushes).GetProperties();

            wybierzKolor = ManagerKlas.InstancjaWyborKoloru as WyborKoloru;
            wybierzKolor.ZmienionoKolor += (sender, e) =>
            {
                Brush nowyKolor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
                kolorUzytkownika.Fill = nowyKolor;
                wybierzKolor.WybranyKolor = nowyKolor;
            };
        }

        public void zmienKolor(object sender, SelectionChangedEventArgs e) => wybierzKolor.ZmienKolor(e);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(poleLogin.Text))
            {
                MessageBox.Show("Pole Login jest puste");
            }
            else if (ListaUzytkownikow.Any(u => u.Login == poleLogin.Text))
            {
                MessageBox.Show("Istnieje już taki Login");
            }
            else
            {
                ListaUzytkownikow.Add(new Uzytkownik() { Login = poleLogin.Text, Kolor = wybierzKolor.WybranyKolor, Numer = ListaUzytkownikow.Count + 1 });
            }
        }

        private void listaUzytkownikow_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (WybranyUzytkownik != null)
            {
                btnUsun.IsEnabled = true;
            }
        }

        private void btnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (WybranyUzytkownik != null)
            {
                ListaUzytkownikow.Remove(WybranyUzytkownik);
                btnUsun.IsEnabled = false;
            }
        }
    }

    public class Uzytkownik
    {
        public string Login { get; set; }
        public Brush Kolor { get; set; }
        public int Punkty { get; set; }
        public int Numer { get; set; }
    }
}
