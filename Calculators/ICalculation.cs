namespace TerrariaGearQualityCalculator.Calculators;

public interface ICalculation
{
    // Id is a boss NPCID (we will decide later for modded)
    public int Id { get; }
    public decimal BossTime { get; }
    public decimal PlayerTime { get; }
    // Sr is Survivability Ratio that effectively shows gear quality for the specific boss fight
    public decimal Sr => PlayerTime / BossTime;
    // MarshalJson is used to store calculations somewhere in persistent storage
    // TODO: I wonder if the interface or the class itself will be serialized, as I need the class
    // public byte[] MarshalJson();
    // public byte[] UnmarshalJson();
}
