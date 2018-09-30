import { Card } from "./card.interface";

export interface GameStatus{
    cards: Card[];
    hands: Map<string, number>;
}