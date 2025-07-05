// Entry point for the SpaceStationSurvival game application
namespace SpaceStationSurvival
{
    class Program
    {
        // Main method: application starts here
        static void Main(string[] args)
        {
            // Create a new instance of the game engine
            var engine = new GameEngine();

            // Create a new console UI, passing the game engine to it
            var ui = new ConsoleUI(engine);

            // Start the UI (and the game)
            ui.Start();
        }
    }
}