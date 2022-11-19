using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppWorldCup.Interfejsy
{
    public interface IWyborKoloru
    {
        Brush WybranyKolor { get; set; }
        event SelectionChangedEventHandler ZmienionoKolor;
    }
}
