import { Card } from "./card.interface";

export interface NewRound {
    wait: boolean;
    cards: Card[];
    ownRank: string;
    switchedCards: Card[];
}