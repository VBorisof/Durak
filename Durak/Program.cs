using System;

namespace Durak
{
    class Program
    {
        static void Main(string[] args)
        {
            // new Engine().Run();
            
            var game = new Game();

            var bot1 = new Bot();
            bot1.Name = "1";
            var bot2 = new Bot();
            bot2.Name = "2";
            var bot3 = new Bot();
            bot3.Name = "3";

            game.AddPlayer(bot1);
            game.AddPlayer(bot2);
            game.AddPlayer(bot3);
            
            game.Deal();

            Console.Clear();
            Console.WriteLine("[+] Rock and Roll! Let's play some Durak! Press any key.");
            
            Console.Write($"[-] Order: ");
            var current = game.Attacker;
            do {
                Console.Write($"{current.Name}->{current.GetNextWithCards().Name} |");
                current = current.GetNextWithCards();
            } while (current != game.Attacker);

            ConsoleKey key = Console.ReadKey().Key;
            while (key != ConsoleKey.X)
            {
                Console.Clear();
                
                Console.WriteLine($"== {game.LastCard} == DECK: {game.Deck.Cards.Count} ========================");
                for (int i = 1; i <= game.Players.Count; ++i)
                {
                    Console.Write($"Player {game.Players[i-1].Name} : ");
                    foreach (var card in game.Players[i-1].PlayerCards)
                    {
                        Console.Write($"{card} |");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("==========================================");

                switch(key)
                {
                    case ConsoleKey.M:
                        if (game.GameState != GameState.Started)
                        {
                            break;
                        }
                        var turnResult = game.Move();
                        Console.WriteLine($"[+] Moved.");
                        if (turnResult.IsGameOver)
                        {
                            Console.WriteLine("[+] Game over.");
                            Console.WriteLine("[-] Hit R to restart. Hit X to exit.");
                            continue;
                        }
                        break;

                    case ConsoleKey.R:
                        if (game.GameState == GameState.Initialized)
                        {
                            break;
                        }

                        game.Restart();

                        break;

                }

                key = Console.ReadKey().Key;
            }
            Console.WriteLine("[+] Done!");
        }
    }
}

