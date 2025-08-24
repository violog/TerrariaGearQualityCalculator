using System.Collections.Generic;
using TerrariaGearQualityCalculator.Calculators;

namespace TerrariaGearQualityCalculator.Storage;

public interface IBackend
{
    // Load fills Bosses list and be called only once, preferably in constructor
    internal List<ICalculation> Load();
    // Save stores new or updates an existing boss entry
    internal void Store(List<ICalculation> calculations);
}
