namespace BlackjackGame.Server.Domain
{
    public enum GamePhase
    {
        NotStarted,
        PlayerTurn,
        DealerTurn,
        GameOver
    }

    public enum GameResult
    {
        None,
        PlayerWins,
        DealerWins,
        Push // Tie
    }

    public class Game
    {
        private Deck _deck;
        
        public string GameId { get; }
        public Hand PlayerHand { get; }
        public Hand DealerHand { get; }
        public GamePhase Phase { get; private set; }
        public GameResult Result { get; private set; }
        public DateTime CreatedAt { get; }
        
        public Game(string? gameId = null)
        {
            GameId = gameId ?? Guid.NewGuid().ToString();
            _deck = new Deck();
            PlayerHand = new Hand();
            DealerHand = new Hand();
            Phase = GamePhase.NotStarted;
            Result = GameResult.None;
            CreatedAt = DateTime.UtcNow;
        }

        public void StartNewGame()
        {
            // Clear hands
            PlayerHand.Clear();
            DealerHand.Clear();
            
            // Reset game state
            Phase = GamePhase.PlayerTurn;
            Result = GameResult.None;
            
            // Deal initial cards
            PlayerHand.AddCard(_deck.DealCard());
            DealerHand.AddCard(_deck.DealCard());
            PlayerHand.AddCard(_deck.DealCard());
            
            // Dealer's second card is hidden
            var dealerSecondCard = _deck.DealCard();
            dealerSecondCard.IsHidden = true;
            DealerHand.AddCard(dealerSecondCard);
            
            // Check for blackjack
            if (PlayerHand.IsBlackjack)
            {
                Phase = GamePhase.DealerTurn;
                PlayDealerTurn();
            }
        }

        public void PlayerHit()
        {
            if (Phase != GamePhase.PlayerTurn)
                throw new InvalidOperationException("It's not the player's turn");

            PlayerHand.AddCard(_deck.DealCard());
            
            if (PlayerHand.IsBusted)
            {
                Phase = GamePhase.GameOver;
                Result = GameResult.DealerWins;
            }
        }

        public void PlayerStand()
        {
            if (Phase != GamePhase.PlayerTurn)
                throw new InvalidOperationException("It's not the player's turn");

            Phase = GamePhase.DealerTurn;
            PlayDealerTurn();
        }

        private void PlayDealerTurn()
        {
            // Reveal dealer's hidden card
            DealerHand.RevealHiddenCards();
            
            // Dealer must hit on 16 and below, stand on 17 and above
            while (DealerHand.CalculateValue() < 17)
            {
                DealerHand.AddCard(_deck.DealCard());
            }
            
            Phase = GamePhase.GameOver;
            DetermineWinner();
        }

        private void DetermineWinner()
        {
            var playerValue = PlayerHand.CalculateValue();
            var dealerValue = DealerHand.CalculateValue();
            
            if (PlayerHand.IsBusted)
            {
                Result = GameResult.DealerWins;
            }
            else if (DealerHand.IsBusted)
            {
                Result = GameResult.PlayerWins;
            }
            else if (PlayerHand.IsBlackjack && !DealerHand.IsBlackjack)
            {
                Result = GameResult.PlayerWins;
            }
            else if (!PlayerHand.IsBlackjack && DealerHand.IsBlackjack)
            {
                Result = GameResult.DealerWins;
            }
            else if (playerValue > dealerValue)
            {
                Result = GameResult.PlayerWins;
            }
            else if (dealerValue > playerValue)
            {
                Result = GameResult.DealerWins;
            }
            else
            {
                Result = GameResult.Push;
            }
        }

        public bool CanPlayerHit => Phase == GamePhase.PlayerTurn && !PlayerHand.IsBusted;
        public bool CanPlayerStand => Phase == GamePhase.PlayerTurn;
        public bool IsGameOver => Phase == GamePhase.GameOver;
    }
}