using FluentAssertions;
using BlackjackGame.Server.Domain;

namespace BlackjackGame.Tests;

public class HandTests
{
    [Fact]
    public void Hand_ShouldInitializeEmpty()
    {
        var hand = new Hand();

        hand.Cards.Should().BeEmpty();
        hand.CalculateValue().Should().Be(0);
        hand.IsBusted.Should().BeFalse();
    }

    [Fact]
    public void AddCard_ShouldAddCardToHand()
    {
        var hand = new Hand();
        var card = new Card(CardSuit.Hearts, CardValue.Ace);

        hand.AddCard(card);

        hand.Cards.Should().HaveCount(1);
        hand.Cards.First().Should().Be(card);
    }

    [Fact]
    public void CalculateValue_WithAceAndTen_ShouldReturnTwentyOne()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardSuit.Hearts, CardValue.Ace));
        hand.AddCard(new Card(CardSuit.Spades, CardValue.Ten));

        hand.CalculateValue().Should().Be(21);
        hand.IsBlackjack.Should().BeTrue();
    }

    [Fact]
    public void CalculateValue_WithMultipleAces_ShouldAdjustAcesCorrectly()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardSuit.Hearts, CardValue.Ace));
        hand.AddCard(new Card(CardSuit.Spades, CardValue.Ace));
        hand.AddCard(new Card(CardSuit.Clubs, CardValue.Nine));

        hand.CalculateValue().Should().Be(21);
        hand.IsBusted.Should().BeFalse();
    }

    [Fact]
    public void IsBusted_WithValueOverTwentyOne_ShouldReturnTrue()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardSuit.Hearts, CardValue.Ten));
        hand.AddCard(new Card(CardSuit.Spades, CardValue.Ten));
        hand.AddCard(new Card(CardSuit.Clubs, CardValue.Five));

        hand.CalculateValue().Should().Be(25);
        hand.IsBusted.Should().BeTrue();
    }

    [Fact]
    public void Clear_ShouldRemoveAllCards()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardSuit.Hearts, CardValue.Ten));
        hand.AddCard(new Card(CardSuit.Spades, CardValue.Five));

        hand.Clear();

        hand.Cards.Should().BeEmpty();
        hand.CalculateValue().Should().Be(0);
    }

    [Fact]
    public void IsSoft_WithAceCountingAsEleven_ShouldReturnTrue()
    {
        var hand = new Hand();
        hand.AddCard(new Card(CardSuit.Hearts, CardValue.Ace));
        hand.AddCard(new Card(CardSuit.Spades, CardValue.Six));

        hand.IsSoft.Should().BeTrue();
        hand.CalculateValue().Should().Be(17);
    }

    [Fact]
    public void RevealHiddenCards_ShouldMakeAllCardsVisible()
    {
        var hand = new Hand();
        var hiddenCard = new Card(CardSuit.Hearts, CardValue.King) { IsHidden = true };
        hand.AddCard(hiddenCard);

        hand.RevealHiddenCards();

        hand.Cards.All(c => !c.IsHidden).Should().BeTrue();
    }
}