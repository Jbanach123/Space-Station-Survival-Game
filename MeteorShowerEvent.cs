namespace SpaceStationSurvival{
    // Represents a meteor shower event that can impact the space station.
    public class MeteorShowerEvent : IStationEvent
    {
        // The intensity of the meteor shower, determining how many modules may be damaged.

        private readonly int _intensity;
        // Initializes a new instance of the MeteorShowerEvent class with the specified intensity.

        public MeteorShowerEvent(int intensity)
        {
            _intensity = intensity;
        }
        // Gets a description of the meteor shower event, including its intensity.
        public string Description => $"Meteor shower (intensity: {_intensity}) approaching!";

        // Applies the meteor shower event to the specified station.
        public void Apply(Station station)
        {
            // Attempts to use the station's defense module to deflect the meteor shower.
            var defense = station.Modules.Find(m => m is DefenseModule) as DefenseModule;
            if (defense != null && defense.DefendAgainstEvent(_intensity))
            {
                Console.WriteLine("[DEFENSE] Meteor shower deflected!");
                return;
            }
            // If unsuccessful, damages a number of random modules equal to the intensity.
            for (int i = 0; i < _intensity && station.Modules.Count > 0; i++)
            {
                var module = station.Modules[new Random().Next(station.Modules.Count)];
                module.IsDamaged = true;
                Console.WriteLine($"[DAMAGE] {module.Name} damaged by meteors!");
            }
        }
    }
}