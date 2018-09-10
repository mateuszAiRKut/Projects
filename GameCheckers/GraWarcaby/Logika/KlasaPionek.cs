using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraWarcaby.Logika
{
    enum GraczWybor { Czerwony, Bialy };

    class KlasaPionek
    {
    }

    class Pionek
    {
        private Ellipse obszar;
        public Ellipse Obszar
        {
            set
            {
                obszar = value;
                if (Gracz == GraczWybor.Czerwony)
                    obszar.Fill = Brushes.Red;
                else
                    obszar.Fill = Brushes.White;
            }

            get
            {
                if (obszar != null)
                    return obszar;
                return new Ellipse();
            }
        }

        public int Wiersz { set; get; }
        public int Kolumna { set; get; }
        public bool Damka { set; get; }
        public GraczWybor Gracz { private set; get; }

        public Pionek(GraczWybor gracz)
        {
            Gracz = gracz;
        }

        public override string ToString()
        {
            return "Pionek Wiersz " + Wiersz + " Kolumna " + Kolumna;
        }
    }

}
