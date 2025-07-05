namespace SpaceStationSurvival
{
    // Life support module for managing oxygen
    public class LifeSupportModule : ModuleBase
    {
        public LifeSupportModule() : base("Life Support Module") { }

        public override int ExpThreshold => 7 + Level;
        public override Dictionary<string, int> MaintenanceCost => new Dictionary<string, int>
        {
            {"Energy", 3 + (Level / 2)}  // Więcej energii przy wyższych poziomach
        };
        public override Dictionary<string, int> ResourceToExpConversion { get; } = new Dictionary<string, int>
        {
            {"Oxygen", 1},
            {"Energy", 1}
        };

        public override Dictionary<string, int> RepairCost { get; } = new()
        {
            {"Oxygen", 10},
            {"Energy", 5},
            {"Water", 3}
        };

        public override void PerformTick(Station station)
        {
            // Skip if module is non-functional
            if (IsDamaged) return;

            // Oxygen production scales with module level
            int production = 10 + (Level * 2);
            
            // Generate oxygen resources
            station.Resources.Produce("Oxygen", production);
            Console.WriteLine($"[LIFE SUPPORT] Produced: {production} Oxygen");
        }
    }
}
