#nullable enable

namespace Durak
{
    public class PlayerPickedCardEventArgs
    {
        public Player Player { get; set; }
        public Card? Card { get; set; }

        public PlayerPickedCardEventArgs(Player player, Card? card)
        {
            Player = player;
            Card = card;
        }
    }
}


