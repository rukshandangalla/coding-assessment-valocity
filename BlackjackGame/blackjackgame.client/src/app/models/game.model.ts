export enum CardSuit {
  Hearts = 0,
  Diamonds = 1,
  Clubs = 2,
  Spades = 3
}

export enum CardValue {
  Ace = 1,
  Two = 2,
  Three = 3,
  Four = 4,
  Five = 5,
  Six = 6,
  Seven = 7,
  Eight = 8,
  Nine = 9,
  Ten = 10,
  Jack = 11,
  Queen = 12,
  King = 13
}

export enum GamePhase {
  NotStarted = 0,
  PlayerTurn = 1,
  DealerTurn = 2,
  GameOver = 3
}

export enum GameResult {
  None = 0,
  PlayerWins = 1,
  DealerWins = 2,
  Push = 3
}

export interface Card {
  suit: CardSuit;
  value: CardValue;
  isHidden: boolean;
  display: string;
}

export interface Hand {
  cards: Card[];
  value: number;
  isBusted: boolean;
  isBlackjack: boolean;
  isSoft: boolean;
}

export interface GameState {
  gameId: string;
  playerHand: Hand;
  dealerHand: Hand;
  phase: GamePhase;
  result: GameResult;
  canPlayerHit: boolean;
  canPlayerStand: boolean;
  isGameOver: boolean;
  resultMessage: string;
}

export interface ActionRequest {
  gameId: string;
}

export interface StartGameRequest {
  // Empty for now
}