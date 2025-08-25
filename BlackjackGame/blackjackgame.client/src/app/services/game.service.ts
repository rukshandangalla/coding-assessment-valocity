import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { GameState, ActionRequest, StartGameRequest } from '../models/game.model';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private readonly baseUrl = 'http://localhost:5017/api/game';
  private gameState = new BehaviorSubject<GameState | null>(null);
  
  public gameState$ = this.gameState.asObservable();

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  startNewGame(): Observable<GameState> {
    const request: StartGameRequest = {};
    return this.http.post<GameState>(`${this.baseUrl}/start`, request, this.httpOptions)
      .pipe(
        tap(gameState => this.gameState.next(gameState))
      );
  }

  getGameState(gameId: string): Observable<GameState> {
    return this.http.get<GameState>(`${this.baseUrl}/${gameId}/state`)
      .pipe(
        tap(gameState => this.gameState.next(gameState))
      );
  }

  playerHit(gameId: string): Observable<GameState> {
    const request: ActionRequest = { gameId };
    return this.http.post<GameState>(`${this.baseUrl}/hit`, request, this.httpOptions)
      .pipe(
        tap(gameState => this.gameState.next(gameState))
      );
  }

  playerStand(gameId: string): Observable<GameState> {
    const request: ActionRequest = { gameId };
    return this.http.post<GameState>(`${this.baseUrl}/stand`, request, this.httpOptions)
      .pipe(
        tap(gameState => this.gameState.next(gameState))
      );
  }

  resetGame(gameId: string): Observable<GameState> {
    const request: ActionRequest = { gameId };
    return this.http.post<GameState>(`${this.baseUrl}/reset`, request, this.httpOptions)
      .pipe(
        tap(gameState => this.gameState.next(gameState))
      );
  }

  getCurrentGameState(): GameState | null {
    return this.gameState.value;
  }

  clearGameState(): void {
    this.gameState.next(null);
  }
}