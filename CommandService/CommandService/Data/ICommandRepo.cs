using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        public bool SaveChanges();

        //Platforms
        public IEnumerable<Platform> GetPlatforms();
        public void CreatePlatform(Platform plat);
        public bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        //Commands
        public IEnumerable<Command> GetCommandsforPlatform(int platformId);

        public Command GetCommandByPlatform(int platformId, int CommandId);

        public void CreateCommand(int platformId, Command Command);        
    }
}
