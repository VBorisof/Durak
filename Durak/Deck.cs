using System;
using System.Collections.Generic;
using System.Linq;

namespace Durak
{
    public class Deck
    {
        public TextureCache _textureCache;
        private Random _random = new Random();
        
        public List<Card> Cards { get; set; } = new List<Card>();

        public Deck(int numPacks = 1)
        {
            _textureCache = new TextureCache();
            _textureCache.LoadCardTextures("res/cards.png");
            
            for (int i = 0; i < numPacks; ++i)
            {
                foreach (var suit in Enum.GetValues(typeof(CardSuit)))
                {
                    foreach (var value in Enum.GetValues(typeof(CardValue)))
                    {
                        Cards.Add(new Card((CardSuit) suit, (CardValue) value, _textureCache));
                    }
                }
            }
        }

        public void Shuffle()
        {
            Cards.Sort(new RandomCardComparer());
        }

        public Card PopFirst()
        {
            var first = Cards.First();
            Cards.Remove(first);

            return first;
        }

        public Card GetLast()
        {
            return Cards.Last();
        }
    }
}
