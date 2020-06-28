using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Try to find the index of the given element in the <see cref="IReadOnlyList{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="elementToFind"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            int i = 0;
            foreach (T element in self)
            {
                if (Equals(element, elementToFind))
                    return i;
                i++;
            }
            return -1;
        }
    }
}
