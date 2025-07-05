namespace SpaceStationSurvival
{
    // Represents a solar storm event that affects the space station.
    public class SolarStormEvent : IStationEvent
    {
        // The severity level of the solar storm.
        private readonly int _severity;
        
        // Constructor to initialize the solar storm with a given severity.
        public SolarStormEvent(int severity)
        {
            _severity = severity;
        }
        
        // Description of the event, including its severity.
        public string Description => $"Solar storm (severity: {_severity}) reduces energy production!";
        
        // Applies the effects of the solar storm to the station by consuming energy resources.
        public void Apply(Station station)
        {
            station.Resources.Consume("Energy", _severity);
        }
    }
}    