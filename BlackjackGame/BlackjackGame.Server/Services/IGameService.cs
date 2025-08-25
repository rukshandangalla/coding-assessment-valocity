using BlackjackGame.Server.Domain;

namespace BlackjackGame.Server.Services
{
    public interface IGameService
    {
        /// <summary>
        /// Starts a new blackjack game
        /// </summary>
        Task<Game> StartNewGameAsync();
        
        /// <summary>
        /// Gets the current game state
        /// </summary>
        Task<Game?> GetGameAsync(string gameId);
        
        /// <summary>
        /// Player hits (takes another card)
        /// </summary>
        Task<Game> PlayerHitAsync(string gameId);
        
        /// <summary>
        /// Player stands (ends their turn)
        /// </summary>
        Task<Game> PlayerStandAsync(string gameId);
        
        /// <summary>
        /// Resets an existing game
        /// </summary>
        Task<Game> ResetGameAsync(string gameId);
        
        /// <summary>
        /// Gets all active games (for management)
        /// </summary>
        Task<IEnumerable<Game>> GetAllGamesAsync();
    }
}