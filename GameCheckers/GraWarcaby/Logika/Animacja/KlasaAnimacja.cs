using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GraWarcaby.Logika.Reguly;

namespace GraWarcaby.Logika.Animacja
{
    static class KlasaAnimacja
    {
        private static int numerAnimacjiGracz1, numerAnimacjiGracz2, numerA1, numerA2;
        private static bool[,] tablicaFlag;

        public static Storyboard Animacja { get; set; }
        public static bool Flaga
        {
            set => tablicaFlag[numerA1, numerA2] = value;
        }

        static KlasaAnimacja()
        {
            tablicaFlag = new bool[2,12];
            for(int i=0; i<tablicaFlag.GetLength(0); i++)
            {
                for(int j=0; j<tablicaFlag.GetLength(1); j++)
                    tablicaFlag[i, j] = true;
            }
        }

        private static bool sprawdz(GraczWybor gracz)
        {
            for (int i = 0; i < tablicaFlag.GetLength(0); i++)
            {
                for (int j = 0; j < tablicaFlag.GetLength(1); j++)
                {
                    if (!tablicaFlag[i, j] || KlasaReguly.Gracz != gracz)
                        return false;
                }                  
            }
            return true;
        }

        private static string dajNazweAnimacjj(SolidColorBrush animacja)
        {
            return "mojaAnimacjaPionek" + ((animacja.Color == Colors.Red) ? "Czerwony" + numerAnimacjiGracz1++ : "Bialy" + numerAnimacjiGracz2++);
        }

        private static DoubleAnimation dajAnimacjeOpacity()
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.To = 0.0;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.7);
            opacityAnimation.AutoReverse = true;
            opacityAnimation.RepeatBehavior = RepeatBehavior.Forever;
            return opacityAnimation;
        }

        private static void dodajAnimacjePola(Rectangle pole, Pole[] tablicaPol)
        {
            Pole p = tablicaPol[KlasaPlansza.WyodrebnijLiczbe(pole.Name)];
            pole.MouseLeftButtonDown += (sender, e) => { p.Obszar.Fill = Brushes.Blue; };
        }

        public static void dodajAnimacje(Ellipse obszar, Canvas panel)
        {
            SolidColorBrush mojaAnimacja = new SolidColorBrush();
            GraczWybor kolorGracz = (obszar.Fill == Brushes.Red) ? GraczWybor.Czerwony: GraczWybor.Bialy;
            mojaAnimacja.Color = (obszar.Fill == Brushes.Red) ? Colors.Red : Colors.White;
            obszar.Fill = mojaAnimacja;
            string nazwaAnimacji = dajNazweAnimacjj(mojaAnimacja);

            MainWindow.DajKontekst.RegisterName(nazwaAnimacji, mojaAnimacja);
            DoubleAnimation animacjaOpacity = dajAnimacjeOpacity();

            Storyboard.SetTargetName(animacjaOpacity, nazwaAnimacji);
            Storyboard.SetTargetProperty(
                animacjaOpacity, new PropertyPath(SolidColorBrush.OpacityProperty));
            Storyboard mouseEnterStoryboard = new Storyboard();
            mouseEnterStoryboard.Children.Add(animacjaOpacity);
            obszar.MouseLeftButtonDown += (sender, e) =>
            {
                int numer = mojaAnimacja.Color == Colors.Red ? 0 : 1, numer2 = KlasaPlansza.WyodrebnijLiczbe(nazwaAnimacji);
                if (tablicaFlag[numer, numer2])
                {
                    if (!sprawdz(kolorGracz))
                        return;
                    mouseEnterStoryboard.Begin(obszar, true);
                    tablicaFlag[numer, numer2] = false;
                    Point p = e.GetPosition(panel);
                    KlasaPlansza.NaJakimPoluStoiPionek(p, kolorGracz);
                    Animacja = mouseEnterStoryboard;
                    numerA1 = numer;
                    numerA2 = numer2;
                }
                else
                {
                    if (KlasaReguly.WTrakcieBicia)
                        return;
                    mouseEnterStoryboard.Remove(obszar);
                    tablicaFlag[numer, numer2] = true;
                    KlasaReguly.UsunMozliweRuchy();
                }
            };

        }
    }
}
