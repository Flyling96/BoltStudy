using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace AutoBinary
{
    public static class BinaryExtension
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T value)
        {
            if (enumerable != null)
            {
                foreach (var current in enumerable)
                {
                    yield return current;
                }
            }

            yield return value;
        }

        public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, IEnumerable<T> values)
        {
            if (enumerable != null)
            {
                foreach (var current in enumerable)
                {
                    yield return current;
                }
            }
            if (values != null)
            {
                foreach (var value in values)
                {
                    yield return value;
                }
            }
        }
    }
}
