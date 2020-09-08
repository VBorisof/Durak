using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Durak
{
    public class Player : Drawable
    {
        public List<Card> PlayerCards { get; set; } = new List<Card>();

        public void Update()
        {
            for (int i = 0; i < PlayerCards.Count; ++i)
            {
                PlayerCards[i].Sprite.Position = new Vector2f(700 + i * 55, 700);
            }
        }
        
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var card in PlayerCards)
            {
                card.Draw(target, states);
            }
        }
    }
}