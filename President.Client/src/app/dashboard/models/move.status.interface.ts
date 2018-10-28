import { Card } from "./card.interface";
import { Hand } from "./hand.interface";

export interface MoveStatus{
    movedCard: Card;
    cards: Card[];
    ownRank: string;
    hands: Hand[];
    nextUser: string;
}