using System.Threading.Tasks;

namespace President.API.Hubs
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage( string payload );
    }
}
