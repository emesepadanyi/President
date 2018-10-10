import { Card } from "./card.interface";
import { Hand } from "./hand.interface";

export interface GameStatus{
    cards: Card[];
    hands: Hand[];
    nextUser: string;
}