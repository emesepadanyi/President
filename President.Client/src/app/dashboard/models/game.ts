import { Card } from "./card.interface";
import { Hand } from "./hand.interface";
import { GameStatus } from "./game.status.interface";
import { MoveStatus } from "./move.status.interface";

export class Game {
    public cards: Card[];
    public ownRank: string;
    public hands: Hand[];
    public nextUser: string;
    public round: number;
    public deck: Card[] = new Array<Card>();

    setUp(gameStatus: GameStatus) {
        this.deck = new Array<Card>();
        this.cards = gameStatus.cards;
        this.ownRank = gameStatus.ownRank;
        this.hands = gameStatus.hands;
        this.nextUser = gameStatus.nextUser;
        this.round = gameStatus.round;
    }

    moveCard(moveStatus: MoveStatus) {
        this.cards = moveStatus.cards;
        this.ownRank = moveStatus.ownRank;
        this.hands = moveStatus.hands;
        this.nextUser = moveStatus.nextUser;
        if (!!moveStatus.movedCard) { this.deck.push(moveStatus.movedCard); }
    }

    resetDeck() {
        this.deck = new Array<Card>();
    }
}