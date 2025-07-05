namespace SpaceStationSurvival
{
    // Represents the main space station, containing modules and managing resources.
    public class Station
    {
        // List of modules attached to the station.
        public List<IModule> Modules { get; } = new List<IModule>();

        // Manages the station's resources.
        public ResourceManager Resources { get; } = new ResourceManager();

    }
    
} 