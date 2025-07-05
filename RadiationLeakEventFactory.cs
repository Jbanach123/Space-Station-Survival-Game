namespace SpaceStationSurvival
{
    public class RadiationLeakEventFactory : IEventFactory
    {
        public IStationEvent CreateEvent(int turn)
        {
            // The later the turn, the greater the leak
            int leakAmount = 15 + (turn / 3); // +8 every 3 turns
            return new RadiationLeakEvent(leakAmount);
        }
    }
}