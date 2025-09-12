using System;
using Terraria.ID;
using Terraria.Localization;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class CalculationModel : ICalculationModelWritable
{
    private const string LocalizationPrefix = "Mods.TerrariaGearQualityCalculator.CalculationModels.Trivial.";

    // TODO: test this on language change, I assume it will break
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
            GetText("Weapon"),
            // already present in vanilla Terraria
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

    private TrivialCalculation Calc { get; set; }
    public string Name { get; }
    public decimal Sr => (Calc as ICalculation).Sr;

    public LocalizedText[] DetailsAttributes => StaticDetailsAttributes;

    public string[] DetailsValues { get; private set; }

    public void Update(ICalculation calculation)
    {
        Calc = (TrivialCalculation)calculation;

        DetailsValues =
        [
            InfToString(Sr),
            InfToString(Calc.PlayerTime),
            InfToString(Calc.BossTime),
            InfToString(Calc.PlayerDps),
            InfToString(Calc.BossRemainingHp),
            InfToString(Calc.BossDps),
            Calc.Gear.Weapon,
            $"{Calc.Gear.Helmet}, {Calc.Gear.Chest}, {Calc.Gear.Legs}",
            string.Join(", ", Calc.Gear.Accessories.ToArray())
        ];
    }

    private static string InfToString(decimal value)
    {
        return value == TrivialCalculation.Infinity ? GetText("Infinity").ToString() : value.ToString();
    }

    private static LocalizedText GetText(string key)
    {
        return Language.GetText(LocalizationPrefix + key);
    }
}