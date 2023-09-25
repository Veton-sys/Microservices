namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message); //get this from event listener        
    }
}