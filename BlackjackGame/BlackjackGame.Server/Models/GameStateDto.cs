using BlackjackGame.Server.Domain;

namespace BlackjackGame.Server.Models
{
    public class GameStateDto
    {
        public string GameId { get; set; } = string.Empty;
        public HandDto PlayerHand { get; set; } = new();
        public HandDto DealerHand { get; set; } = new();
        public GamePhase Phase { get; set; }
        public GameResult Result { get; set; }
        public bool CanPlayerHit { get; set; }
        public bool CanPlayerStand { get; set; }
        public bool IsGameOver { get; set; }
        public string ResultMessage { get; set; } = string.Empty;

        public static GameStateDto FromDomain(Game game)
        {
            var dto = new GameStateDto
            {
                GameId = game.GameId,
                PlayerHand = HandDto.FromDomain(game.PlayerHand),
                DealerHand = HandDto.FromDomain(game.DealerHand),
                Phase = game.Phase,
                Result = game.Result,
                CanPlayerHit = game.CanPlayerHit,
                CanPlayerStand = game.CanPlayerStand,
                IsGameOver = game.IsGameOver
            };

            // Add friendly result message
            dto.ResultMessage = game.Result switch
            {
                GameResult.PlayerWins => "You win!",
                GameResult.DealerWins => "Dealer wins!",
                GameResult.Push => "It's a push!",
                _ => string.Empty
            };

            return dto;
        }
    }
}