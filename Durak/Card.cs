using SFML.Graphics;
using SFML.System;

namespace Durak
{
    public class Card : Drawable
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }

        public Sprite Sprite { get; set; }

        public Card(CardSuit suit, CardValue value, TextureCache textureCache)
        {
            Suit = suit;
            Value = value;

            Sprite = new Sprite(textureCache.CardsTexture, textureCache.GetCardIntRect(suit, value))
            {
                Position = new Vector2f(800, 600)
            };
        }
        
        
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite);
        }
    }
}