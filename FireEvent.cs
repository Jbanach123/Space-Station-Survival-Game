namespace SpaceStationSurvival{    
    // Represents a fire outbreak event that can damage multiple modules
    public class FireEvent : IStationEvent
    {
        private readonly int _spreadFactor;  // How aggressively the fire spreads
        
        // Initializes with specified spread factor
        public FireEvent(int spreadFactor)
        {
            _spreadFactor = spreadFactor;
        }
        
        // Event description with spread factor
        public string Description => $"Fire outbreak in the station! Spread factor: {_spreadFactor}";
        
        // Applies fire event effects to the station
        public void Apply(Station station)
        {
            // Attempt defense using defense module
            var defense = station.Modules.OfType<DefenseModule>().FirstOrDefault();
            if (defense?.DefendAgainstEvent(_spreadFactor) == true)
            {
                Console.WriteLine("[DEFENSE] Fire contained by defense systems!");
                return;
            }

            var random = new Random();
            int damageCount = 0;
            
            // Damage modules based on spread factor
            foreach (var module in station.Modules)
            {
                // Each module has 25% base chance per spread factor point
                if (random.Next(0, 4) < _spreadFactor) 
                {
                    module.IsDamaged = true;
                    Console.WriteLine($"[DAMAGE] {module.Name} damaged by fire!");
                    damageCount++;
                }
            }
            
            // Resource cost for fire containment
            if (damageCount > 0)
            {
                int energyUsed = damageCount * 5;
                station.Resources.Consume("Energy", energyUsed);
                Console.WriteLine($"Used {energyUsed} energy to contain fire!");
            }
        }
    }
}