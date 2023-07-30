using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    
    public class EventProcesser : IEventProcesser
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcesser(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            EventType eventType = DetermineEventType(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
            }
        }

        private void AddPlatform(string message)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<ICommandRepo>();
                try
                {
                    var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                    var plaform = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(plaform.ExternalId))
                    {
                        repo.CreatePlatform(plaform);
                        repo.SaveChanges();
                        Console.WriteLine($"Platform added...");
                    }
                    else
                        Console.WriteLine($"Platform already exists...");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error adding platform : {ex}");
                }
            }
        }

        private EventType DetermineEventType(string notificationMessage)
        {
            Console.WriteLine("Determining event type...");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "Platform_Published" :
                    Console.WriteLine($"Platform published event is detected...");
                    return EventType.PlatformPublished;

                default:
                    Console.WriteLine($"Event is undetermined...");
                    return EventType.Undefined;

            }
        }

    }



    enum EventType
    {
        PlatformPublished,
        Undefined

    } 
}
