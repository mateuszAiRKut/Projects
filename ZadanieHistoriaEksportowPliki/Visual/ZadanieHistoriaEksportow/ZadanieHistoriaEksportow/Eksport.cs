using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieHistoriaEksportow
{
    public class Eksport
    {
        public int ID { get; set; }
        public string NazwaEksportu { get; set; }
        public string DataEksportu { get; set; }
        public string NazwaUzytkownika { get; set; }
        public string NazwaLokalu { get; set; }

        public static explicit operator EksportGrid(Eksport e)
        {
            string data, godzina;
            Przetwarzanie.ZamianaNaDateGodzine(data: out data, godzina: out godzina, e.DataEksportu);
            return new EksportGrid() { Nazwa = e.NazwaEksportu, Data = data, Godzina = godzina, Uzytkownik = e.NazwaUzytkownika, Lokal = e.NazwaLokalu };
        }
    }

    public class EksportGrid : IEquatable<EksportGrid>, IComparable<EksportGrid>
    {
        public string Nazwa { get; set; }
        public string Data { get; set; }
        public string Godzina { get; set; }
        public string Uzytkownik { get; set; }
        public string Lokal { get; set; }

        public int CompareTo(EksportGrid other)
        {
            return Nazwa.CompareTo(other.Nazwa);
        }

        public bool Equals(EksportGrid other)
        {
            return Nazwa.Equals(other.Nazwa);
        }

        public override int GetHashCode()
        {
            return Nazwa.GetHashCode();
        }
    }
}
