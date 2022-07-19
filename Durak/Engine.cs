using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Durak
{
    class Engine
    {
        private Game game = new Game();

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

            game.AddPlayer(bot1);
            game.AddPlayer(bot2);
            //game.AddPlayer(bot3);
            //game.AddPlayer(bot4);
            
            game.Init();

            foreach(var player in game.Players)
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
                    if (keyArgs.Code == Keyboard.Key.M)
                    {
                        if (game.GameState != GameState.Started)
                        {
                            return;
                        }
                        game.Move();
                        Console.WriteLine($"[+] Moved.");
                        if (game.GameState == GameState.Over)
                        {
                            Console.WriteLine("[+] Game over.");
                            Console.WriteLine("[-] Hit R to restart. Hit Escape to exit.");
                        }
                    }

                    if (keyArgs.Code == Keyboard.Key.R)
                    {
                        game.ResetTable();
                    }

                    foreach(var player in game.Players)
                    {
                        player.Update();
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
                            $"FPS: {fps}\n"
                            + $"TRUMP: {game.LastCard}\n"
                            + $"Cards: {game.Deck.Cards.Count}\n",
                            new Vector2f(20, 40), 
                            _window
                        );
                    }
                    
                    foreach(var player in game.Players)
                    {
                        player.Draw(_window, RenderStates.Default);
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
