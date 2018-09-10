using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAppHoliday.Logic
{
    static class ClassExtensions
    {
        public static IEnumerable<TabItem> ReverseExt(this ItemCollection itemC)
        {
            for (int i=itemC.Count-1; i>=0; i--)
            {
                yield return (TabItem)itemC[i];
            }
        }

        public static TabItem WhereExt(this ItemCollection itemc, Func<TabItem,bool> predicate)
        {
            foreach(TabItem tab in itemc)
            {
                if (predicate(tab))
                    return tab;
            }
            return null;
        }
    }
}
