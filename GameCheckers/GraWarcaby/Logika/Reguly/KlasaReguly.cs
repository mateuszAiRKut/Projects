using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GraWarcaby.Logika.Animacja;

namespace GraWarcaby.Logika.Reguly
{
    static class KlasaReguly
    {
        private static List<WybranePola> listaWybranychPol;
        private static List<Pionek> listaZablokowanychPionkow;
        private static int zbiteCzerwone, zbiteBiale;

        public static GraczWybor Gracz { private set; get; }
        public static bool WTrakcieBicia { set; get; }

        static KlasaReguly()
        {
            listaWybranychPol = new List<WybranePola>();
            listaZablokowanychPionkow = new List<Pionek>();
            Gracz = GraczWybor.Bialy;
        }

        private static bool SprawdzCzyKoniecGry()
        {
            return (zbiteCzerwone == 12 || zbiteBiale == 12) || (Gracz == GraczWybor.Czerwony ? listaZablokowanychPionkow.Count == 12-zbiteCzerwone : listaZablokowanychPionkow.Count == 12-zbiteBiale);
        }

        public static void MozliweRuchy(Pole[] tablica, List<Pionek> listaPionkowCzerwony, List<Pionek> listaPionkowBialy, int wiersz, int kolumna, int liczbaWierszy, int liczbaKolumn, bool czyTylkoSprawdzacZbijanie)
        {
            if (wiersz > liczbaWierszy || wiersz < 1)
                return;
            int lewo = kolumna - 1, prawo = kolumna + 1;
            if (lewo >= 1)
                czyMoznaWykonacRuch(tablica, listaPionkowCzerwony, listaPionkowBialy, wiersz, lewo, liczbaWierszy, czyTylkoSprawdzacZbijanie);
            if (prawo <= liczbaKolumn)
                czyMoznaWykonacRuch(tablica, listaPionkowCzerwony, listaPionkowBialy, wiersz, prawo, liczbaWierszy, czyTylkoSprawdzacZbijanie);

            if (listaWybranychPol.Count == 0)
            {
                if (!listaZablokowanychPionkow.Contains(KlasaPlansza.WybranyPionek))
                    listaZablokowanychPionkow.Add(KlasaPlansza.WybranyPionek);
                if (SprawdzCzyKoniecGry())
                    MainWindow.KtoryGracz.Text = "Wygrał Gracz: " + (Gracz == GraczWybor.Czerwony ? "Gracz1" : "Gracz2");
            }
            else
                listaZablokowanychPionkow.Clear();
        }

        public static void MozliweRuchyDamka(Pole[] tablica, List<Pionek> listaPionkowCzerwony, List<Pionek> listaPionkowBialy, int wiersz, int kolumna, int liczbaWierszy, int liczbaKolumn, bool czyTylkoSprawdzacZbijanie)
        {
            if (wiersz > liczbaWierszy || wiersz < 1)
                return;
            int lewo = kolumna - 1, prawo = kolumna + 1;
            if (lewo >= 1)
                czyMoznaWykonacRuchDamka(tablica, listaPionkowCzerwony, listaPionkowBialy, wiersz, lewo, liczbaWierszy, liczbaKolumn, true, czyTylkoSprawdzacZbijanie);
            if (prawo <= liczbaKolumn)
                czyMoznaWykonacRuchDamka(tablica, listaPionkowCzerwony, listaPionkowBialy, wiersz, prawo, liczbaWierszy, liczbaKolumn, false, czyTylkoSprawdzacZbijanie);

            if (listaWybranychPol.Count == 0)
            {
                if (!listaZablokowanychPionkow.Contains(KlasaPlansza.WybranyPionek))
                    listaZablokowanychPionkow.Add(KlasaPlansza.WybranyPionek);
                if (SprawdzCzyKoniecGry())
                    MainWindow.KtoryGracz.Text = "Wygrał Gracz: " + (Gracz == GraczWybor.Czerwony ? "Gracz1" : "Gracz2");
            }
            else
                listaZablokowanychPionkow.Clear();
        }

        private static void czyMoznaWykonacRuch(Pole[] tablica, List<Pionek> listaPionkowCzerwony, List<Pionek> listaPionkowBialy, int wiersz, int kolumna, int liczbaWierszy, bool czyTylkoSprawdzacZbijanie)
        {
            wiersz = (Gracz == GraczWybor.Czerwony) ? wiersz + 1 : wiersz - 1;
            for (int i = 0; i < 2; i++, wiersz = (Gracz == GraczWybor.Czerwony) ? wiersz - 2 : wiersz + 2)
            {
                if (wiersz < 1 || wiersz > liczbaWierszy)
                    continue;
                Pole p = tablica.Where((pole) => pole.Wiersz == wiersz && pole.Kolumna == kolumna).First();
                if (i == 0 && !czyStoiPionekNaTymPolu(p, (Gracz == GraczWybor.Czerwony) ? listaPionkowBialy : listaPionkowCzerwony) && !czyTylkoSprawdzacZbijanie)
                {
                    if (!czyStoiPionekNaTymPolu(p, (Gracz == GraczWybor.Czerwony) ? listaPionkowCzerwony : listaPionkowBialy))
                    {
                        listaWybranychPol.Add(new WybranePola() { Kolor = p.Obszar.Fill, WybranePole = p });
                        p.Obszar.Fill = Brushes.Blue;
                    }
                }
                else
                    czyMoznaZbicPionek(p, (Gracz == GraczWybor.Czerwony) ? listaPionkowBialy : listaPionkowCzerwony, tablica);
            }
        }

        private static void czyMoznaWykonacRuchDamka(Pole[] tablica, List<Pionek> listaPionkowCzerwony, List<Pionek> listaPionkowBialy, int wiersz, int kolumna, int liczbaWierszy, int liczbaKolumn, bool czyLewo, bool czyTylkoSprawdzacZbijanie)
        {
            wiersz = (Gracz == GraczWybor.Czerwony) ? wiersz + 1 : wiersz - 1;
            for (int i = 0; i < 2; i++, wiersz = (Gracz == GraczWybor.Czerwony) ? wiersz - 2 : wiersz + 2)
            {
                Pionek zbijanyPionek = null;
                bool flagaSprawdzania = false;
                for(int wierszPomocniczy = wiersz, kolumnaPomocnicza = kolumna, liczbaPionkowPrzeciwnika = 0; (wierszPomocniczy >= 1 && wierszPomocniczy <= liczbaWierszy) && (kolumnaPomocnicza >= 1 && kolumnaPomocnicza <= liczbaKolumn);
                    wierszPomocniczy = (Gracz == GraczWybor.Bialy) ? (i == 0 ? wierszPomocniczy-1 : wierszPomocniczy+1) :(i == 0 ? wierszPomocniczy +1:wierszPomocniczy-1), kolumnaPomocnicza = (czyLewo? kolumnaPomocnicza-1 : kolumnaPomocnicza+1 ))
                {
                    if (liczbaPionkowPrzeciwnika == 2)
                        break;
                    Pole p = tablica.Where((pole) => pole.Wiersz == wierszPomocniczy && pole.Kolumna == kolumnaPomocnicza).First();
                    if (!czyStoiPionekNaTymPolu(p, (Gracz == GraczWybor.Czerwony) ? listaPionkowBialy : listaPionkowCzerwony))
                    {
                        if (!czyStoiPionekNaTymPolu(p, (Gracz == GraczWybor.Czerwony) ? listaPionkowCzerwony : listaPionkowBialy))
                        {
                            if ((!czyTylkoSprawdzacZbijanie || flagaSprawdzania))
                            {
                                if (liczbaPionkowPrzeciwnika == 1 && zbijanyPionek != null)
                                    listaWybranychPol.Add(new WybranePola() { Kolor = p.Obszar.Fill, WybranePole = p, ZbijanyPionek = zbijanyPionek });
                                else
                                    listaWybranychPol.Add(new WybranePola() { Kolor = p.Obszar.Fill, WybranePole = p });
                                p.Obszar.Fill = Brushes.Blue;
                            }
                        }
                        else
                            break;
                    }
                    else
                    {
                        liczbaPionkowPrzeciwnika++;
                        flagaSprawdzania = true;
                        List<Pionek> lista = (Gracz == GraczWybor.Bialy) ? listaPionkowCzerwony.Where((pionek) => pionek.Wiersz == wierszPomocniczy && pionek.Kolumna == kolumnaPomocnicza).ToList()
                            : listaPionkowBialy.Where((pionek) => pionek.Wiersz == wierszPomocniczy && pionek.Kolumna == kolumnaPomocnicza).ToList();
                        if (lista.Count != 0)
                            zbijanyPionek = lista.First();
                    }
                }
            }
        }

        private static void czyPionekMozeBycDamka(Pionek p)
        {
            if(Gracz == GraczWybor.Czerwony && p.Wiersz == 8)
            {
                p.Obszar.Stroke = Brushes.Black;
                p.Obszar.StrokeThickness = 3;
                p.Damka = true;
            }
            else if(Gracz == GraczWybor.Bialy && p.Wiersz == 1)
            {
                p.Obszar.Stroke = Brushes.Black;
                p.Obszar.StrokeThickness = 3;
                p.Damka = true;
            }
        }

        public static void WykonajRuchLubBicie(object sender, MouseButtonEventArgs e)
        {
            Rectangle obszar = sender as Rectangle;
            if (obszar != null && obszar.Fill == Brushes.Blue)
            {
                Pionek p = KlasaPlansza.WybranyPionek;
                Point punkt = e.GetPosition(KlasaPlansza.PoleGry);
                int kolumna = 0, wiersz = 0;
                for (int i = 0, dol = i * KlasaPlansza.DlugoscBoku, gora = dol + KlasaPlansza.DlugoscBoku; i < 8; i++, dol = i * KlasaPlansza.DlugoscBoku, gora = dol + KlasaPlansza.DlugoscBoku)
                {
                    if (punkt.X >= dol && punkt.X <= gora)
                        kolumna = (i + 1);
                    else if (punkt.Y >= dol && punkt.Y <= gora)
                        wiersz = (i + 1);
                }
                WybranePola wybor = listaWybranychPol.Where((wybrane) => wybrane.WybranePole.Wiersz == wiersz && wybrane.WybranePole.Kolumna == kolumna).First();
                Canvas.SetLeft(p.Obszar, Canvas.GetLeft(p.Obszar) + KlasaPlansza.DlugoscBoku * (wybor.WybranePole.Kolumna - p.Kolumna));
                Canvas.SetTop(p.Obszar, Canvas.GetTop(p.Obszar) + KlasaPlansza.DlugoscBoku * (wybor.WybranePole.Wiersz - p.Wiersz));
                p.Kolumna = wybor.WybranePole.Kolumna;
                p.Wiersz = wybor.WybranePole.Wiersz;
                if (wybor.ZbijanyPionek != null)
                {
                    KlasaPlansza.PoleGry.Children.Remove(wybor.ZbijanyPionek.Obszar);
                    KlasaPlansza.UsunPionek(wybor.ZbijanyPionek);
                    if (Gracz == GraczWybor.Bialy)
                        MainWindow.ZbitePionkiCzerwone.Text = "x" + (++zbiteCzerwone);
                    else
                        MainWindow.ZbitePionkiBiale.Text = "x" + (++zbiteBiale);
                    UsunMozliweRuchy();
                    Point nowyPunkt = e.GetPosition(KlasaPlansza.PoleGry);
                    KlasaPlansza.NaJakimPoluStoiPionek(nowyPunkt, Gracz, true);
                    WTrakcieBicia = true;
                    if (listaWybranychPol.Count != 0)
                        return;
                }
                KlasaAnimacja.Animacja.Remove(p.Obszar);
                KlasaAnimacja.Flaga = true;
                UsunMozliweRuchy();
                WTrakcieBicia = false;
                if (SprawdzCzyKoniecGry())
                    MainWindow.KtoryGracz.Text = "Wygrał Gracz: " + (Gracz == GraczWybor.Czerwony ? "Gracz2" : "Gracz1");
                else
                {
                    czyPionekMozeBycDamka(KlasaPlansza.WybranyPionek);
                    Gracz = (Gracz == GraczWybor.Czerwony) ? GraczWybor.Bialy : GraczWybor.Czerwony;
                    MainWindow.KtoryGracz.Text = "Ruch: " + (Gracz == GraczWybor.Czerwony ? "Gracz2" : "Gracz1");
                    listaZablokowanychPionkow.Clear();
                }

            }
        }

        private static bool czyStoiPionekNaTymPolu(Pole p, List<Pionek> tablica)
        {
            return tablica.Where((pionek) => pionek.Wiersz == p.Wiersz && pionek.Kolumna == p.Kolumna).Count() != 0;
        }

        private static void czyMoznaZbicPionek(Pole p, List<Pionek> tablica, Pole[] tablicaPol)
        {
            List<Pionek> lista = tablica.Where((pionek) => pionek.Wiersz == p.Wiersz && pionek.Kolumna == p.Kolumna).ToList();
            if (lista.Count == 0)
                return;
            int kolumnaPionkaZbijanego = lista.First().Kolumna, wierszPionkaZbijanego = lista.First().Wiersz;
            int kolumnaWolnegoPola = (KlasaPlansza.WybranyPionek.Kolumna > kolumnaPionkaZbijanego) ? kolumnaPionkaZbijanego - 1 : kolumnaPionkaZbijanego + 1;
            int wierszWolnegoPola = (KlasaPlansza.WybranyPionek.Wiersz > wierszPionkaZbijanego) ? wierszPionkaZbijanego - 1 : wierszPionkaZbijanego + 1;
            if (KlasaPlansza.czyWolnePole(kolumnaWolnegoPola, wierszWolnegoPola))
            {
                Pole wolnePole = tablicaPol.Where((pole) => pole.Wiersz == wierszWolnegoPola && pole.Kolumna == kolumnaWolnegoPola).First();
                listaWybranychPol.Add(new WybranePola() { Kolor = wolnePole.Obszar.Fill, WybranePole = wolnePole, ZbijanyPionek = lista.First() });
                wolnePole.Obszar.Fill = Brushes.Blue;
            }
        }

        public static void UsunMozliweRuchy()
        {
            if (listaWybranychPol == null && listaWybranychPol.Count == 0)
                return;
            foreach (WybranePola p in listaWybranychPol)
                p.WybranePole.Obszar.Fill = p.Kolor;
            listaWybranychPol.Clear();
            /*for(int i=listaWybranychPol.Count-1; i >=0; i--)
            {
                listaWybranychPol[i].WybranePole.Obszar.Fill = listaWybranychPol[i].Kolor;
                listaWybranychPol.RemoveAt(i);
            }*/
        }
    }
}
