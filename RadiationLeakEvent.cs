namespace SpaceStationSurvival{

    // Represents a radiation leak event that affects the space station by consuming a specified amount of oxygen.
    public class RadiationLeakEvent : IStationEvent
    {
        private readonly int _leakAmount;
        // Initializes a new instance of the RadiationLeakEvent class with the specified leak amount.
        public RadiationLeakEvent(int leakAmount)
        {
            _leakAmount = leakAmount;
        }

        public string Description => $"Radiation leak detected! Losing {_leakAmount} oxygen!";

        // Applies the effects of the radiation leak event to the specified station, consuming oxygen resources.
        public void Apply(Station station)
        {
            station.Resources.Consume("Oxygen", _leakAmount);
        }
    }
}