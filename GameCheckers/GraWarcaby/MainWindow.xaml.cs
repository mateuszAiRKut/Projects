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
using GraWarcaby.Logika;
using System.Windows.Media.Animation;

namespace GraWarcaby
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow DajKontekst { private set; get; }
        public static TextBlock KtoryGracz { get; set; }
        public static TextBlock ZbitePionkiCzerwone { get; set; }
        public static TextBlock ZbitePionkiBiale { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DajKontekst = this;
            KlasaPlansza.DlugoscBoku = 65;
            KlasaPlansza.StworzPlansze(obszar, Brushes.Green, Brushes.White, Brushes.Green);
            KtoryGracz = ktoryGracz;
            ZbitePionkiCzerwone = zbitePionkiCzerwone;
            ZbitePionkiBiale = zbitePionkiBiale;
        }

    }
}
