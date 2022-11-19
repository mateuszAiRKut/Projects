using AppWorldCup.Interfejsy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppWorldCup.Widok
{
    public class WyborKoloru : IWyborKoloru
    {
        public Brush WybranyKolor { get; set; }

        public event SelectionChangedEventHandler ZmienionoKolor;

        public void ZmienKolor(SelectionChangedEventArgs e)
        {
            ZmienionoKolor?.Invoke(this, e);
        }
    }
}
