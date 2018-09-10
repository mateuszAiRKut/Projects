using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media.Animation;
using GraWarcaby.Logika.Animacja;
using GraWarcaby.Logika.Reguly;

namespace GraWarcaby.Logika
{
    static class KlasaPlansza
    {
        private static Pole[] tablicaPol;
        private static List<Pionek> tablicaPionkiGraczCzerwony;
        private static List<Pionek> tablicaPionkiGraczBialy;
        private static int numer, liczbaKolumn, liczbaWierszy;

        public static int DlugoscBoku { set; get; }
        public static string Komunikat { private set; get; }
        public static Pionek WybranyPionek { set; get; }
        public static Canvas PoleGry { private set; get; }

        static KlasaPlansza()
        {
            tablicaPol = new Pole[64];
            tablicaPionkiGraczCzerwony = new List<Pionek>(12);
            tablicaPionkiGraczBialy = new List<Pionek>(12);
            liczbaKolumn = liczbaWierszy = 8;
           
        }

        public static void StworzPlansze(Grid obszar, SolidColorBrush kolor1, SolidColorBrush kolor2, SolidColorBrush kolorObszar, int gruboscO = 2)
        {
            DlugoscBoku = (DlugoscBoku < 50 ? 50 : (DlugoscBoku > 85 ? 85 : DlugoscBoku));
            double margines = obliczMarginesObramowania(obszar.Height);
            Border obramowanie = new Border() { Margin = new Thickness(margines - gruboscO), CornerRadius = new CornerRadius(0), Background = kolorObszar,
                BorderBrush = Brushes.Black, BorderThickness = new Thickness(gruboscO) };
            PoleGry = new Canvas() { Height = obszar.Height - 2 * margines, Width = obszar.Width - 2 * margines };
            //panel.MouseLeftButtonDown += (sender, e) => { Point p = e.GetPosition(panel); MessageBox.Show(p.X.ToString() + " " + p.Y.ToString()); };
            obramowanie.Child = PoleGry;
            for (int i = 0; i < liczbaWierszy; i++)
            {
                if (i % 2 == 0)
                    stworzRzad(kolor1, kolor2, i);
                else
                    stworzRzad(kolor2, kolor1, i);
            }
            dodajLinie(gruboscO);
            rozstawPionkiGracza(PoleGry, tablicaPionkiGraczCzerwony);
            rozstawPionkiGracza(PoleGry, tablicaPionkiGraczBialy);
            obszar.Children.Add(obramowanie);
        }

        public static void NaJakimPoluStoiPionek(Point p, GraczWybor kolor, bool czyTylkoSprawdzacZbijanie = false)
        {
            int kolumna = 0, wiersz = 0;
            for(int i=0, dol = i*DlugoscBoku, gora = dol + DlugoscBoku; i < liczbaKolumn; i++, dol = i * DlugoscBoku, gora = dol + DlugoscBoku)
            {
                if (p.X >= dol && p.X <= gora)
                    kolumna = (i+1);
                else if (p.Y >= dol && p.Y <= gora)
                    wiersz = (i+1);
            }
            Pole poleSzukane = tablicaPol.Where((pole) => pole.Wiersz == wiersz && pole.Kolumna == kolumna).First();
            if(kolor == GraczWybor.Czerwony)
                WybranyPionek = tablicaPionkiGraczCzerwony.Where((pion) => pion.Wiersz == wiersz && pion.Kolumna == kolumna).First();
            else
                WybranyPionek = tablicaPionkiGraczBialy.Where((pion) => pion.Wiersz == wiersz && pion.Kolumna == kolumna).First();
            if (WybranyPionek.Damka)
                KlasaReguly.MozliweRuchyDamka(tablica: tablicaPol, listaPionkowCzerwony: tablicaPionkiGraczCzerwony, listaPionkowBialy: tablicaPionkiGraczBialy, wiersz: wiersz, kolumna: kolumna, liczbaWierszy: liczbaWierszy, liczbaKolumn: liczbaKolumn, czyTylkoSprawdzacZbijanie: czyTylkoSprawdzacZbijanie);
            else
                KlasaReguly.MozliweRuchy(tablica: tablicaPol, listaPionkowCzerwony: tablicaPionkiGraczCzerwony, listaPionkowBialy: tablicaPionkiGraczBialy, wiersz: wiersz, kolumna: kolumna, liczbaWierszy: liczbaWierszy, liczbaKolumn: liczbaKolumn, czyTylkoSprawdzacZbijanie: czyTylkoSprawdzacZbijanie);
        } 

        public static bool czyWolnePole(int kolumna, int wiersz)
        {
            return (kolumna >= 1 && kolumna <=liczbaKolumn) && (wiersz >= 1 && wiersz <= liczbaWierszy) && tablicaPionkiGraczBialy.Where((pionek) => pionek.Wiersz == wiersz && pionek.Kolumna == kolumna).Count() == 0
                && tablicaPionkiGraczCzerwony.Where((pionek) => pionek.Wiersz == wiersz && pionek.Kolumna == kolumna).Count() == 0;
        }
        

        private static void rozstawPionkiGracza(Canvas panel, List<Pionek> tablica)
        {
            if (tablicaPionkiGraczCzerwony.Count == 0 && tablicaPionkiGraczBialy.Count == 0)
                rozstawPionki(GraczWybor.Czerwony, tablica);
            else
                rozstawPionki(GraczWybor.Bialy, tablica);
        }

        private static void rozstawPionki(GraczWybor gracz, List<Pionek> tablica)
        {
            Pionek pionek = null;
            for (int i = 0; i < 3; i++)
            {
                int mnoznik = (i % 2 == 0 ? (gracz == GraczWybor.Czerwony ? 3 : 1) : (gracz == GraczWybor.Czerwony ? 1 : 3));
                int mnoznik2 = (gracz == GraczWybor.Czerwony ? 1 : 11);
                for (int j = 0; j < 4; j++)
                {
                    pionek = new Pionek(gracz);
                    pionek.Obszar = new Ellipse() { Fill = Brushes.Red, Width = DlugoscBoku - 20, Height = DlugoscBoku - 20 };
                    pionek.Kolumna = (2*j +(mnoznik == 3 ? 2 : 1));
                    pionek.Wiersz = ((mnoznik2 == 1 ? 1 : 6) + i);
                    KlasaAnimacja.dodajAnimacje(pionek.Obszar, PoleGry);
                    tablica.Add(pionek);
                    Canvas.SetLeft(pionek.Obszar, (DlugoscBoku * (mnoznik + j * 4) - pionek.Obszar.Width) / 2);
                    Canvas.SetTop(pionek.Obszar, (DlugoscBoku * (mnoznik2 + i * 2) - pionek.Obszar.Height) / 2);
                    PoleGry.Children.Add(pionek.Obszar);
                }
            }
        }

        private static void dodajLinie(int gruboscO)
        {
            for (int i = 0; i < liczbaKolumn - 1; i++)
                PoleGry.Children.Add(new Line() { Stroke = Brushes.Black, StrokeThickness = gruboscO, X1 = DlugoscBoku * (i + 1), X2 = DlugoscBoku * (i + 1), Y1 = -1, Y2 = PoleGry.Height});
            for (int i = 0; i < liczbaWierszy - 1; i++)
                PoleGry.Children.Add(new Line() {Stroke = Brushes.Black, StrokeThickness = gruboscO, Y1=DlugoscBoku*(i+1), Y2=DlugoscBoku*(i+1), X1=-1, X2= PoleGry.Width});
        }

        private static double obliczMarginesObramowania(double szerokoscObszaru)
        {
            return (szerokoscObszaru - 8.0 * DlugoscBoku)/2.0;
        }

       public static int WyodrebnijLiczbe(string tekst)
       {
            int liczba = 0;
            for(int i=tekst.Length-1,j=0; i >=0;i--,j++)
            {
                if (!Char.IsNumber(tekst[i]))
                    break;
                liczba += ((int)Char.GetNumericValue(tekst[i])) * ((int)Math.Pow(10, j));
                //(int)(tekst[i] - '0')
            }
            return liczba;
        }

        public static void UsunPionek(Pionek zbijanyPionek)
        {
            if(tablicaPionkiGraczBialy.Exists((pionek) => pionek.Kolumna == zbijanyPionek.Kolumna && pionek.Wiersz == zbijanyPionek.Wiersz))
            {
                tablicaPionkiGraczBialy.Remove(zbijanyPionek);
                return;
            }
            if (tablicaPionkiGraczCzerwony.Exists((pionek) => pionek.Kolumna == zbijanyPionek.Kolumna && pionek.Wiersz == zbijanyPionek.Wiersz))
            {
                tablicaPionkiGraczCzerwony.Remove(zbijanyPionek);
            }
        }

        private static void stworzRzad(SolidColorBrush kolor1, SolidColorBrush kolor2, int numerWiersza)
        {
            for (int i=0; i < liczbaWierszy; i++)
            {
                if (numer >= tablicaPol.Length)
                    break;
                tablicaPol[numer] = new Pole();
                tablicaPol[numer].Obszar = new Rectangle() { Width = DlugoscBoku, Height = DlugoscBoku, Fill = (i % 2 == 0 ? kolor2 : kolor1) };
                tablicaPol[numer].Obszar.MouseLeftButtonDown += KlasaReguly.WykonajRuchLubBicie;
                //tablicaPol[numer].Obszar.MouseLeftButtonDown += new MouseButtonEventHandler( (sender, e) => { MessageBox.Show("sdsd"); });
                Canvas.SetLeft(tablicaPol[numer].Obszar, (DlugoscBoku) * i);
                Canvas.SetTop(tablicaPol[numer].Obszar, (DlugoscBoku) * numerWiersza);
                PoleGry.Children.Add(tablicaPol[numer++].Obszar);
            }
        }

       
    }

    class WybranePola
    {
        public Pole WybranePole { get; set; }
        public Brush Kolor { get; set; }
        public Pionek ZbijanyPionek { get; set; }
    }
 
    class Pole
    {
        private static int kolumna, wiersz;
        private Rectangle obszar;

        public int Kolumna { private set; get; }
        public int Wiersz { private set; get; }

        public Rectangle Obszar
        {
            set
            {
                obszar = value;
                ustawBokiPola(obszar);
            }

            get
            {
                if (obszar != null)
                    return obszar;
                return new Rectangle();
            }
        }

        public double Bok
        {
            get
            {
                if(Obszar != null)
                    return Obszar.Width;
                return 0d;
            }
        }

        public Pole()
            :this(null)
        {
        }

        static Pole()
        {
            kolumna = 0;
            wiersz = 1;
        }

        public Pole(Rectangle obszar)
        {
            this.obszar = obszar;
            if(kolumna++ >= 8)
            {
                kolumna = 1;
                wiersz++;
            }
            Kolumna = kolumna;
            Wiersz = wiersz;
        }

        private void ustawBokiPola(Rectangle zmienianyObszar)
        {
            if (zmienianyObszar.Width > zmienianyObszar.Height)
                zmienianyObszar.Height = zmienianyObszar.Width;
            else if (zmienianyObszar.Height > zmienianyObszar.Width)
                zmienianyObszar.Width = zmienianyObszar.Height;
        }

        public override string ToString()
        {
            return "Pole Kolumna " + Kolumna + " Wiersz " + Wiersz;
        }

    }
}

