import { Card } from "./card.interface";
import { Hand } from "./hand.interface";

export interface GameStatus{
    cards: Card[];
    ownRank: string;
    hands: Hand[];
    nextUser: string;
    round: number;
}
