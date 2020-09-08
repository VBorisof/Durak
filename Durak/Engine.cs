using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Durak
{
    class Engine
    {
        public void Run()
        {
            var _textManager = new TextManager("res/fonts/monaco.ttf");
            var IsDebug = true;
            
            var deck = new Deck();
            deck.Shuffle();
            
            var player = new Player();
            for (int i = 0; i < 6; ++i)
            {
                player.PlayerCards.Add(deck.PopFirst());
            }
            
            player.Update();
            
            using (var _window = new RenderWindow(new VideoMode(1920, 1080), "Game"))
            {
                _window.Closed += (_, __) => _window.Close();
                _window.SetFramerateLimit(60);

                _window.KeyPressed += (_, keyArgs) =>
                {
                    if (keyArgs.Code == Keyboard.Key.Escape)
                    {
                        _window.Close();
                    }
                };
                
                var clock = new Clock();
                int fps = 0;
                while (_window.IsOpen)
                {
                    _window.Clear(new Color(0, 0, 25));
                    
                    if (IsDebug)
                    {
                        _textManager.WriteDebug(
                            $"FPS: {fps}\n",
                            new Vector2f(20, 40), 
                            _window
                        );
                    }
                    
                    player.Draw(_window, RenderStates.Default);
                    
                    _window.DispatchEvents();
                    
                    var elapsed = clock.Restart();
                    fps = (int) (1.0f / elapsed.AsSeconds());
                    
                    _window.Display();  
                }
            }
        }
    }
}