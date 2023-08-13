using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _dbContext;

        public CommandRepo(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(Command));

            command.PlatformId = platformId;
            _dbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
                throw new ArgumentNullException(nameof(Platform));

            _dbContext.Platforms.Add(plat);
        }

        public Command GetCommandByPlatform(int platformId, int commandId)
        {
            return _dbContext.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();

        }

        public IEnumerable<Command> GetCommandsforPlatform(int platformId)
        {
            return _dbContext.Commands.Where(c => c.PlatformId == platformId)
                     .OrderBy(c=> c.Platform.Name)
                     .ToList();
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public bool PlatformExists(int platformId)
        {
            return _dbContext.Platforms.Any(p => p.Id == platformId);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _dbContext.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
