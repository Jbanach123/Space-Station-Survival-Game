namespace SpaceStationSurvival{    
    // Represents a habitation module in the space station.
    // Responsible for consuming essential resources (Oxygen, Energy, Water, Food) each tick
    // and providing a morale bonus under certain conditions.
    public class HabitationModule : ModuleBase
    {
        public HabitationModule() : base("Habitation Module") { }

        public override int ExpThreshold => 5 + Level; 

        public override Dictionary<string, int> MaintenanceCost => new Dictionary<string, int>
        {
            {"Oxygen", 3 + Level}  
        };
        public override Dictionary<string, int> ResourceToExpConversion { get; } = new Dictionary<string, int>
        {
            {"Water", 1},
            {"Food", 1}
        };

        public override void PerformTick(Station station)
        {
            // Skip processing if module is damaged
            if (IsDamaged) return;

            // Calculate production based on current level
            int waterProduction = 4 + Level;
            int foodProduction = 4 + Level;

            // Generate water and food resources
            station.Resources.Produce("Water", waterProduction);
            station.Resources.Produce("Food", foodProduction);
            Console.WriteLine($"[HABITATION] Produced: {waterProduction} Water, {foodProduction} Food");
            
            // Calculate oxygen consumption (scales with level)
            int oxygenCost = Level * 2;
            
            // Consume oxygen if sufficient available
            if (station.Resources.Oxygen >= oxygenCost)
            {
                station.Resources.Consume("Oxygen", oxygenCost);
            }
        }
    }
}
    