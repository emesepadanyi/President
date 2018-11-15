using President.API.ViewModels;
using System.Threading.Tasks;

namespace President.API.Hubs
{
    public interface IGameHub
    {
        Task StartGame(GameViewModel gameViewModel);
        Task PutCard(MoveViewModel moveViewModel);
        Task ResetDeck(string NextPlayer);
        Task WaitForNewRound(NewRoundViewModel newRoundViewModel);
        Task GameEnded(EndStatisticsViewModel endGameStatisticsViewModel);
    }
}
