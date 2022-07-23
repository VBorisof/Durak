using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

#nullable enable

namespace Durak
{
    public abstract class Player : Drawable
    {
        public string Name { get; set; } = "Unnamed";
        public Game? Game { get; set; }
        public List<Card> PlayerCards { get; } = new List<Card>();
        public Player? Next { private get; set; } = null;
        public Vector2f Position { get; set; } = new Vector2f();

        public PlayerState PlayerState = PlayerState.Waiting;

        public event EventHandler<PlayerPickedCardEventArgs> PlayerPickedCard = (_, __) => {};
        public void OnPlayerPickedCard(PlayerPickedCardEventArgs e)
        {
            var handler = PlayerPickedCard;
            handler(this, e);
        }

        public Player? GetNextWithCards()
        {
            var current = Next;
            while (current != this)
            {
                if (current?.PlayerCards.Count > 0)
                {
                    return current;
                }
                current = current?.Next;
            }
            return null;
        }
        
        public Player? GetNext()
        {
            return Next;
        }

        public virtual void Update()
        {
            for (int i = 0; i < PlayerCards.Count; ++i)
            {
                PlayerCards[i].Sprite.Position = Position + new Vector2f(i * 100, 100);
                if (PlayerCards[i].IsSelected)
                {
                    PlayerCards[i].Sprite.Position -= new Vector2f(0, 20);
                }
            }
        }
        
        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var card in PlayerCards)
            {
                card.Draw(target, states);
            }
        }

        public virtual void SwitchState(PlayerState newState)
        {
            PlayerState = newState;
        }
    }
}

