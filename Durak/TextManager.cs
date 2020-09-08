using System;
using SFML.Graphics;
using SFML.System;

namespace Durak
{
    public class TextManager
    {
        private readonly Font _debugFont;

        public Vector2f Scale { get; set; } = new Vector2f(1, 1);

        public TextManager(string debugFontPath)
        {
            _debugFont = new Font(debugFontPath);
        }
        
        
        public void WriteDebug(string content, Vector2f position, RenderTarget target)
        {
            var text = new SFML.Graphics.Text(content, _debugFont)
            {
                Position = position,
                FillColor = Color.Green,
                CharacterSize = 8,
                Scale = Scale,
                OutlineColor = Color.Black,
                OutlineThickness = 1.0f
            };
                
            text.Draw(target, RenderStates.Default);
        }

        public void Draw(SFML.Graphics.Text text, RenderTarget target)
        {
            text.Draw(target, RenderStates.Default);
        }
       
    }
}