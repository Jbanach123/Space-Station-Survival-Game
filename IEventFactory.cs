namespace SpaceStationSurvival

// Defines a factory interface for creating station events based on the current turn.
{
    public interface IEventFactory
    {
        IStationEvent CreateEvent(int turn);
    }
}