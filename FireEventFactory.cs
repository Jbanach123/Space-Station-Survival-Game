namespace SpaceStationSurvival
{
    // Factory for creating fire events with progressive difficulty
    public class FireEventFactory : IEventFactory
    {
        // Creates a fire event with intensity based on current turn
        public IStationEvent CreateEvent(int turn)
        {
            // The later the turn, the more modules can be damaged
            int spreadFactor = 1 + (turn / 10); // +1 every 7 turns
            return new FireEvent(spreadFactor);
        }
    }
}