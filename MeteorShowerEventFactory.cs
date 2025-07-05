namespace SpaceStationSurvival
{
    public class MeteorShowerEventFactory : IEventFactory
    {
        public IStationEvent CreateEvent(int turn)
        {
            // The later the turn, the more meteors
            int intensity = 1 + (turn / 10); // +1 every 10 turns
            return new MeteorShowerEvent(intensity);
        }
    }
}