namespace SpaceStationSurvival
{
    // Represents the state of a module in the space station.
    public class ModuleState
    {
        public string Name { get; set; } = string.Empty;
        public bool IsDamaged { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
    }
}
