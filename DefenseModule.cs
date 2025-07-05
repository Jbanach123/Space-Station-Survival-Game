namespace SpaceStationSurvival
{
    // Defense module for protecting the station against various events
    public class DefenseModule : ModuleBase
    {
        public DefenseModule() : base("Defense Module") { }
        public override int ExpThreshold => 6 + Level;
        public override Dictionary<string, int> MaintenanceCost => new Dictionary<string, int>
        {
            {"Energy", Level}
        };

        public override Dictionary<string, int> ResourceToExpConversion { get; } = new Dictionary<string, int>
        {
            {"Energy", 1},
            {"Water", 1}
        };

        public override Dictionary<string, int> RepairCost { get; } = new()
        {
            {"Energy", 25},
            {"Water", 10}
        };
        
        // Defense module doesn't perform any active production
        public override void PerformTick(Station station)
        {
            // Passive module - no action needed during regular ticks
            // Defense functionality is event-based through DefendAgainstEvent()
        }
        
        // Attempts to defend against a station event
        public bool DefendAgainstEvent(int intensity)
        {
            // Cannot defend if module is damaged
            if (IsDamaged) return false;
            
            // Base defense chance without any modifiers
            double baseChance = 0.4;
            
            // Bonus based on module level (3% per level)
            double levelBonus = 0.03 * Level;
            
            // Penalty based on event intensity (5% per intensity point)
            double intensityPenalty = 0.05 * intensity;
            
            // Calculate final defense chance (capped at 85%)
            double defenseChance = Math.Min(0.85, baseChance + levelBonus - intensityPenalty);
            
            // Determine if defense was successful
            return new Random().NextDouble() < defenseChance;
        }
    }
}