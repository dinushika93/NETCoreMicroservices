using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult TestInboundConnectionService()
        {
            Console.WriteLine("Inbound post command service");

            return Ok("Inbound post command service controller");
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting platforms...");
            var platforms = _repository.GetPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
    }
}
