using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.Extensions;

namespace ServiceTests.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> instance)
        {
            int len = instance.Count();
            var rnd = new Random();
            var element = rnd.Next(0, len);
            
            return instance.ToArray()[element];
        }
    }
}