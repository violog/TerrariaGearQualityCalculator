using System;
using System.Globalization;
using Terraria.ID;
using Terraria.Localization;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class CalculationModel : ICalculationModelWritable
{
    // TODO: test this on language change when we add russian localization, I assume it will break
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

    public CalculationModel(TrivialCalculation calculation)
    {
        var npc = ContentSamples.NpcsByNetId[calculation.Id];
        if (npc == null) throw new NullReferenceException($"NPC not found for id {calculation.Id}");

        Name = npc.FullName;
        Update(calculation);
    }

    private static LocalizedText[] StaticDetailsAttributes { get; }

    public string Name { get; }
    public decimal Sr { get; private set; }

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

    private static string FormatFixed(decimal value, int digits = 0)
    {
        return value == TrivialCalculation.Infinity
            ? GetText("Infinity").ToString()
            : value.ToString($"F{digits}", CultureInfo.InvariantCulture);
    }

    private static LocalizedText GetText(string key)
    {
        const string localizationPrefix = "Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.";
        return Language.GetText(localizationPrefix + key);
    }
}