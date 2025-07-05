namespace SpaceStationSurvival
{
    // Interface for a space station module
    public interface IModule
    {
        string Name { get; }
        bool IsDamaged { get; set; }
        Dictionary<string, int> RepairCost { get; }
        Dictionary<string, int> ResourceToExpConversion { get; }
        Dictionary<string, int> MaintenanceCost { get; }
        void PerformTick(Station station);
        void AllocateResource(string resourceName, int amount);
        void Repair();
        
        // Dodajemy nowe właściwości dla systemu ulepszeń
        // Zmienione na właściwości tylko do odczytu
        int Level { get; }
        int Exp { get; }
        int ExpThreshold { get; }
        
        // Nowa metoda do ładowania stanu
        void LoadState(int level, int exp);
    }
}
