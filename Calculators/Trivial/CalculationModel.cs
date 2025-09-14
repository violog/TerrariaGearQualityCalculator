using System.Globalization;
using System.Linq;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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

    // CalculationModel throws InvalidOperationException when NPC with such ID is not found: ensure to catch it
    public CalculationModel(TrivialCalculation calculation)
    {
        var npc = ContentSamples.NpcsByNetId[calculation.Id];
        if (npc == null)
        {
            var modNpc = ModContent.GetContent<ModNPC>()
                .First(n => n.NPC.netID == calculation.Id);
            npc = modNpc.NPC;
        }

        Name = npc.FullName;
        Update(calculation);
    }

    private static LocalizedText[] StaticDetailsAttributes { get; }

    public string Name { get; }
    public double Sr { get; private set; }

    public LocalizedText[] DetailsAttributes => StaticDetailsAttributes;

    public string[] DetailsValues { get; private set; }

    public void Update(ICalculation calculation)
    {
        Sr = calculation.Sr;
        var calc = (TrivialCalculation)calculation;
        DetailsValues =
        [
            FormatFixed(Sr, 3),
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

    // ToString is used for debugging and may be not updated with latest changes
    public override string ToString()
    {
        var details = string.Join(", ", DetailsAttributes.Zip(DetailsValues, (attr, value) => $"{attr}: {value}"));
        return $"Name=${Name} DetailsValues={details}";
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