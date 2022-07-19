using SFML.Graphics;
using SFML.System;

namespace Durak
{
    // TODO: Revise this stupid thing...
    public class TextureCache
    {
        public Texture CardsTexture { get; set; }
        private Vector2i CardSize { get; set; } = new Vector2i(144, 192);
        
        public void LoadCardTextures(string filePath)
        {
            CardsTexture = new Texture(filePath);
        }

        public IntRect GetCardIntRect(CardSuit suit, CardValue value)
        {
            return new IntRect(
                new Vector2i(CardSize.X * (int)value, CardSize.Y * (int)suit),
                CardSize
            );
        }
    }
}
