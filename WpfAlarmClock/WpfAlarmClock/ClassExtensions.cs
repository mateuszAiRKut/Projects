using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfAlarmClock
{
    namespace ExtensionMethods
    {
        public static class ClassExtensions
        {
            public static string GetFileNameWithoutPath(this string path, char separator)
            {
                int index = path.IndexOf(separator);
                if (index != -1)
                    return path.Substring(index + 1, path.Length - index - 1).GetFileNameWithoutPath(separator);
                else
                    return path;
            }

            public static object FindValueAndIndex<T>(this List<T> list, Func<T, bool> action)
            {
                int index = -1;
                while(++index < list.Count)
                {
                    if (action(list[index]))
                        return new { AnonymousKey = index, AnonymousValue = list[index] };
                }
                return null;
            }

            public static object FindValueAndIndex<T>(this List<T> list, Predicate<T> action)
            {
                int index = list.FindIndex(action);
                T value = list.Find(action);
                return new { AnonymousKey = index, AnonymousValue = value };
            }

            public static int FindIndex<T>(this ObservableCollection<T> oCollection, Predicate<T> action)
            {
                int index = -1;
                while (++index < oCollection.Count)
                {
                    if (action(oCollection[index]))
                        return index;
                }
                return -1;
            }

            public static void RemoveAll<T>(this ObservableCollection<T> oCollection, Predicate<T> action)
            {
                int index = 0;
                while (index < oCollection.Count)
                {
                    if (action(oCollection[index]))
                        oCollection.RemoveAt(index);
                    else
                        index++;
                }
            }

            public static void Sort<T>(this ObservableCollection<T> oCollection, Comparison<T> compare)
            {
                T temp;
                for (int i = 0; i < oCollection.Count; i++)
                {
                    for (int j = 1; j < oCollection.Count - i; j++)
                    {
                        if (compare(oCollection[j-1], oCollection[j]) < 0)
                        {
                            temp = oCollection[j - 1];
                            oCollection[j - 1] = oCollection[j];
                            oCollection[j] = temp;
                        }
                    }
                }
            }
        }
    }
}
