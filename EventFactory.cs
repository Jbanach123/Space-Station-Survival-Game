namespace SpaceStationSurvival
{
    // Creates random station events using different event factories.
    public class EventFactory : IEventFactory
    {
        private readonly List<IEventFactory> _eventFactories;
        private readonly Random _random;
        
        // Initializes with all available event factories
        public EventFactory()
        {
            _random = new Random();
            _eventFactories = new List<IEventFactory>
            {
                new SolarStormEventFactory(),
                new MeteorShowerEventFactory(),
                new RadiationLeakEventFactory(),
                new HullBreachEventFactory(),
                new FireEventFactory()
            };
        }
        
        // Creates a random event based on current turn number
        public IStationEvent CreateEvent(int turn)
        {
            // For first 10 turns, use only basic events (first 3 factories)
            // After turn 10, use all available event types
            int index = turn < 10 
                ? _random.Next(0, 3)   // Basic events only
                : _random.Next(0, _eventFactories.Count);  // All events
            
            return _eventFactories[index].CreateEvent(turn);
        }
    }
}
