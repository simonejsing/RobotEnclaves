using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> projection)
        {
            foreach (var element in collection)
            {
                projection(element);
            }
        }

        public static T ArgMin<T>(this IEnumerable<T> collection, Func<T, float> projection)
        {
            if (!collection.Any())
            {
                throw new ArgumentException("Collection is empty");
            }

            var minimizingElement = collection.First();
            var minimumValue = projection(minimizingElement);

            foreach (var element in collection.Skip(1))
            {
                var elementValue = projection(element);
                if (minimumValue > elementValue)
                {
                    minimizingElement = element;
                    minimumValue = elementValue;
                }
            }
            return minimizingElement;
        }

    }
}
