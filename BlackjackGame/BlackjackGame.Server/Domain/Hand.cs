namespace BlackjackGame.Server.Domain
{
    public class Hand
    {
        private List<Card> _cards;

        public IReadOnlyList<Card> Cards => _cards.AsReadOnly();
        
        public Hand()
        {
            _cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public void Clear()
        {
            _cards.Clear();
        }

        public int CalculateValue()
        {
            int value = 0;
            int aceCount = 0;

            foreach (var card in _cards)
            {
                if (card.Value == CardValue.Ace)
                {
                    aceCount++;
                    value += 11; // Initially count Ace as 11
                }
                else if (card.Value >= CardValue.Jack)
                {
                    value += 10; // Face cards worth 10
                }
                else
                {
                    value += (int)card.Value;
                }
            }

            // Adjust for Aces if bust
            while (value > 21 && aceCount > 0)
            {
                value -= 10; // Convert Ace from 11 to 1
                aceCount--;
            }

            return value;
        }

        public bool IsBusted => CalculateValue() > 21;

        public bool IsBlackjack => _cards.Count == 2 && CalculateValue() == 21;

        public bool IsSoft => _cards.Any(c => c.Value == CardValue.Ace) && CalculateValue() <= 21;

        public void RevealHiddenCards()
        {
            foreach (var card in _cards)
            {
                card.IsHidden = false;
            }
        }
    }
}