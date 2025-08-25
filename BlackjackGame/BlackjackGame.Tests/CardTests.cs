using FluentAssertions;
using BlackjackGame.Server.Domain;

namespace BlackjackGame.Tests;

public class CardTests
{
    [Fact]
    public void Card_ShouldInitializeWithCorrectSuitAndValue()
    {
        var card = new Card(CardSuit.Hearts, CardValue.Ace);

        card.Suit.Should().Be(CardSuit.Hearts);
        card.Value.Should().Be(CardValue.Ace);
        card.IsHidden.Should().BeFalse();
    }

    [Theory]
    [InlineData(CardValue.Ace, 11)]
    [InlineData(CardValue.Two, 2)]
    [InlineData(CardValue.Ten, 10)]
    [InlineData(CardValue.Jack, 10)]
    [InlineData(CardValue.Queen, 10)]
    [InlineData(CardValue.King, 10)]
    public void GetBlackjackValue_ShouldReturnCorrectValue(CardValue cardValue, int expectedValue)
    {
        var card = new Card(CardSuit.Hearts, cardValue);

        card.GetBlackjackValue().Should().Be(expectedValue);
    }

    [Fact]
    public void ToString_WhenNotHidden_ShouldReturnCardDescription()
    {
        var card = new Card(CardSuit.Spades, CardValue.King);

        card.ToString().Should().Be("King of Spades");
    }

    [Fact]
    public void ToString_WhenHidden_ShouldReturnHidden()
    {
        var card = new Card(CardSuit.Spades, CardValue.King);
        card.IsHidden = true;

        card.ToString().Should().Be("Hidden");
    }
}