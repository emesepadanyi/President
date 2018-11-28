
using System.Threading.Tasks;

namespace President.API.Hubs
{
    public interface IOnlineHub
    {
        Task Invite();
    }
}
