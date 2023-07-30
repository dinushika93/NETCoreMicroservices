using AutoMapper;
using CommandService.Data;
using CommandService.Models;
using Grpc.Net.Client;

namespace CommandService.SynchrounusDataService.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"Calling GRPC service {_config["GrpcPlatformService"]} ...");
            var channel = GrpcChannel.ForAddress(_config["GrpcPlatformService"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();
            try
            {
                var reply = client.getAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.PlatformResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to GRPC service : {ex}...");
                return null;
            }
            
        }
    }
}
