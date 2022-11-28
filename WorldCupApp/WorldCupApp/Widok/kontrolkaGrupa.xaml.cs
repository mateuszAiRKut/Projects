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

namespace WorldCupApp.Widok
{
    /// <summary>
    /// Interaction logic for kontrolkaGrupa.xaml
    /// </summary>
    public partial class kontrolkaGrupa : UserControl
    {
        private string _wybranaGrupa;
        private Dictionary<string, List<GrupaWiersz>> daneDlaGrup = new Dictionary<string, List<GrupaWiersz>>()
        {
            { "Grupa A", new List<GrupaWiersz>() { new GrupaWiersz() { Druzyna = "Holandia" } } },
            { "Grupa B", new List<GrupaWiersz>() { new GrupaWiersz() { Druzyna = "Anglia" } } }
        };

        public ObservableCollection<GrupaWiersz> Grupa { get; set; } = new ObservableCollection<GrupaWiersz>();
        public GrupaWiersz WybranyWiersz { get; set; }
        public string WybranaGrupa
        {
            get => _wybranaGrupa;
            set
            {
                _wybranaGrupa = value;

                Grupa.Clear();

                if (daneDlaGrup.ContainsKey(_wybranaGrupa))
                {
                    foreach (var wiesz in daneDlaGrup[_wybranaGrupa])
                    {
                        Grupa.Add(wiesz);
                    }
                }
            }
        }

        public kontrolkaGrupa()
        {
            InitializeComponent();
            DataContext = this;
        }
    }

    public class GrupaWiersz
    {
        public int Pozycja { get; set; }
        public string Druzyna { get; set; }
        public int IloscMeczy { get; set; }
        public int Wygrane { get; set; }
        public int Przegrane { get; set; }
        public int Remisy { get; set; }
        public (int, int) BilansBramek { get; set; }
        public string BilansBramekTekst => $"{BilansBramek.Item1} : {BilansBramek.Item2}";
        public int Punkty { get; set; }
    }
}
