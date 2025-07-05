namespace SpaceStationSurvival
{
    // Represents a research module in the space station
    public class ResearchModule : ModuleBase
    {
        public ResearchModule() : base("Research Module") { }

        public override int ExpThreshold => 5 + Level;
        public override Dictionary<string, int> MaintenanceCost => new Dictionary<string, int>
        {
            {"Energy", 1}, 
            {"Oxygen", 1}
        };      
        public override Dictionary<string, int> ResourceToExpConversion { get; } = new Dictionary<string, int>
        {
            {"Energy", 1},
            {"Oxygen", 1}
        };
        public override Dictionary<string, int> RepairCost { get; } = new()
        {
            {"Energy", 10},
            {"Water", 8}
        };
        public override void PerformTick(Station station)
        {
            if (IsDamaged) return;

            // Research gives random bonuses
            if (new Random().NextDouble() < (Level * 0.05))
            {
                var resource = new[] { "Energy", "Oxygen", "Water", "Food" }[new Random().Next(4)];
                int bonus = Level * 2;
                station.Resources.Produce(resource, bonus);
                Console.WriteLine($"[RESEARCH] Discovered efficiency boost! Gained {bonus} {resource}");
            }
        }
    }
}