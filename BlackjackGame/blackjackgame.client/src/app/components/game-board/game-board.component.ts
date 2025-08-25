import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { GameService } from '../../services/game.service';
import { GameState, GameResult, GamePhase } from '../../models/game.model';

@Component({
  selector: 'app-game-board',
  standalone: false,
  templateUrl: './game-board.component.html',
  styleUrl: './game-board.component.css'
})
export class GameBoardComponent implements OnInit, OnDestroy {
  gameState: GameState | null = null;
  private subscription: Subscription = new Subscription();
  isLoading = false;
  errorMessage = '';

  constructor(private gameService: GameService) { }

  ngOnInit(): void {
    this.subscription.add(
      this.gameService.gameState$.subscribe(
        state => {
          this.gameState = state;
          this.isLoading = false;
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  startNewGame(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.gameService.startNewGame().subscribe({
      next: () => {
        console.log('New game started');
      },
      error: (error) => {
        this.errorMessage = 'Failed to start new game';
        this.isLoading = false;
        console.error('Error starting game:', error);
      }
    });
  }

  playerHit(): void {
    if (!this.gameState) return;
    
    this.isLoading = true;
    this.gameService.playerHit(this.gameState.gameId).subscribe({
      next: () => {
        console.log('Player hit');
      },
      error: (error) => {
        this.errorMessage = 'Failed to hit';
        this.isLoading = false;
        console.error('Error hitting:', error);
      }
    });
  }

  playerStand(): void {
    if (!this.gameState) return;
    
    this.isLoading = true;
    this.gameService.playerStand(this.gameState.gameId).subscribe({
      next: () => {
        console.log('Player stood');
      },
      error: (error) => {
        this.errorMessage = 'Failed to stand';
        this.isLoading = false;
        console.error('Error standing:', error);
      }
    });
  }

  resetGame(): void {
    if (!this.gameState) return;
    
    this.isLoading = true;
    this.errorMessage = '';
    this.gameService.resetGame(this.gameState.gameId).subscribe({
      next: () => {
        console.log('Game reset');
      },
      error: (error) => {
        this.errorMessage = 'Failed to reset game';
        this.isLoading = false;
        console.error('Error resetting game:', error);
      }
    });
  }

  getResultClass(): string {
    if (!this.gameState || !this.gameState.isGameOver) return '';
    
    switch (this.gameState.result) {
      case GameResult.PlayerWins:
        return 'result-win';
      case GameResult.DealerWins:
        return 'result-lose';
      case GameResult.Push:
        return 'result-push';
      default:
        return '';
    }
  }
}
