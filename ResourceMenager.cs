namespace SpaceStationSurvival
{
    // Resource types in the game
    public class ResourceManager
    {
        public int Energy { get; set; } = 100;
        public int Oxygen { get; set; } = 100;
        public int Water { get; set; } = 80;
        public int Food { get; set; } = 70;

        // Consume resource, not below zero
        public void Consume(string resource, int amount)
        {
            if (resource == "Energy") Energy = Math.Max(0, Energy - amount);
            if (resource == "Oxygen") Oxygen = Math.Max(0, Oxygen - amount);
            if (resource == "Water") Water = Math.Max(0, Water - amount);
            if (resource == "Food") Food = Math.Max(0, Food - amount);
        }

        // Add resource
        public void Produce(string resource, int amount)
        {
            if (resource == "Energy") Energy += amount;
            if (resource == "Oxygen") Oxygen += amount;
            if (resource == "Water") Water += amount;
            if (resource == "Food") Food += amount;
        }
    }
}
