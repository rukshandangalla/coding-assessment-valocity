using System.ComponentModel.DataAnnotations;

namespace BlackjackGame.Server.Models
{
    public class ActionRequestDto
    {
        [Required]
        public string GameId { get; set; } = string.Empty;
    }

    public class StartGameRequestDto
    {
        // Empty for now, could include player name, betting amount, etc.
    }
}