using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Durak
{
    class Engine
    {
        private Game _game = new Game();

        public void Run()
        {
            var _textManager = new TextManager("res/fonts/monaco.ttf");
            var IsDebug = true;
            
            var bot1 = new Bot()
            {
                Position = new Vector2f(100, 200)
            };
            bot1.Name = "1";
            var bot2 = new Bot()
            {
                Position = new Vector2f(100, 400)
            };
            bot2.Name = "2";
            var bot3 = new Bot()
            {
                Position = new Vector2f(100, 600)
            };
            bot3.Name = "3";
            var bot4 = new Bot()
            {
                Position = new Vector2f(100, 800)
            };
            bot4.Name = "4";

            var pc = new PC()
            {
                Name = "Player",
                Position = new Vector2f(100, 700)
            };

            _game.AddPlayer(bot1);
            _game.AddPlayer(bot2);
            _game.AddPlayer(pc);
            
            //game.AddPlayer(bot3);
            //game.AddPlayer(bot4);
            
            _game.Init();

            foreach(var player in _game.Players)
            {
                player.Update();
            }

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

                    if (keyArgs.Code == Keyboard.Key.R)
                    {
                        _game.ResetTable();
                    }

                    pc.HandleKey(keyArgs.Code);
                };
                
                var clock = new Clock();
                int fps = 0;
                while (_window.IsOpen)
                {
                    _window.Clear(new Color(0, 0, 25));
                    
                    if (IsDebug)
                    {
                        _textManager.WriteDebug(
                            $"FPS: {fps}\n"
                            + $"TRUMP: {_game.LastCard}\n"
                            + $"Cards: {_game.Deck.Cards.Count}\n"
                            + $"Current turn: {_game.Attacker?.Name}\n"
                            + $"Turn time: {_game.GetTurnTimeLeft()}\n",
                            new Vector2f(20, 40), 
                            _window
                        );
                    }
                    foreach(var player in _game.Players)
                    {
                        player.Update();
                    }

                    _game.Update();
                    
                    foreach(var player in _game.Players)
                    {
                        player.Draw(_window, RenderStates.Default);
                    }

                    foreach(var card in _game.TableCards)
                    {
                        card.Draw(_window, RenderStates.Default);
                    }
                    
                    _window.DispatchEvents();
                    
                    var elapsed = clock.Restart();
                    fps = (int) (1.0f / elapsed.AsSeconds());
                    
                    _window.Display();  
                }
            }
        }
    }
}
