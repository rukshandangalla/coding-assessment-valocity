namespace BlackjackGame.Server.Domain
{
    public enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum CardValue
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    public class Card
    {
        public CardSuit Suit { get; }
        public CardValue Value { get; }
        public bool IsHidden { get; set; }

        public Card(CardSuit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
            IsHidden = false;
        }

        public int GetBlackjackValue()
        {
            // Face cards are worth 10
            if (Value >= CardValue.Jack)
                return 10;
            
            // Ace can be 1 or 11 (handled at hand level)
            if (Value == CardValue.Ace)
                return 11;
            
            // Number cards are face value
            return (int)Value;
        }

        public override string ToString()
        {
            return IsHidden ? "Hidden" : $"{Value} of {Suit}";
        }
    }
}