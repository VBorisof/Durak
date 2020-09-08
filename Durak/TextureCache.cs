using SFML.Graphics;
using SFML.System;

namespace Durak
{
    public class TextureCache
    {
        public Texture CardsTexture { get; set; }
        
        public void LoadCardTextures(string filePath)
        {
            CardsTexture = new Texture(filePath);
        }

        public IntRect GetCardIntRect(CardSuit suit, CardValue value)
        {
            return new IntRect(new Vector2i(62 * (int)value, 81 * (int)suit), new Vector2i(62, 81));
        }
    }
}