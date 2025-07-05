namespace SpaceStationSurvival
{
    // Provides a console-based user interface for the game
    public class ConsoleUI
    {
        private readonly GameEngine engine;
        
        // Initializes with game engine dependency
        public ConsoleUI(GameEngine engine) => this.engine = engine;

        // Main game loop
        public void Start()
        {
            Console.WriteLine("=== Space Station Survival ===");
            while (!engine.IsGameOver())
            {
                ShowStatus();
                ShowMenu();
                HandleInput();
            }
            
            string? reason = engine.GetGameOverReason();
            Console.WriteLine($"GAME OVER! Reason: {reason}");
            ShowStatus();
        }

        #region Display Methods
        // Shows current station status
        private void ShowStatus()
        {
            Console.ResetColor();
            Console.WriteLine("\n=== STATION STATUS ===");
            Console.WriteLine($"Turn: {engine.Turn}");

            // Display resources with color-coded warnings
            DisplayResource("Energy", engine.Station.Resources.Energy, 20);
            DisplayResource("Oxygen", engine.Station.Resources.Oxygen, 30);
            DisplayResource("Water", engine.Station.Resources.Water, 15);
            DisplayResource("Food", engine.Station.Resources.Food, 10);

            // Display module status
            Console.WriteLine("\nModules:");
            foreach (var module in engine.Station.Modules)
            {
                string status = module.IsDamaged ? "[DAMAGED]" : "[OPERATIONAL]";
                string levelInfo = $"(Level: {module.Level}, Exp: {module.Exp}/{module.ExpThreshold})";

                Console.ForegroundColor = module.IsDamaged ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine($"- {module.Name} {status} {levelInfo}");
                Console.ResetColor();

                Console.WriteLine(module.IsDamaged
                    ? $"  Repair cost: {FormatCosts(module.RepairCost)}"
                    : $"  Upgrade with: {GetUpgradeCostInfo(module)}");
            }
            
            // Display next turn costs
            Console.WriteLine("\nNext turn maintenance costs:");
            foreach (var cost in CalculateNextTurnCosts())
            {
                Console.WriteLine($"- {cost.Key}: {cost.Value}");
            }
        }

        // Displays a resource value with color coding
        private void DisplayResource(string name, int value, int warningThreshold)
        {
            Console.ForegroundColor = value < warningThreshold ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine($"{name}: {value}");
            Console.ResetColor();
        }

        // Calculates resource costs for the next turn
        private Dictionary<string, int> CalculateNextTurnCosts()
        {
            var costs = new Dictionary<string, int>
            {
                {"Energy", 5}, {"Oxygen", 8}, {"Water", 3}, {"Food", 4}
            };
            
            // Add module maintenance costs
            foreach (var module in engine.Station.Modules)
            {
                if (module.IsDamaged) continue;
                
                foreach (var cost in module.MaintenanceCost)
                {
                    costs.TryGetValue(cost.Key, out int current);
                    costs[cost.Key] = current + cost.Value;
                }
            }
            
            return costs;
        }
        #endregion

        #region Menu System
        // Displays main menu options
        private void ShowMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Advance turn");
            Console.WriteLine("2. Upgrade module");
            Console.WriteLine("3. Repair module");
            Console.WriteLine("4. Save game");
            Console.WriteLine("5. Load game");
            Console.WriteLine("Enter choice:");
        }

        // Handles user menu selection
        private void HandleInput()
        {
            switch (Console.ReadLine())
            {
                case "1": engine.ProcessTurn(); break;
                case "2": UpgradeModule(); break;
                case "3": RepairModule(); break;
                case "4": SaveGame(); break;
                case "5": LoadGame(); break;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
        #endregion

        #region Module Management
        // Handles module upgrade process
        private void UpgradeModule()
        {
            var modules = engine.Station.Modules;
            Console.WriteLine("Select module to upgrade:");
            
            // List available modules
            for (int i = 0; i < modules.Count; i++)
            {
                var module = modules[i];
                Console.WriteLine($"{i + 1}. {module.Name} (Level: {module.Level}, Exp: {module.Exp}/{module.ExpThreshold})");
                Console.WriteLine($"   Upgrade cost: {GetUpgradeCostInfo(module)}");
            }

            // Validate module selection
            if (!int.TryParse(Console.ReadLine(), out int choice) || 
                choice < 1 || choice > modules.Count)
            {
                Console.WriteLine("Invalid module choice.");
                return;
            }

            var selectedModule = modules[choice - 1];
            
            // Check if module is damaged
            if (selectedModule.IsDamaged)
            {
                Console.WriteLine("Cannot upgrade a damaged module! Repair it first.");
                return;
            }
            
            // Show upgrade details
            Console.WriteLine($"\n=== {selectedModule.Name} Upgrade ===");
            Console.WriteLine(GetModuleDescription(selectedModule));
            Console.WriteLine($"Next level requires: {selectedModule.ExpThreshold - selectedModule.Exp} EXP");
            
            // Get available resources for upgrade
            var upgradeResources = selectedModule.ResourceToExpConversion.Keys.ToList();
            Console.WriteLine("\nEnter resource to use for upgrade:");
            Console.WriteLine($"Available: {string.Join(", ", upgradeResources)}");
            
            string resource = Console.ReadLine()?.Trim() ?? "";
            if (!upgradeResources.Contains(resource))
            {
                Console.WriteLine($"Invalid resource. Valid options: {string.Join(", ", upgradeResources)}");
                return;
            }

            // Calculate maximum possible allocation
            int maxAmount = GetResourceAmount(resource);
            int expPerUnit = selectedModule.ResourceToExpConversion[resource];
            int expNeeded = selectedModule.ExpThreshold - selectedModule.Exp;
            int maxEffective = Math.Min(maxAmount, (int)Math.Ceiling((double)expNeeded / expPerUnit));
            
            // Get allocation amount from user
            Console.WriteLine($"Enter amount to allocate (1 {resource} = {expPerUnit} EXP, max: {maxEffective}):");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }
            
            if (amount > maxEffective)
            {
                Console.WriteLine($"Cannot allocate more than {maxEffective}!");
                return;
            }

            // Confirm upgrade
            Console.WriteLine($"\nConfirm upgrade investment? ({amount} {resource} = {amount * expPerUnit} EXP) (Y/N)");
            if (Console.ReadLine()?.Trim().ToUpper() != "Y")
            {
                Console.WriteLine("Upgrade canceled.");
                return;
            }

            // Apply upgrade
            selectedModule.AllocateResource(resource, amount);
            engine.Station.Resources.Consume(resource, amount);
            Console.WriteLine($"Invested {amount} {resource} in {selectedModule.Name}");
            Console.WriteLine($"Current EXP: {selectedModule.Exp}/{selectedModule.ExpThreshold}");
        }

        // Handles module repair process
        private void RepairModule()
        {
            var damagedModules = engine.Station.Modules
                .Where(m => m.IsDamaged)
                .ToList();

            if (damagedModules.Count == 0)
            {
                Console.WriteLine("No damaged modules to repair.");
                return;
            }

            // List damaged modules
            Console.WriteLine("\nSelect module to repair:");
            for (int i = 0; i < damagedModules.Count; i++)
            {
                var module = damagedModules[i];
                Console.WriteLine($"{i + 1}. {module.Name} Cost: {FormatCosts(module.RepairCost)}");
            }

            // Validate selection
            if (!int.TryParse(Console.ReadLine(), out int choice) || 
                choice < 1 || choice > damagedModules.Count)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            var moduleToRepair = damagedModules[choice - 1];
            
            // Check resources and repair
            if (CanAffordRepair(moduleToRepair.RepairCost))
            {
                foreach (var cost in moduleToRepair.RepairCost)
                {
                    engine.Station.Resources.Consume(cost.Key, cost.Value);
                }
                moduleToRepair.Repair();
                Console.WriteLine($"[REPAIR] {moduleToRepair.Name} has been repaired");
            }
            else
            {
                Console.WriteLine("Insufficient resources for repair!");
                Console.WriteLine($"Required: {FormatCosts(moduleToRepair.RepairCost)}");
            }
        }
        #endregion

        #region Helper Methods
        // Formats upgrade cost information
        private string GetUpgradeCostInfo(IModule module)
        {
            return string.Join(", ", 
                module.ResourceToExpConversion.Select(kvp => $"{kvp.Value} EXP per {kvp.Key}"));
        }

        // Gets module description based on type
        private string GetModuleDescription(IModule module) => module switch
        {
            PowerModule pm => $"Generates Energy. Maintenance: {FormatCosts(pm.MaintenanceCost)}",
            LifeSupportModule lsm => $"Produces Oxygen. Maintenance: {FormatCosts(lsm.MaintenanceCost)}",
            HabitationModule hm => $"Produces Food/Water. Maintenance: {FormatCosts(hm.MaintenanceCost)}",
            ResearchModule rm => $"Research bonuses. Maintenance: {FormatCosts(rm.MaintenanceCost)}",
            DefenseModule dm => $"Station defense. Maintenance: {FormatCosts(dm.MaintenanceCost)}",
            _ => "General purpose module"
        };

        // Formats cost dictionary to string
        private string FormatCosts(Dictionary<string, int> costs) => 
            string.Join(", ", costs.Select(c => $"{c.Value} {c.Key}"));

        // Gets current amount of a resource
        private int GetResourceAmount(string resource) => resource switch
        {
            "Energy" => engine.Station.Resources.Energy,
            "Oxygen" => engine.Station.Resources.Oxygen,
            "Water" => engine.Station.Resources.Water,
            "Food" => engine.Station.Resources.Food,
            _ => 0
        };

        // Checks if station can afford repair costs
        private bool CanAffordRepair(Dictionary<string, int> repairCost)
        {
            foreach (var cost in repairCost)
            {
                if (GetResourceAmount(cost.Key) < cost.Value) 
                    return false;
            }
            return true;
        }
        #endregion

        #region Save/Load System
        // Handles game saving
        private void SaveGame()
        {
            Console.Write("\nEnter save file name: ");
            string fileName = Console.ReadLine() + ".json";
            engine.SaveGame(fileName);
        }

        // Handles game loading
        private void LoadGame()
        {
            Console.Write("\nEnter save file name: ");
            string fileName = Console.ReadLine() + ".json";
            engine.LoadGame(fileName);
        }
        #endregion
    }
}