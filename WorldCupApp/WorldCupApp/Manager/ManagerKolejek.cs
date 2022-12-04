using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupApp.Manager
{
    public static class ManagerKolejek
    {
        public static IEnumerable<IEnumerable<T>> GenerujMeczeDlaGrupy<T>(IEnumerable<T> druzyny)
            where T : class
        {
            T[] paraMeczowa = new T[2];
            int liczbaDruzyn = druzyny.Count(), indeksDruzyny = 0, indeks = 0;
            T? druzyna = null;

            var stosDruzyn = new Stack<T>();
            stosDruzyn.Push(druzyny.First());

            while (stosDruzyn.Count > 0)
            {
                indeks = stosDruzyn.Count - 1;
                druzyna = stosDruzyn.Pop();
                indeksDruzyny = druzyny.IndeksElementu(druzyna);

                while (druzyna != default(T))
                {
                    paraMeczowa[indeks++] = druzyna;

                    if (++indeksDruzyny < liczbaDruzyn)
                    {
                        druzyna = druzyny.ElementAt(indeksDruzyny);
                    }
                    else
                    {
                        druzyna = null;
                    }

                    stosDruzyn.Push(druzyna);

                    if (indeks == paraMeczowa.Length)
                    {
                        yield return paraMeczowa;
                        break;
                    }
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> GenerujMeczeDlaGrupy<T>(IEnumerable<T> druzyny, int liczba = 2)
        {
            for (int i = 0; i < druzyny.Count(); i++)
            {
                if (liczba == 1)
                    yield return new T[] { druzyny.ElementAt(i) };
                else
                {
                    foreach (var result in GenerujMeczeDlaGrupy(druzyny.Skip(i + 1), liczba - 1))
                        yield return new T[] { druzyny.ElementAt(i) }.Concat(result);
                }
            }
        }

        public static int IleMeczy<T>(IEnumerable<T> druzyny) => druzyny.Count().Silnia() / (2.Silnia() * (druzyny.Count() - 2).Silnia());
    }

    static class KlasaRozszerzen
    {
        public static int IndeksElementu<T>(this IEnumerable<T> kolekcja, T wartosc)
        {
            return kolekcja
                .Select((a, i) => (a.Equals(wartosc)) ? i : -1)
                .Max();
        }

        public static int Silnia(this int wartosc)
        {
            if (wartosc == 0)
                return 1;

            return wartosc * Silnia(wartosc - 1);
        }
    }

}
