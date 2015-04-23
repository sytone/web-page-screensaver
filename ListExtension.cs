using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageScreensaver
{
    public static class ListExtension
    {
        public static T Next<T>(this IList<T> list, T item)
        {
            if (list.IndexOf(item) == -1)
            {
                return list[0];
            }

            var nextIndex = list.IndexOf(item) + 1;

            if (nextIndex == list.Count)
            {
                return list[0];
            }

            return list[nextIndex];
        }
    }
}
