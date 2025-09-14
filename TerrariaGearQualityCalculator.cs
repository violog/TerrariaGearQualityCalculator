using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Calculators.Trivial;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator;

internal sealed class TerrariaGearQualityCalculator : Mod
{
    private const string StorageFileName = "TrivialCalculation.json";

    internal static ModKeybind CalculatorHotKey;
    internal static Log Log { get; private set; }
    internal static ModelStorage Storage { get; private set; }

    public override void Load()
    {
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            Logger.Error("TerrariaGearQualityCalculator is not supported in multiplayer!");
            return;
        }

        CalculatorHotKey = KeybindLoader.RegisterKeybind(this, "GearQualityCalculator", "P");
        Log = new Log();
        var backend = new FileBackend<TrivialCalculation>(StorageFileName);
        Storage = new ModelStorage(backend);
    }

    public override void Unload()
    {
        CalculatorHotKey = null;
        Log = null;
        Storage = null;
    }
}