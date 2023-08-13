using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private IPlatformRepo _repo;
        private IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<platformResponse> getAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var reponse = new platformResponse();
            var platforms = _repo.GetAllPlatforms();
            foreach( var platform in platforms )
            {
                reponse.PlatformResponse.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }
            return Task.FromResult(reponse);
        }
    }
}
