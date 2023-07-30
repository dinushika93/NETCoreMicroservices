using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace CommandService.Controllers
{
    [Route("api/c/platform/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsforPlatform(int platformId)
        {
            Console.WriteLine($"GetCommandsforPlatform : {platformId}");
            if (!_repository.PlatformExists(platformId))
                return NotFound();

            var commands = _repository.GetCommandsforPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"GetCommandByPlatform : {platformId} - {commandId}");
            if(!_repository.PlatformExists(platformId))
                return NotFound();

           var command = _repository.GetCommandByPlatform(platformId, commandId);
           if(command == null)
                return NotFound();
            return Ok(_mapper.Map<CommandReadDto>(command));

        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto CommandDto)
        {
            Console.WriteLine($"CreateCommand : {platformId}");
            if (!_repository.PlatformExists(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(CommandDto);
            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
            

        }
    }
}
