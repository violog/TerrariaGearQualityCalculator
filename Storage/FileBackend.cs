using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Terraria;
using TerrariaGearQualityCalculator.Calculators;
using Terraria.Utilities;

namespace TerrariaGearQualityCalculator.Storage;

// File structure:
// { "Type": "Namespace.CalculationType, AssemblyName", "Items": [...]}
// maybe not
public class FileBackend<T> (string filePath): IBackend where T: ICalculation
{
    private record Head(string Type, JsonElement Items);
    
    // no, this assumes duplication
    // public List<ICalculation> Calculations { get; private set;  } = [];
    // private Type CalcType { get; } = calcType;
    private string FilePath { get; } = filePath;
    
    public List<ICalculation> Load()
    {
        // also, don't forget that file might exist, but be corrupted or even empty due to no encounters present
        if (!FileUtilities.Exists(FilePath, false))
        {
            Write([]);
            return [];
        }
        // The file is not that big to read it async
        var raw = FileUtilities.ReadAllBytes(FilePath, false);
        // var listType = typeof(List<>).MakeGenericType(CalcType);
        // JsonSerializer.Deserialize(raw, listType);
        // var file = JsonSerializer.Deserialize<CalculationEntry>(raw)!;
        List<T> list = [];
        try
        {
            var file = JsonSerializer.Deserialize<Head>(raw)!;
            list = JsonSerializer.Deserialize<List<T>>(file.Items.GetRawText())!;
            // list = file.Items.Deserialize<List<T>>()!;
        }
        catch (Exception e)
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var dst = $"{FilePath}.backup-${time}.json";
            FileUtilities.Copy(FilePath, FilePath, false, true);
            Write([]);
            // TODO: find a way to log the error
            // var log = ModContent.GetInstance<TerrariaGearQualityCalculator>().Instance.Logger;
            // this should be tested - it appeared not to be working
            Main.NewText($"FileStorage.Load failed, the old file backed up to {dst}, the new file was created. {e}", 255, 0, 0);
        }

        return list.Cast<ICalculation>().ToList();
    }

    public void Store(List<ICalculation> calculations) 
    {
        if (!FileUtilities.Exists(FilePath, false))
        {
            Write([]);
            Main.NewText($"Called Save() before initialization for file {FilePath}", 128, 255, 0);
            return;
        }
        var list = calculations.Cast<T>().ToList();
        Write(list);
    }

    private void Write(List<T> calculations)
    {
        var items = new JsonElement();
        if (calculations.Count > 0)
        {
            items = JsonSerializer.SerializeToElement(calculations)!;
        }
        
        var head = new Head(typeof(T).FullName, items);
        var rawHead = JsonSerializer.SerializeToUtf8Bytes(head);
        FileUtilities.WriteAllBytes(FilePath, rawHead, false);
    }
}