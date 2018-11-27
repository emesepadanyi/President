import { Card } from "./card.interface";
import { Score } from "./score.interface";

export interface NewRound {
    wait: boolean;
    cards: Card[];
    ownRank: string;
    switchedCards: Card[];
    scoreCard: Score[];
}
