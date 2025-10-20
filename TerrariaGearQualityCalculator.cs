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

    // The calculator only works correctly for singleplayer, which means,
    // no calculations must be modified in multiplayer -- read-only access.
    internal static readonly bool IsSingleplayer = Main.netMode == NetmodeID.SinglePlayer;
    internal static Log Log { get; private set; }
    internal static ModelStorage Storage { get; private set; }

    public override void Load()
    {
        if (!IsSingleplayer)
        {
            Log = new Log(true, 2);
            Log.Warn(
                "Calculator will be opened read-only in multiplayer. You can view stats, but boss fight data will not be updated.");
            return;
        }

        CalculatorHotKey = KeybindLoader.RegisterKeybind(this, "GearQualityCalculator", "P");
        Log = new Log(true); // make configurable?
    }

    public override void PostSetupContent()
    {
        // must be here to load modded bosses properly
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