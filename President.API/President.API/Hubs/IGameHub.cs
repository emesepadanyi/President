using President.API.ViewModels;
using System.Threading.Tasks;

namespace President.API.Hubs
{
    public interface IGameHub
    {
        Task StartGame(GameViewModel gameViewModel);
    }
}
