using System;
using Terraria.ID;
using Terraria.Localization;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class CalculationModel : ICalculationModelWritable
{
    static CalculationModel()
    {
        // preserve this order in values
        StaticDetailsAttributes =
        [
            Language.GetText("Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.Sr"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.PlayerTime"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.BossTime"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.PlayerDps"),
            Language.GetText("Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.BossRemainingHp")
        ];
    }

    public CalculationModel(TrivialCalculation calculation)
    {
        var npc = ContentSamples.NpcsByNetId[calculation.Id];
        if (npc == null) throw new NullReferenceException($"NPC not found for id {Calc.Id}");

        Name = npc.FullName;
        Update(calculation);
    }

    private static LocalizedText[] StaticDetailsAttributes { get; }

    private TrivialCalculation Calc { get; set; }
    public string Name { get; }
    public decimal Sr => (Calc as ICalculation).Sr;

    public LocalizedText[] DetailsAttributes => StaticDetailsAttributes;

    public string[] DetailsValues { get; private set; }
    // this will be reused for gear
    // public LocalizedText[] GearDetails { get; set; }

    public void Update(ICalculation calculation)
    {
        Calc = (TrivialCalculation)calculation;

        DetailsValues =
        [
            Sr.ToString(),
            Calc.PlayerTime.ToString(),
            Calc.BossTime.ToString(),
            Calc.PlayerDps.ToString(),
            Calc.BossRemainingHp.ToString()
        ];
    }
}