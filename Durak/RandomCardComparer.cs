using System;
using System.Collections.Generic;

namespace Durak
{
    public class RandomCardComparer : IComparer<Card>
    {
        private readonly Random _random = new Random();
        
        public int Compare(Card x, Card y)
        {
            return _random.Next(-1, 2);
        }
    }
}