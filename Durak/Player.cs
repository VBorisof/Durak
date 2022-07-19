using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Durak
{
    public abstract class Player : Drawable
    {
        public string Name { get; set; }
        public Game Game { get; set; }
        public List<Card> PlayerCards { get; } = new List<Card>();
        public Player Next { private get; set; }
        public Vector2f Position { get; set; } = new Vector2f();

        public Player GetNextWithCards()
        {
            var current = Next;
            while (current != this)
            {
                if (current.PlayerCards.Count > 0)
                {
                    return current;
                }
                current = current.Next;
            }
            return null;
        }
        
        public Player GetNext()
        {
            return Next;
        }

        public virtual void Update()
        {
            for (int i = 0; i < PlayerCards.Count; ++i)
            {
                PlayerCards[i].Sprite.Position = Position + new Vector2f(i * 100/*55*/, 100);
            }
        }
        
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var card in PlayerCards)
            {
                card.Draw(target, states);
            }
        }

        public abstract Card Attack();
        public abstract Card Defend(Card against);
    }
}
