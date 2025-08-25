using BlackjackGame.Server.Domain;

namespace BlackjackGame.Server.Models
{
    public class CardDto
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }
        public bool IsHidden { get; set; }
        public string Display { get; set; } = string.Empty;

        public static CardDto FromDomain(Card card)
        {
            return new CardDto
            {
                Suit = card.Suit,
                Value = card.Value,
                IsHidden = card.IsHidden,
                Display = card.ToString()
            };
        }
    }
}