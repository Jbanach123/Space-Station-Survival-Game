namespace SpaceStationSurvival
{
    // Factory class for creating SolarStormEvent instances based on the current turn
    public class SolarStormEventFactory : IEventFactory
    {
        // Creates a SolarStormEvent with severity increasing every 5 turns
        public IStationEvent CreateEvent(int turn)
        {
            int severity = 20 + (turn / 5);
            return new SolarStormEvent(severity);
        }
    }
}