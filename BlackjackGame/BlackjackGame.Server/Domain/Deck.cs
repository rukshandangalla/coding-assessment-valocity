namespace BlackjackGame.Server.Domain
{
    public class Deck
    {
        private List<Card> _cards = new();
        private readonly Random _random;

        public Deck()
        {
            _random = new Random();
            Initialize();
            Shuffle();
        }

        private void Initialize()
        {
            _cards = new List<Card>();
            
            foreach (CardSuit suit in Enum.GetValues<CardSuit>())
            {
                foreach (CardValue value in Enum.GetValues<CardValue>())
                {
                    _cards.Add(new Card(suit, value));
                }
            }
        }

        public void Shuffle()
        {
            // Fisher-Yates shuffle algorithm
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            }
        }

        public Card DealCard()
        {
            if (_cards.Count == 0)
            {
                // Reinitialize and shuffle if we run out of cards
                Initialize();
                Shuffle();
            }

            var card = _cards[^1];
            _cards.RemoveAt(_cards.Count - 1);
            return card;
        }

        public int RemainingCards => _cards.Count;
    }
}