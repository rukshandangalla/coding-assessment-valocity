using FluentAssertions;
using BlackjackGame.Server.Domain;

namespace BlackjackGame.Tests;

public class DeckTests
{
    [Fact]
    public void Deck_ShouldInitializeWithFiftyTwoCards()
    {
        var deck = new Deck();

        deck.RemainingCards.Should().Be(52);
    }

    [Fact]
    public void DealCard_ShouldReturnCardAndReduceCount()
    {
        var deck = new Deck();
        var initialCount = deck.RemainingCards;

        var card = deck.DealCard();

        card.Should().NotBeNull();
        deck.RemainingCards.Should().Be(initialCount - 1);
    }

    [Fact]
    public void DealCard_WhenEmpty_ShouldReinitializeAndShuffle()
    {
        var deck = new Deck();
        
        // Deal all cards
        for (int i = 0; i < 52; i++)
        {
            deck.DealCard();
        }

        deck.RemainingCards.Should().Be(0);

        // Next deal should reinitialize
        var card = deck.DealCard();

        card.Should().NotBeNull();
        deck.RemainingCards.Should().Be(51);
    }

    [Fact]
    public void Deck_ShouldContainAllUniqueCards()
    {
        var deck = new Deck();
        var dealtCards = new List<Card>();

        // Deal all cards
        for (int i = 0; i < 52; i++)
        {
            dealtCards.Add(deck.DealCard());
        }

        // Should have all 52 unique combinations
        dealtCards.Should().HaveCount(52);
        
        // Check we have all suits and values
        var suits = dealtCards.Select(c => c.Suit).Distinct();
        var values = dealtCards.Select(c => c.Value).Distinct();
        
        suits.Should().HaveCount(4);
        values.Should().HaveCount(13);
    }
}