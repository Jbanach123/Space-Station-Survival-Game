// The <c>GameEngine</c> class manages the core logic of the Space Station Survival game,
// including station state, turn progression, event handling, and save/load functionality.

using System.Text.Json;
namespace SpaceStationSurvival
{
    public class GameEngine
    {
        // Current station instance
        public Station Station { get; } = new Station();
        
        // Event generator
        private EventFactory factory = new EventFactory();
        
        // Current game turn counter
        public int Turn { get; private set; }

        // Initialize game state with starting modules
        public GameEngine()
        {
            Station.Modules.Add(new PowerModule());
            Station.Modules.Add(new LifeSupportModule());
            Station.Modules.Add(new HabitationModule());
            Station.Modules.Add(new ResearchModule());
            Station.Modules.Add(new DefenseModule());
        }

        // Determine game over reason based on resource thresholds
        public string? GetGameOverReason()
        {
            var resources = Station.Resources;
    
            if (resources.Energy <= 0) 
                return "CRITICAL FAILURE: Energy reserves depleted. Life support systems offline.";
            
            if (resources.Oxygen <= 0) 
                return "FATAL ERROR: Oxygen levels at zero. Crew unconscious.";
            
            if (resources.Water <= 0) 
                return "SYSTEM COLLAPSE: Water reserves exhausted. Cooling systems failed.";
            
            if (resources.Food <= 0) 
                return "CREW STARVATION: Food supplies depleted. Survival impossible.";
            
            // Additional loss conditions
            if (Station.Modules.All(m => m.IsDamaged))
                return "TOTAL SYSTEM FAILURE: All station modules damaged beyond repair.";
            
            bool lifeSupportDead = Station.Modules.OfType<LifeSupportModule>().All(m => m.IsDamaged);
            bool powerDead = Station.Modules.OfType<PowerModule>().All(m => m.IsDamaged);
            
            if (lifeSupportDead && powerDead)
                return "DUAL SYSTEM FAILURE: Both power and life support systems destroyed.";
            
            return null;
        }

        // Check if game has ended
        public bool IsGameOver() => GetGameOverReason() != null;
            
        // Process a full game turn sequence
        public void ProcessTurn()
        {
            Turn++;
            Console.WriteLine($"\n=== Turn {Turn} ===");
            
            // Module production phase
            foreach (var module in Station.Modules)
                module.PerformTick(Station);
            
            // Resource consumption phase
            ApplyMaintenanceCosts();
            
            // Random event phase
            var ev = factory.CreateEvent(Turn);
            Console.WriteLine($"\n[EVENT] {ev.Description}");
            ev.Apply(Station);
        }

        // Calculate and apply resource maintenance costs
        private void ApplyMaintenanceCosts()
        {
            Console.WriteLine("\n[MAINTENANCE] Resource consumption:");
            var totalCosts = new Dictionary<string, int>();
            
            // Base station maintenance
            var baseCosts = new Dictionary<string, int>
            {
                {"Energy", 8},
                {"Oxygen", 6},
                {"Water", 4},
                {"Food", 5}
            };
            
            // Apply base costs
            foreach (var cost in baseCosts)
            {
                totalCosts[cost.Key] = cost.Value;
                Console.WriteLine($"- Base: {cost.Value} {cost.Key}");
            }
            
            // Module operational costs
            foreach (var module in Station.Modules)
            {
                if (!module.IsDamaged)
                {
                    foreach (var cost in module.MaintenanceCost)
                    {
                        if (!totalCosts.ContainsKey(cost.Key))
                            totalCosts[cost.Key] = 0;
                        
                        totalCosts[cost.Key] += cost.Value;
                        Console.WriteLine($"- {module.Name}: {cost.Value} {cost.Key}");
                    }
                }
            }
            
            // Deduct resources from station
            foreach (var cost in totalCosts)
            {
                Station.Resources.Consume(cost.Key, cost.Value);
                Console.WriteLine($"Total {cost.Key} consumed: {cost.Value}");
            }
        }
        
        // Save current game state to JSON file
        public void SaveGame(string fileName)
        {
            var state = new GameState
            {
                Turn = Turn,
                Energy = Station.Resources.Energy,
                Oxygen = Station.Resources.Oxygen,
                Water = Station.Resources.Water,
                Food = Station.Resources.Food
            };
            
            foreach (var module in Station.Modules)
            {
                state.Modules.Add(new ModuleState
                {
                    Name = module.Name,
                    IsDamaged = module.IsDamaged,
                    Level = module.Level,
                    Exp = module.Exp,
                });
            }
            
            File.WriteAllText(fileName, JsonSerializer.Serialize(state));
            Console.WriteLine($"Game saved to {fileName}");
        }
        
        // Load game state from JSON file
        public void LoadGame(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Save file not found");
                return;
            }

            var jsonString = File.ReadAllText(fileName);
            var state = JsonSerializer.Deserialize<GameState>(jsonString);

            if (state == null)
            {
                Console.WriteLine("Failed to load game: save file is corrupted or invalid.");
                return;
            }

            Turn = state.Turn;
            Station.Resources.Energy = state.Energy;
            Station.Resources.Oxygen = state.Oxygen;
            Station.Resources.Water = state.Water;
            Station.Resources.Food = state.Food;

            // Restore module states
            foreach (var savedModule in state.Modules)
            {
                var module = Station.Modules.FirstOrDefault(m => m.Name == savedModule.Name);
                if (module != null)
                {
                    module.IsDamaged = savedModule.IsDamaged;
                    module.LoadState(savedModule.Level, savedModule.Exp);
                }
            }

            Console.WriteLine($"Game loaded from {fileName}");
        }
    }
}


