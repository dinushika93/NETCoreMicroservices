using CommandService.Models;

namespace CommandService.SynchrounusDataService.Grpc
{
    public interface IPlatformDataClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}
