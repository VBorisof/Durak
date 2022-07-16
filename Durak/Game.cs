using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Durak
{
    public class TurnResult
    {
        public bool IsGameOver { get; set; }
    }

    public enum GameState
    {
        Initialized,
        Started,
        Over
    }

    public class Game
    {
        public GameState GameState { get; set; }

        public Deck Deck { get; set; }
        public Card? LastCard { get; set; }
        public CardSuit Trump { get; set; }

        public List<Player> Players { get; set; }

        public Player? Attacker { get; set; }
        public Player? LastLoser { get; set; }

        public const int MIN_PLAYER_CARDS = 6;

        public Game()
        {
            Deck = new Deck(1);
            Players = new List<Player>();

            GameState = GameState.Initialized;
        }

        public bool AddPlayer(Player player)
        {
            if (GameState == GameState.Started)
            {
                return false;
            }

            if (Players.Count >= (Deck.Cards.Count % MIN_PLAYER_CARDS))
            {
                // Not enough cards to go around.
                return false;
            }

            player.Game = this;
            Players.Add(player);
            if (Players.Count > 1)
            {
                Players.ElementAt(Players.Count - 2).Next = player;
                player.Next = Players.First();
            }

            return true;
        }

        public bool Deal()
        {
            if (GameState == GameState.Started)
            {
                return false;
            }

            if (Players.Count < 2)
            {
                return false;
            }

            InitDeck();

            // See who's got lowest trump, otherwise lowest card. They will go first.
            Attacker = Players.Where(p => p.PlayerCards.Any(c => c.Suit == Trump))
                .OrderBy(p => p.PlayerCards.Where(c => c.Suit == Trump).Min(c => c.Value))
                .FirstOrDefault();

            if (Attacker == null)
            {
                Attacker = Players
                    .OrderBy(p => p.PlayerCards.Min(c => c.Value))
                    .First();
            }

            GameState = GameState.Started;
            return true;
        }


        public void Restart()
        {
            InitDeck();

            Attacker = Players.Single(p => p.GetNext() == LastLoser);

            GameState = GameState.Started;
        }

        public TurnResult Move()
        {
            // TODO: No check for no-cards players

            var originalAttacker = Attacker;
            var originalDefender = Attacker?.GetNextWithCards();

            var attackCard = originalAttacker?.Attack();
            Console.WriteLine($"{originalAttacker?.Name} moves with {attackCard}...");

            var nextPlayerIndex = Players.IndexOf(originalDefender!);
            var defendCard = originalDefender?.Defend(attackCard);
            Console.WriteLine($"{originalDefender?.Name} defends with {(defendCard == null ? "NOTHING" : defendCard.ToString())}");
            
            if (defendCard == null)
            {
                // Means he's taken and will skip turn.
                Attacker = originalDefender?.GetNextWithCards();
            }
            else
            {
                Attacker = Attacker?.GetNextWithCards();
            }

            Console.WriteLine("Turn ends.");

            DrawCards(originalAttacker!, originalDefender!);

            Console.WriteLine("Cards Drawn.");

            var playersWithCards = Players.Where(p => p.PlayerCards.Count() > 0);
            if (playersWithCards.Count() == 1)
            {
                LastLoser = playersWithCards.First();
                Console.WriteLine($"Game over. Player {LastLoser.Name} lost.");
                GameState = GameState.Over;
                return new TurnResult { IsGameOver = true };
            }

            Console.WriteLine($"Next turn: {Attacker?.Name} attacks {Attacker?.GetNextWithCards().Name}");
            return new TurnResult { IsGameOver = false };
        }

        private void DrawCards(Player first, Player last)
        {
            // Let first player draw first.
            PlayerDrawCards(first);

            // Go through all players except first and last.
            var current = first.GetNext();
            
            // TODO: This is incorrect. This causes infinite loops 
            // if `first` has no cards. Maybe an extra method to pull true first?
            // Done. See if it's fixed now...
            while (current != first)
            {
                // Skip last player. Draw cards otherwise.
                if (current != last)
                {
                    PlayerDrawCards(current);
                }
                current = current.GetNext();
            }

            // Let last player draw.
            PlayerDrawCards(last);
        }

        private void PlayerDrawCards(Player player)
        {
            for (
                int i = player.PlayerCards.Count();
                Deck.Cards.Count() > 0 && i < MIN_PLAYER_CARDS;
                ++i
            )
            {
                player.PlayerCards.Add(Deck.PopFirst());
            }
        }

        private void InitDeck()
        {
            foreach(var player in Players)
            {
                player.PlayerCards.Clear();
            }
            Deck = new Deck();
            Deck.Shuffle();
            
            LastCard = Deck.GetLast();
            Trump = LastCard.Suit;

            for (int i = 0; i < MIN_PLAYER_CARDS; ++i)
            {
                foreach(var player in Players)
                {
                    player.PlayerCards.Add(Deck.PopFirst());
                }
            }
        }
    }
}

