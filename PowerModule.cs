namespace SpaceStationSurvival
{
    // Represents a power-generating module in the space station
    public class PowerModule : ModuleBase
    {
        public PowerModule() : base("Power Module") { }
        public override int ExpThreshold => 8 + (Level * 2);
        public override Dictionary<string, int> MaintenanceCost => new Dictionary<string, int>
        {
            {"Water", 2 + (Level / 2)}
        };
        
        // Resource conversion rates for upgrades
        public override Dictionary<string, int> ResourceToExpConversion { get; } = new Dictionary<string, int>
        {
            {"Energy", 2},
            {"Water", 1}
        };

        // Repair requirements (Energy + Water)
        public override Dictionary<string, int> RepairCost { get; } = new()
        {
            {"Energy", 15},
            {"Water", 8}
        };

        // Energy generation each turn
        public override void PerformTick(Station station)
        {
            // Skip if damaged
            if (IsDamaged) return;

            // Production formula: 8 + (Level * 3)
            int production = 8 + (Level * 3); 
            
            // Generate energy
            station.Resources.Produce("Energy", production);
            Console.WriteLine($"[POWER] Produced: {production} Energy");
        }
    }
}