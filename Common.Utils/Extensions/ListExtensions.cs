using System;
using System.Collections.Generic;

namespace Common.Utils.Extensions
{
    public static class ListExtensions
    {
        public static void Replace<T>(this IList<T> list, T originalItem, T replacementItem)
        {
            Replace(list, originalItem, new List<T> {replacementItem});
        }

        public static void Replace<T>(this IList<T> list, T originalItem, IList<T> replacements)
        {
            var index = list.IndexOf(originalItem);
            if (index < 0) throw new NullReferenceException("The 'originalItem' object " + originalItem + "is not present in 'list'.");

            list.Remove(originalItem);
            foreach (var newUnit in replacements)
                list.Insert(index++, newUnit);
        }
    }
}