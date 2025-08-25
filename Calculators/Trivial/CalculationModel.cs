using System;
using System.Globalization;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social.Steam;
using TerrariaGearQualityCalculator.Content.UI.Models;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class CalculationModel : ICalculationModel
{
    public string Name { get; }
    public decimal Sr => (Calc as ICalculation).Sr;
    public static LocalizedText[] DetailsAttributes { get; }
    public string[] DetailsValues { get; private set; }

    private TrivialCalculation Calc { get; }
    // this will be reused for gear
    // public LocalizedText[] GearDetails { get; set; }

    static CalculationModel()
    {
        // preserve this order in values
        DetailsAttributes =
        [
            Language.GetText("Mods.TerrariaGearQualityCalculator.Calculators.Trivial.Sr"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.Calculators.Trivial.PlayerTime"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.Calculators.Trivial.BossTime"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.Calculators.Trivial.PlayerDps"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.Calculators.Trivial.BossRemainingHp"),
        ];
    }

    public CalculationModel(TrivialCalculation calculation)
    {
        Calc = calculation;
        var npc = ContentSamples.NpcsByNetId[Calc.Id];
        if (npc == null)
        {
            throw new NullReferenceException($"NPC not found for id {Calc.Id}");
        }

        Name = npc.FullName;
        UpdateDetailsValues();
    }

    private void UpdateDetailsValues()
    {
        if (Calc.CacheValid) return;

        DetailsValues =
        [
            Sr.ToString(),
            Calc.PlayerTime.ToString(),
            Calc.BossTime.ToString(),
            Calc.PlayerDps.ToString(),
            Calc.BossRemainingHp.ToString(),
        ];

        Calc.CacheValid = true;
    }
}