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

        public override string ToString()
        {
            char value = '-';
            switch(Value)
            {
                case CardValue.Two:
                    value = '2';
                    break;
                case CardValue.Three:
                    value = '3';
                    break;
                case CardValue.Four:
                    value = '4';
                    break;
                case CardValue.Five:
                    value = '5';
                    break;
                case CardValue.Six:
                    value = '6';
                    break;
                case CardValue.Seven:
                    value = '7';
                    break;
                case CardValue.Eight:
                    value = '8';
                    break;
                case CardValue.Nine:
                    value = '9';
                    break;
                case CardValue.Ten:
                    value = '0';
                    break;
                case CardValue.Jack:
                    value = 'J';
                    break;
                case CardValue.Queen:
                    value = 'Q';
                    break;
                case CardValue.King:
                    value = 'K';
                    break;
                case CardValue.Ace:
                    value = 'A';
                    break;
            }

            char suit = '-';
            switch(Suit)
            {
                case CardSuit.Spades:
                    suit = '♠';
                    break;
                case CardSuit.Clubs:
                    suit = '♣';
                    break;
                case CardSuit.Diamonds:
                    suit = '♦';
                    break;
                case CardSuit.Hearts:
                    suit = '♥';
                    break;
            }
            return $"{value}{suit}";
        }
    }
}
