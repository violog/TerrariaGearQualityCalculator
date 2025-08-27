using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Terraria;
using TerrariaGearQualityCalculator.Calculators;
using TerrariaGearQualityCalculator.Calculators.Trivial;

namespace TerrariaGearQualityCalculator.Storage;

// FileBackend stores all calculations in a file with the following structure:
// { "Type": "DotnetICalculationImplementationTypeName", "Items": [calculations...]}
public class FileBackend<T>(string filePath) : IBackend where T : ICalculation
{
    private string FilePath { get; } = filePath;

    public List<ICalculation> Load()
    {
        if (!File.Exists(FilePath))
        {
            var cc = Write([]);
            return cc.Cast<ICalculation>().ToList();
        }

        // The file is not that big to read it async
        var raw = File.ReadAllBytes(FilePath);
        List<T> list = [];
        try
        {
            var file = JsonSerializer.Deserialize<Head>(raw)!;
            list = JsonSerializer.Deserialize<List<T>>(file.Items.GetRawText())!;
        }
        catch (Exception e)
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var dst = $"{FilePath}.backup-${time}.json";
            File.Copy(FilePath, FilePath, true);
            list = Write([]);
            // TODO: find a way to log the error, for now chat will suffice
            // var log = ModContent.GetInstance<TerrariaGearQualityCalculator>().Instance.Logger;
            Main.NewText($"FileStorage.Load failed, the old file backed up to {dst}, the new file was created. {e}",
                255, 0, 0);
        }

        return list.Cast<ICalculation>().ToList();
    }

    public void Store(List<ICalculation> calculations)
    {
        if (!File.Exists(FilePath))
        {
            Write([]);
            Main.NewText($"Called Save() before initialization for file {FilePath}", 128, 255, 0);
            return;
        }

        var list = calculations.Cast<T>().ToList();
        Write(list);
    }

    // Writes empty or filled calculations and returns them.
    //
    // This backend updates the entire list at once. This is not much, assuming
    // 64 bytes per boss entry, 200 bosses == 12KB per update == 12 KB/s,
    // which is tiny compared to how other programs write dozens and hundreds MB/s.
    private List<T> Write(List<T> calculations)
    {
        if (calculations.Count == 0) calculations = Initializer.Init().Cast<T>().ToList();

        var items = JsonSerializer.SerializeToElement(calculations)!;
        var head = new Head(typeof(T).FullName, items);
        var rawHead = JsonSerializer.SerializeToUtf8Bytes(head);
        File.WriteAllBytes(FilePath, rawHead);
        return calculations;
    }

    private record Head(string Type, JsonElement Items);
}