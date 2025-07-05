namespace SpaceStationSurvival
{
    public class HullBreachEventFactory : IEventFactory
    {
        public IStationEvent CreateEvent(int turn)
        {
            // Calculate damage severity: increases every 12 turns
            int damageSeverity = 1 + (turn / 12);
            
            // Calculate oxygen loss: increases every 4 turns
            int oxygenLoss = 10 + (turn / 4);
            
            // Create new hull breach event with calculated values
            return new HullBreachEvent(damageSeverity, oxygenLoss);
        }
    }
}