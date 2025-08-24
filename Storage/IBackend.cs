using System.Collections.Generic;
using TerrariaGearQualityCalculator.Calculators;

namespace TerrariaGearQualityCalculator.Storage;

public interface IBackend
{
    // Load fills Bosses list and be called only once, preferably in constructor
    internal List<ICalculation> Load();

    // Store stores the entire updated list
    internal void Store(List<ICalculation> calculations);
}