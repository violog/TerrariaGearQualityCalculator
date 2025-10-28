using System.Globalization;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class CalculationModel : ICalculationModelWritable
{
    // Strangely enough, this works on language switch in main menu,
    // which means that static classes of the mod are loaded upon entering a world
    static CalculationModel()
    {
        // preserve this order in values
        StaticDetailsAttributes =
        [
            GetText("Sr"),
            GetText("PlayerTime"),
            GetText("BossTime"),
            GetText("PlayerDps"),
            GetText("BossRemainingHp"),
            GetText("BossDps"),
            // already present in vanilla Terraria
            Language.GetText("CreativePowers.TabWeapons"),
            Language.GetText("CreativePowers.TabArmor"),
            Language.GetText("CreativePowers.TabAccessories")
        ];
    }

    internal static CalculationModel TryCreate(TrivialCalculation calculation)
    {
        var ok = ContentSamples.NpcsByNetId.TryGetValue(calculation.Id, out var npc);
        if (ok) return new CalculationModel(calculation, npc);

        var modNpc = ModContent.GetContent<ModNPC>().FirstOrDefault(n => n.NPC.netID == calculation.Id);
        if (modNpc == null)
        {
            TGQC.Log.Warn(
                $"NPC {calculation.Id} not found; the respective mod could have been unloaded; skipping.");
            return null;
        }

        npc = modNpc.NPC;

        return new CalculationModel(calculation, npc);
    }

    private CalculationModel(TrivialCalculation calculation, NPC npc)
    {
        _id = calculation.Id;
        Name = npc.FullName;
        Update(calculation);
    }

    private static LocalizedText[] StaticDetailsAttributes { get; }
    private readonly int _id;

    public string Name { get; }
    public string Sr { get; private set; }

    public LocalizedText[] DetailsAttributes => StaticDetailsAttributes;

    public string[] DetailsValues { get; private set; }

    public void Update(ICalculation calculation)
    {
        Sr = FormatFixed(calculation.Sr, 3);
        var calc = (TrivialCalculation)calculation;
        DetailsValues =
        [
            Sr,
            FormatFixed(calc.PlayerTime, 3),
            FormatFixed(calc.BossTime, 3),
            FormatFixed(calc.PlayerDps),
            FormatFixed(calc.BossRemainingHp),
            FormatFixed(calc.BossDps),
            string.Join(", ", calc.Gear.Weapons),
            $"{calc.Gear.Helmet}, {calc.Gear.Chest}, {calc.Gear.Legs}",
            string.Join(", ", calc.Gear.Accessories)
        ];
    }

    internal bool IsDefault() => _id == 0 && Name == "";

    // ToString is used for debugging and may be not updated with latest changes
    public override string ToString()
    {
        var details = string.Join(", ", DetailsAttributes.Zip(DetailsValues, (attr, value) => $"{attr}: {value}"));
        return $"Id={_id} Name={Name} Details={details}";
    }

    private static string FormatFixed(double value, int digits = 0)
    {
        if (value < 0)
            value = 0; // shouldn't have negative values, this happened to BossRemainingHp

        return double.IsPositiveInfinity(value)
            ? GetText("Infinity").ToString()
            : value.ToString($"F{digits}", CultureInfo.InvariantCulture);
    }

    private static LocalizedText GetText(string key)
    {
        const string localizationPrefix = "Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.";
        return Language.GetText(localizationPrefix + key);
    }
}