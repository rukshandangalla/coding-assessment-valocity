using BlackjackGame.Server.Domain;
using System.Collections.Concurrent;

namespace BlackjackGame.Server.Services
{
    public class GameService : IGameService
    {
        private readonly ConcurrentDictionary<string, Game> _games = new();
        private readonly ILogger<GameService> _logger;

        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }

        public Task<Game> StartNewGameAsync()
        {
            var game = new Game();
            game.StartNewGame();
            
            _games.TryAdd(game.GameId, game);
            
            _logger.LogInformation("Started new game with ID: {GameId}", game.GameId);
            
            return Task.FromResult(game);
        }

        public Task<Game?> GetGameAsync(string gameId)
        {
            _games.TryGetValue(gameId, out var game);
            return Task.FromResult(game);
        }

        public async Task<Game> PlayerHitAsync(string gameId)
        {
            var game = await GetGameAsync(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found");
            }

            try
            {
                game.PlayerHit();
                _logger.LogInformation("Player hit in game {GameId}. Hand value: {HandValue}", 
                    gameId, game.PlayerHand.CalculateValue());
                
                return game;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid player hit attempt in game {GameId}: {Message}", 
                    gameId, ex.Message);
                throw;
            }
        }

        public async Task<Game> PlayerStandAsync(string gameId)
        {
            var game = await GetGameAsync(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found");
            }

            try
            {
                game.PlayerStand();
                _logger.LogInformation("Player stood in game {GameId}. Final result: {Result}", 
                    gameId, game.Result);
                
                return game;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid player stand attempt in game {GameId}: {Message}", 
                    gameId, ex.Message);
                throw;
            }
        }

        public async Task<Game> ResetGameAsync(string gameId)
        {
            var game = await GetGameAsync(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found");
            }

            game.StartNewGame();
            _logger.LogInformation("Reset game {GameId}", gameId);
            
            return game;
        }

        public Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            var games = _games.Values.AsEnumerable();
            return Task.FromResult(games);
        }
    }
}