//@BaseCode
//MdStart
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CommonBase.Extensions
{
    public static class ConcurrentBagExtensions
    {
        public static void AddSafe<T>(this ConcurrentBag<T> source, T otherElement)
        {
            source.CheckArgument(nameof(source));
            otherElement.CheckArgument(nameof(otherElement));

                source.Add(otherElement);
        }
        public static IEnumerable<T> AddRangeSafe<T>(this ConcurrentBag<T> source, IEnumerable<T> otherSource)
        {
            source.CheckArgument(nameof(source));
            otherSource.CheckArgument(nameof(otherSource));

            foreach (var item in otherSource)
            {
                source.Add(item);
            }
            return source;
        }
    }
}
//MdEnd