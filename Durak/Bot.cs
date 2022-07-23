using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Durak
{
    public class Bot : Player
    {
        public List<Card> Memory { get; set; } = new List<Card>();
        private Random _random = new Random();

        public override void Update()
        {
            base.Update();
        }

        public Card Attack()
        {
            // Naive implementation: select weakest card.

            var card = PlayerCards
                // First, push trumps away.
                .OrderByDescending(c => (c.Suit == Game?.Trump ? 0 : 1))
                // Then Sort, lowest first.
                .ThenBy(c => c.Value)
                // And grab the first one.
                .First();

            return card;
        }

        public Card? Defend(Card against)
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

                return card;
            }

            // Try trumps.
            var trumpStrongerCards =
                PlayerCards.Where(c => 
                    against.Suit != Game?.Trump
                    && c.Suit == Game?.Trump
                    && c.Value > against.Value
                );

            if (trumpStrongerCards.Any())
            {
                var card = trumpStrongerCards
                    .OrderBy(c => c.Value)
                    .First();

                return card;
            }

            // Take it otherwise.
            return null;
        }

        public override void SwitchState(PlayerState newState)
        {
            base.SwitchState(newState);
            switch (PlayerState)
            {
                case PlayerState.Attacking:
                    OnPlayerPickedCard(
                        new PlayerPickedCardEventArgs(
                            this, Attack()
                        )
                    );
                    break;

                case PlayerState.Defending:
                    OnPlayerPickedCard(
                        new PlayerPickedCardEventArgs(
                            this, Defend(Game?.AttackingCard!)
                        )
                    );
                    break;

                case PlayerState.Waiting:
                default:
                    break;
            }
        }
    }
}

