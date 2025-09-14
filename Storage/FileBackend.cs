using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Terraria;
using TerrariaGearQualityCalculator.Calculators;
using TerrariaGearQualityCalculator.Calculators.Trivial;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Storage;

// FileBackend stores all calculations in a file with the following structure:
// { "Type": "DotnetICalculationImplementationTypeName", "Items": [calculations...]}
public class FileBackend<T> : IBackend where T : ICalculation
{
    private const string DbDirName = "TerrariaGearQualityCalculator";
    private string FilePath { get; }

    private readonly JsonSerializerOptions _jsonOpts = new()
        { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals };

    public FileBackend(string fileName)
    {
        var dirPath = string.Concat(Main.SavePath, Path.DirectorySeparatorChar, DbDirName);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        var dbPath = string.Concat(dirPath, Path.DirectorySeparatorChar, fileName);
        FilePath = dbPath;
    }

    public List<ICalculation> Load()
    {
        if (!File.Exists(FilePath))
        {
            var cc = Write([]);
            return cc.Cast<ICalculation>().ToList();
        }

        // The file is not that big to read it async
        var raw = File.ReadAllBytes(FilePath);
        List<T> list;
        try
        {
            var file = JsonSerializer.Deserialize<Head>(raw, _jsonOpts)!;
            list = JsonSerializer.Deserialize<List<T>>(file.Items.GetRawText(), _jsonOpts)!;
        }
        catch (Exception e)
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var dst = $"{FilePath}.backup-${time}.json";
            File.Copy(FilePath, FilePath, true);
            list = Write([]);
            TGQC.Log.Warn($"Failed to load storage, the old file backed up to {dst}, the new file was created. {e}");
        }

        return list.Cast<ICalculation>().ToList();
    }

    public void Store(List<ICalculation> calculations)
    {
        if (!File.Exists(FilePath))
        {
            Write([]);
            TGQC.Log.Info($"Called Store() before initialization for file {FilePath}");
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

        var items = JsonSerializer.SerializeToElement(calculations, _jsonOpts)!;
        var head = new Head(typeof(T).FullName, items);
        var rawHead = JsonSerializer.SerializeToUtf8Bytes(head, _jsonOpts);
        File.WriteAllBytes(FilePath, rawHead);
        return calculations;
    }

    private record Head(string Type, JsonElement Items);
}