namespace SpaceStationSurvival
{
    // Interface representing an event that can occur on the space station
    public interface IStationEvent
    {
        void Apply(Station station);
        string Description { get; }
    }
}
