namespace SpaceStationSurvival
{
    public abstract class ModuleBase : IModule
    {
        public string Name { get; }
        public bool IsDamaged { get; set; }
        public virtual Dictionary<string, int> RepairCost { get; } = new Dictionary<string, int>();
        public virtual Dictionary<string, int> MaintenanceCost { get; } = new Dictionary<string, int>();
        
        // Właściwości z chronionymi setterami
        public int Level { get; protected set; } = 1;
        public int Exp { get; protected set; } = 0;
        public virtual int ExpThreshold { get; protected set; } = 10;
        // Resource-to-EXP conversion rates
        public virtual Dictionary<string, int> ResourceToExpConversion { get; } = new Dictionary<string, int>();

        // Initialize module with name
        public ModuleBase(string name) => Name = name;

        // Load saved state (level and EXP)
        public virtual void LoadState(int level, int exp)
        {
            Level = level;
            Exp = exp;
        }

        // Convert resources to EXP and handle level progression
        public void AllocateResource(string resourceName, int amount)
        {
            // Skip if resource not convertible
            if (!ResourceToExpConversion.TryGetValue(resourceName, out int factor))
                return;

            // Calculate gained EXP
            int expGained = amount * factor;
            int remainingExp = expGained;
            
            // Apply EXP in chunks (handles multiple level-ups)
            while (remainingExp > 0)
            {
                int expNeeded = ExpThreshold - Exp;
                
                // Level up if threshold reached
                if (expNeeded <= 0)
                {
                    LevelUp();
                    expNeeded = ExpThreshold;
                }
                
                // Apply EXP (partial level-ups)
                int expToApply = Math.Min(expNeeded, remainingExp);
                Exp += expToApply;
                remainingExp -= expToApply;
                
                // Final level check
                if (Exp >= ExpThreshold)
                {
                    LevelUp();
                }
            }
            
            Console.WriteLine($"[{Name}] Gained {expGained} EXP from {amount} {resourceName}");
        }
        
        // Increase module level and reset EXP
        private void LevelUp()
        {
            Level++;
            Exp = 0;
            Console.WriteLine($"[{Name}] Level increased to {Level}");
        }

        public void Repair() => IsDamaged = false;
        public abstract void PerformTick(Station station);
    }
}