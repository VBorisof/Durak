using System;
using System.Collections.Generic;

namespace Durak
{
    /// TODO: This comparer sometimes throws this exception:
    /// Unhandled exception.
    /// System.ArgumentException: Unable to sort because the IComparer.Compare()
    /// method returns inconsistent results. Either a value does not compare equal to itself,
    /// or one value repeatedly compared to another value yields different results.
    /// IComparer: 'Durak.RandomCardComparer'.
    public class RandomCardComparer : IComparer<Card>
    {
        private readonly Random _random = new Random();
        
        public int Compare(Card x, Card y)
        {
            return _random.Next(-1, 2);
        }
    }
}
