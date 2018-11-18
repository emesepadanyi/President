import { Score } from "./score.interface";
import { PlayerStatistics } from "./player.statistics.interface";

export interface EndStatistics {
    scoreCard: Score[];
    stats: PlayerStatistics;
}
