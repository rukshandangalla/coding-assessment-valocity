using BlackjackGame.Server.Domain;

namespace BlackjackGame.Server.Models
{
    public class HandDto
    {
        public List<CardDto> Cards { get; set; } = new();
        public int Value { get; set; }
        public bool IsBusted { get; set; }
        public bool IsBlackjack { get; set; }
        public bool IsSoft { get; set; }

        public static HandDto FromDomain(Hand hand)
        {
            return new HandDto
            {
                Cards = hand.Cards.Select(CardDto.FromDomain).ToList(),
                Value = hand.CalculateValue(),
                IsBusted = hand.IsBusted,
                IsBlackjack = hand.IsBlackjack,
                IsSoft = hand.IsSoft
            };
        }
    }
}