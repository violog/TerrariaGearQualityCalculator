using System.IO;
using Terraria;
using TerrariaGearQualityCalculator.Calculators.Trivial;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator;

// State is a communication layer between UI and backend
internal class State
{
    private const string DbDirName = "TerrariaGearQualityCalculator";
    private const string FileName = "TrivialCalculation.json";

    internal State()
    {
        var dirPath = string.Concat(Main.SavePath, Path.DirectorySeparatorChar, DbDirName);
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

        var dbPath = string.Concat(dirPath, Path.DirectorySeparatorChar, FileName);
        var backend = new FileBackend<TrivialCalculation>(dbPath);
        Storage = new ModelStorage(backend);
    }

    internal ModelStorage Storage { get; private set; }
}