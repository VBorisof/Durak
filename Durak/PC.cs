using SFML.Window;

namespace Durak
{
    public class PC : Player
    {
        private int _selectedCardIdx = 0;

        public override void Update()
        {
            base.Update();
        }

        public void HandleKey(Keyboard.Key code)
        {
            var originalIndex = _selectedCardIdx;

            if (code == Keyboard.Key.D)
            {
                if (++_selectedCardIdx >= PlayerCards.Count)
                {
                    _selectedCardIdx = 0;
                }
            }
            if (code == Keyboard.Key.A)
            {
                if (--_selectedCardIdx < 0)
                {
                    _selectedCardIdx = PlayerCards.Count - 1;
                }
            }
            if (code == Keyboard.Key.Enter)
            {
                OnPlayerPickedCard(
                    new PlayerPickedCardEventArgs(
                        this, PlayerCards[_selectedCardIdx]
                    )
                );
            }
            if (code == Keyboard.Key.Num0)
            {
                OnPlayerPickedCard(
                    new PlayerPickedCardEventArgs(
                        this, null
                    )
                );
            }

            if (originalIndex != _selectedCardIdx)
            {
                if (PlayerCards[originalIndex].IsSelected)
                {
                    PlayerCards[originalIndex].IsSelected = false;
                }

                PlayerCards[_selectedCardIdx].IsSelected = true;
            }
        }


        public override void SwitchState(PlayerState newState)
        {
            base.SwitchState(newState);
        }
    }
}

