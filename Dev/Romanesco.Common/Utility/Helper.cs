using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Utility
{
    public static class Helper
    {
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            int index = 0;
            foreach (var item in source)
            {
                if (condition(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
