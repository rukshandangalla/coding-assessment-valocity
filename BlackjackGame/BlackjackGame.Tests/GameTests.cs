using FluentAssertions;
using BlackjackGame.Server.Domain;

namespace BlackjackGame.Tests;

public class GameTests
{
    [Fact]
    public void Game_ShouldInitializeWithCorrectState()
    {
        var game = new Game();

        game.GameId.Should().NotBeNullOrEmpty();
        game.PlayerHand.Should().NotBeNull();
        game.DealerHand.Should().NotBeNull();
        game.Phase.Should().Be(GamePhase.NotStarted);
        game.Result.Should().Be(GameResult.None);
    }

    [Fact]
    public void StartNewGame_ShouldDealInitialCards()
    {
        var game = new Game();

        game.StartNewGame();

        game.PlayerHand.Cards.Should().HaveCount(2);
        game.DealerHand.Cards.Count.Should().BeGreaterThanOrEqualTo(2);
        
        // Phase should change from NotStarted
        game.Phase.Should().NotBe(GamePhase.NotStarted);
        
        // If game is still in player turn, dealer's second card should be hidden
        if (game.Phase == GamePhase.PlayerTurn)
        {
            var dealerCards = game.DealerHand.Cards.ToList();
            dealerCards.Should().HaveCount(2);
            dealerCards.Last().IsHidden.Should().BeTrue();
        }
    }

    [Fact]
    public void PlayerHit_ShouldAddCardToPlayerHand()
    {
        var game = new Game();
        game.StartNewGame();
        var initialCardCount = game.PlayerHand.Cards.Count;

        game.PlayerHit();

        game.PlayerHand.Cards.Should().HaveCount(initialCardCount + 1);
    }

    [Fact]
    public void PlayerHit_WhenPlayerBusts_ShouldEndGame()
    {
        var game = new Game();
        game.StartNewGame();
        
        // Force bust by adding high cards
        while (!game.PlayerHand.IsBusted && game.Phase == GamePhase.PlayerTurn)
        {
            game.PlayerHit();
        }

        if (game.PlayerHand.IsBusted)
        {
            game.Phase.Should().Be(GamePhase.GameOver);
            game.Result.Should().Be(GameResult.DealerWins);
        }
    }

    [Fact]
    public void PlayerStand_ShouldTriggerDealerPlay()
    {
        var game = new Game();
        game.StartNewGame();

        game.PlayerStand();

        game.Phase.Should().Be(GamePhase.GameOver);
        game.Result.Should().NotBe(GameResult.None);
        
        // Dealer cards should be revealed
        game.DealerHand.Cards.All(c => !c.IsHidden).Should().BeTrue();
    }
}
