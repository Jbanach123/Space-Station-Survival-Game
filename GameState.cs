namespace SpaceStationSurvival{    
    public class GameState
    // Represents the current state of the game, including resources and modules
    {
        public int Turn { get; set; }
        public int Energy { get; set; }
        public int Oxygen { get; set; }
        public int Water { get; set; }
        public int Food { get; set; }
        public List<ModuleState> Modules { get; set; } = new List<ModuleState>();
    }
}