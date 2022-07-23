using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.System;

#nullable enable

namespace Durak
{
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

        public Card? AttackingCard { get; set; } = null;

        // Cards currently on the table.
        public List<Card> TableCards { get; set; } = new List<Card>();
        private Vector2f _tablePos = new Vector2f(800, 400);

        public List<Card> Beaten { get; set; } = new List<Card>();

        public const int turn_time_seconds = 30;
        private Stopwatch _sw;

        public Game()
        {
            Deck = new Deck(1);
            Players = new List<Player>();

            GameState = GameState.Initialized;

            _sw = new Stopwatch();
        }

        public int GetTurnTimeLeft()
        {
            return turn_time_seconds - _sw.Elapsed.Seconds;
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

            player.PlayerPickedCard += OnPlayerPickedCard;

            return true;
        }

        public bool Init()
        {
            if (GameState == GameState.Started)
            {
                return false;
            }

            if (Players.Count < 2)
            {
                return false;
            }

            ResetTable();

            return true;
        }

        public void ResetTable()
        {
            InitDeck();

            // TODO: We have some trouble with initial state
            // setting if the PC is next.
            Attacker = ComputeAttacker();

            GameState = GameState.Started;
        }

        public void Update()
        {
            // Query attacker for cards.
            
            
            // Query defender for defence.


            // Give option for everything to go to beaten when appropriate.
        

            // Check the timer and see if we need to interfere.
        }

        private void DrawCards(Player first, Player last)
        {
            // Let first player draw first.
            PlayerDrawCards(first);

            // Go through all players except first and last.
            var current = first.GetNext();
            
            while (current != first)
            {
                // Skip last player. Draw cards otherwise.
                if (current != last)
                {
                    PlayerDrawCards(current!);
                }
                current = current?.GetNext();
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

        private Player ComputeAttacker()
        {
            Player? attacker;
            if (LastLoser != null)
            {
                attacker = Players.Single(p => p.GetNext() == LastLoser);
            }
            else
            {
                // See who's got lowest trump, otherwise lowest card. They will go first.
                attacker = Players.Where(p => p.PlayerCards.Any(c => c.Suit == Trump))
                    .OrderBy(p => p.PlayerCards.Where(c => c.Suit == Trump).Min(c => c.Value))
                    .FirstOrDefault();

                if (attacker == null)
                {
                    attacker = Players
                        .OrderBy(p => p.PlayerCards.Min(c => c.Value))
                        .First();
                }
            }

            attacker.SwitchState(PlayerState.Attacking);
            return attacker;
        }

        private void OnPlayerPickedCard(object? sender, PlayerPickedCardEventArgs args)
        {
            Console.WriteLine($"OnPlayerPickedCard({args.Player.Name}, {args.Card})");
            Console.WriteLine($"This player is currently {args.Player.PlayerState.ToString()}");
            Console.WriteLine($"AttackingCard: {AttackingCard}.");
            switch(args.Player.PlayerState)
            {
                case PlayerState.Attacking:
                    if (args.Card == null)
                    {
                        break;
                    }
                    OnAttackerPickedCard(args.Card);
                    args.Player.PlayerCards.Remove(args.Card);
                    Attacker?.GetNextWithCards()?.SwitchState(PlayerState.Defending);
                    break;

                case PlayerState.Defending:
                    if (args.Card == null)
                    {
                        args.Player.PlayerCards.Add(AttackingCard!);
                        TurnEnd(args.Player, false);
                        break;
                    }

                    // Validate the card is eligible for defense.
                    if (
                        IsEligibleToDefend(
                            card: args.Card,
                            against: AttackingCard!,
                            trump: Trump
                        )
                    )
                    {
                        Console.WriteLine("Defense successful.");
                        OnDefenderPickedCard(args.Card);
                        args.Player.PlayerCards.Remove(args.Card);

                        // Successfully defended. End the turn for now.
                        TurnEnd(args.Player, true);
                    }
                    break;

                case PlayerState.Waiting:
                default:
                    break;
            }
        }

        private void OnAttackerPickedCard(Card card)
        {
            card.IsSelected = false;
            card.Sprite.Position = _tablePos + new Vector2f(TableCards.Count * 50, 0);

            TableCards.Add(card);
            AttackingCard = card;
        }

        private void OnDefenderPickedCard(Card card)
        {
            card.IsSelected = false;
            card.Sprite.Position = _tablePos + new Vector2f(TableCards.Count * 50 + 20, -20);

            TableCards.Add(card);
        }

        private bool IsEligibleToDefend(Card card, Card against, CardSuit trump)
        {
            if (card.Suit == against.Suit)
            {
                return card.Value > against.Value;
            }

            // Implies the `against` is not trump. Auto-eligible
            if (card.Suit == trump)
            {
                return true;
            }

            return false;
        }

        private void TurnEnd(Player defender, bool wasDefenceSuccessful)
        {
            var originalAttacker = Attacker;

            originalAttacker?.SwitchState(PlayerState.Waiting);
            defender.SwitchState(PlayerState.Waiting);

            if (wasDefenceSuccessful)
            {
                Attacker = defender;
            }
            else
            {
                Attacker = defender.GetNextWithCards();
            }

            Console.WriteLine("Turn ends.");

            DrawCards(originalAttacker!, defender);

            Console.WriteLine("Cards Drawn.");

            var playersWithCards = Players.Where(p => p.PlayerCards.Count() > 0);
            if (playersWithCards.Count() == 1)
            {
                LastLoser = playersWithCards.First();
                Console.WriteLine($"Game over. Player {LastLoser.Name} lost.");
                GameState = GameState.Over;
                return;
            }
            if (playersWithCards.Count() == 0)
            {
                Console.WriteLine($"Game over. It's a draw!");
                GameState = GameState.Over;
                return;
            }

            Attacker?.SwitchState(PlayerState.Attacking);
            var nextDefender = Attacker?.GetNextWithCards();

            Console.WriteLine($"Next turn: {Attacker?.Name} attacks {nextDefender?.Name}");

            TableCards.Clear();

            return;
        }
    }
}

