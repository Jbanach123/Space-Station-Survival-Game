namespace SpaceStationSurvival
{
    // Represents a hull breach event in the space station
    public class HullBreachEvent : IStationEvent
    {
        private readonly int _damageSeverity;
        private readonly int _oxygenLoss;

        public HullBreachEvent(int damageSeverity, int oxygenLoss)
        {
            _damageSeverity = damageSeverity;
            _oxygenLoss = oxygenLoss;
        }

        public string Description => $"Hull breach emergency! Severity: {_damageSeverity}. Losing {_oxygenLoss} oxygen!";

        public void Apply(Station station)
        {
            // Attempt defense using defense module
            var defense = station.Modules.Find(m => m is DefenseModule) as DefenseModule;
            
            // Check if defense succeeded
            if (defense != null && defense.DefendAgainstEvent(_damageSeverity))
            {
                Console.WriteLine("[DEFENSE] Hull breach contained!");
                return; // Skip damage if defense successful
            }

            // Apply damage to random modules based on severity
            for (int i = 0; i < _damageSeverity && station.Modules.Count > 0; i++)
            {
                // Select random module
                var module = station.Modules[new Random().Next(station.Modules.Count)];
                
                // Damage the module
                module.IsDamaged = true;
                Console.WriteLine($"[DAMAGE] {module.Name} damaged by hull breach!");
            }

            // Deduct oxygen resources
            station.Resources.Consume("Oxygen", _oxygenLoss);
        }
    }
}