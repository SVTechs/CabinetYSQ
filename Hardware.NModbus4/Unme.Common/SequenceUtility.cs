using System;
using System.Collections.Generic;
using System.Linq;

namespace Hardware.NModbus4.Unme.Common
{
    internal static class SequenceUtility
    {
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int startIndex, int size)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            var enumerable = source as T[] ?? source.ToArray();
            int num = enumerable.Count();
            if (startIndex < 0 || num < startIndex)
                throw new ArgumentOutOfRangeException("startIndex");
            if (size < 0 || startIndex + size > num)
                throw new ArgumentOutOfRangeException("size");

            return enumerable.Skip(startIndex).Take(size);
        }
    }
}
