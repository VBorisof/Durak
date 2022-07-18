using System.Collections.Generic;
using System.Linq;

namespace Durak
{
    public class Bot : Player
    {
        public List<Card> Memory { get; set; } = new List<Card>();

        public override void Update()
        {
            base.Update();
        }

        public override Card Attack()
        {
            // Naive implementation: select weakest card.

            var card = PlayerCards
                // First, push trumps away.
                .OrderByDescending(c => (c.Suit == Game.Trump ? 0 : 1))
                // Then Sort, lowest first.
                .ThenBy(c => c.Value)
                // And grab the first one.
                .First();

            PlayerCards.Remove(card);

            return card;
        }

        public override Card Defend(Card against)
        {
            // Naive implementation: select weakest card that can defend.
            
            // Try weakest cards first.
            var sameSuitStrongerCards =
                PlayerCards.Where(c => c.Suit == against.Suit && c.Value > against.Value);

            if (sameSuitStrongerCards.Any())
            {
                var card = sameSuitStrongerCards
                    .OrderBy(c => c.Value)
                    .First();

                PlayerCards.Remove(card);

                return card;
            }

            // Try trumps.
            var trumpStrongerCards =
                PlayerCards.Where(c => 
                    against.Suit != Game.Trump
                    && c.Suit == Game.Trump
                    && c.Value > against.Value
                );

            if (trumpStrongerCards.Any())
            {
                var card = trumpStrongerCards
                    .OrderBy(c => c.Value)
                    .First();

                PlayerCards.Remove(card);

                return card;
            }

            PlayerCards.Add(against);

            // Take it otherwise.
            return null;
        }
    }
}

