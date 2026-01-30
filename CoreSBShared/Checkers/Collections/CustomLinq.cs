using System;
using System.Collections.Generic;
using System.Linq;

namespace InfrastructureCheckers.Collections
{
    public static class CustomLinq
    {
        public static IEnumerable<T> CustomWhere<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items?.Any() != true)
                yield break;

            foreach (var i in items)
                if (predicate(i))
                    yield return i;
        }
    }
}
