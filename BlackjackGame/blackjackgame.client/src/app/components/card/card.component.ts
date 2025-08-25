import { Component, Input } from '@angular/core';
import { Card, CardSuit } from '../../models/game.model';

@Component({
  selector: 'app-card',
  standalone: false,
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  @Input() card!: Card;

  getCardSymbol(): string {
    if (this.card.isHidden) {
      return '🂠';
    }

    const suits = {
      [CardSuit.Hearts]: '♥',
      [CardSuit.Diamonds]: '♦',
      [CardSuit.Clubs]: '♣',
      [CardSuit.Spades]: '♠'
    };

    return suits[this.card.suit];
  }

  getCardValue(): string {
    if (this.card.isHidden) {
      return '?';
    }

    switch (this.card.value) {
      case 1: return 'A';
      case 11: return 'J';
      case 12: return 'Q';
      case 13: return 'K';
      default: return this.card.value.toString();
    }
  }

  getCardColor(): string {
    if (this.card.isHidden) {
      return 'blue';
    }
    return (this.card.suit === CardSuit.Hearts || this.card.suit === CardSuit.Diamonds) ? 'red' : 'black';
  }
}
