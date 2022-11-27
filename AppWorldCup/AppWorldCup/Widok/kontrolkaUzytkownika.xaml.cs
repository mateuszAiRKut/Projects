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
        private ManagerRepozytorium repozytorium;

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

            AktualizujListeUzytkownikow();
        }

        public void zmienKolor(object sender, SelectionChangedEventArgs e) => wybierzKolor.ZmienKolor(e);

        private async void AktualizujListeUzytkownikow()
        {
            repozytorium = ManagerKlas.InstancjaManagerRepozytorium as ManagerRepozytorium;
            string lista = await repozytorium.PobierzZawartoscPliku("mateuszAirKut", "Projects", "AppWorldCup/AppWorldCup/listaUzytkownikow.txt", "master");

            foreach (var linia in lista.Split(Environment.NewLine))
            {
                var dane = linia.Split(' ');

                if (dane.Length == 4)
                {
                    ListaUzytkownikow.Add(new Uzytkownik() { Login = dane[0], 
                        Kolor = new BrushConverter().ConvertFromString(dane[1]) as SolidColorBrush,  Numer = int.Parse(dane[3]), Punkty = int.Parse(dane[2]) });
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(poleLogin.Text))
            {
                MessageBox.Show("Pole Login jest puste");
            }
            else if (wybierzKolor.WybranyKolor == null)
            {
                MessageBox.Show("Nie wybrano koloru");
            }
            else if (ListaUzytkownikow.Any(u => u.Login == poleLogin.Text))
            {
                MessageBox.Show("Istnieje już taki Login");
            }
            else
            {
                ListaUzytkownikow.Add(new Uzytkownik() { Login = poleLogin.Text, Kolor = wybierzKolor.WybranyKolor, Numer = ListaUzytkownikow.Count + 1 });
                btnZapisz.IsEnabled = true;
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

                if (ListaUzytkownikow.Count == 0)
                {
                    btnZapisz.IsEnabled = false;
                }
                else
                {
                    btnZapisz.IsEnabled = true;
                }
            }
        }

        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var uzytkownik in ListaUzytkownikow)
            {
                sb.AppendLine(uzytkownik);
            }

            try
            {
                repozytorium.AktualizujPlik("mateuszAirKut", "Projects", "AppWorldCup/AppWorldCup/listaUzytkownikow.txt", sb.ToString(), "aktualizacja listy uzytkownikow", "master");
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                MessageBox.Show("Pomyslnie zapisano uzytkownikow");
            }
        }
    }

    public class Uzytkownik
    {
        public string Login { get; set; }
        public Brush Kolor { get; set; }
        public int Punkty { get; set; }
        public int Numer { get; set; }

        public override string ToString() => $"{Login} {Kolor} {Punkty} {Numer}";
        public static implicit operator string(Uzytkownik u) => u.ToString();
    }
}
