namespace CommandService.EventProcessing
{
    public interface IEventProcesser
    {
        void ProcessEvent(string message);
    }
}
