using System.Collections.Generic;

namespace System.Linq
{
    static class IEnumerableExtensions
    {
        public static IEnumerable<TResult> Append<TResult>(this IEnumerable<TResult> source, TResult element)
        {
            foreach (var sourceElement in source)
                yield return sourceElement;

            yield return element;
        }
    }
}
