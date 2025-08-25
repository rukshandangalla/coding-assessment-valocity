using BlackjackGame.Server.Models;
using BlackjackGame.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlackjackGame.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        /// <summary>
        /// Start a new blackjack game
        /// </summary>
        [HttpPost("start")]
        public async Task<ActionResult<GameStateDto>> StartGame([FromBody] StartGameRequestDto? request = null)
        {
            try
            {
                var game = await _gameService.StartNewGameAsync();
                var gameState = GameStateDto.FromDomain(game);
                
                _logger.LogInformation("New game started: {GameId}", game.GameId);
                
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting new game");
                return StatusCode(500, new { message = "Failed to start new game" });
            }
        }

        /// <summary>
        /// Get current game state
        /// </summary>
        [HttpGet("{gameId}/state")]
        public async Task<ActionResult<GameStateDto>> GetGameState(string gameId)
        {
            try
            {
                var game = await _gameService.GetGameAsync(gameId);
                if (game == null)
                {
                    return NotFound(new { message = $"Game {gameId} not found" });
                }

                var gameState = GameStateDto.FromDomain(game);
                return Ok(gameState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting game state for {GameId}", gameId);
                return StatusCode(500, new { message = "Failed to get game state" });
            }
        }

        /// <summary>
        /// Player hits (takes another card)
        /// </summary>
        [HttpPost("hit")]
        public async Task<ActionResult<GameStateDto>> Hit([FromBody] ActionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var game = await _gameService.PlayerHitAsync(request.GameId);
                var gameState = GameStateDto.FromDomain(game);
                
                _logger.LogInformation("Player hit in game {GameId}, new value: {HandValue}", 
                    request.GameId, game.PlayerHand.CalculateValue());
                
                return Ok(gameState);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid hit request for game {GameId}: {Message}", 
                    request.GameId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing hit for game {GameId}", request.GameId);
                return StatusCode(500, new { message = "Failed to process hit" });
            }
        }

        /// <summary>
        /// Player stands (ends their turn, dealer plays)
        /// </summary>
        [HttpPost("stand")]
        public async Task<ActionResult<GameStateDto>> Stand([FromBody] ActionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var game = await _gameService.PlayerStandAsync(request.GameId);
                var gameState = GameStateDto.FromDomain(game);
                
                _logger.LogInformation("Player stood in game {GameId}, result: {Result}", 
                    request.GameId, game.Result);
                
                return Ok(gameState);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid stand request for game {GameId}: {Message}", 
                    request.GameId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stand for game {GameId}", request.GameId);
                return StatusCode(500, new { message = "Failed to process stand" });
            }
        }

        /// <summary>
        /// Reset an existing game
        /// </summary>
        [HttpPost("reset")]
        public async Task<ActionResult<GameStateDto>> Reset([FromBody] ActionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var game = await _gameService.ResetGameAsync(request.GameId);
                var gameState = GameStateDto.FromDomain(game);
                
                _logger.LogInformation("Game {GameId} was reset", request.GameId);
                
                return Ok(gameState);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Invalid reset request for game {GameId}: {Message}", 
                    request.GameId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting game {GameId}", request.GameId);
                return StatusCode(500, new { message = "Failed to reset game" });
            }
        }

        /// <summary>
        /// Get all active games (for management/debugging)
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GameStateDto>>> GetAllGames()
        {
            try
            {
                var games = await _gameService.GetAllGamesAsync();
                var gameStates = games.Select(GameStateDto.FromDomain);
                
                return Ok(gameStates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all games");
                return StatusCode(500, new { message = "Failed to get games" });
            }
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}