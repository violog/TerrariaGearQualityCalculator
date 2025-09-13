using log4net;
using Terraria;

namespace TerrariaGearQualityCalculator;

internal static class Logger
{
    private const int Level = 4;
    private const bool ChatEnabled = true;
    private static readonly ILog Log = LogManager.GetLogger("TerrariaGearQualityCalculator");

    internal static void Debug(string message)
    {
        Log.Debug(message);
        WriteInChat(message, 4, 175, 175, 175);
    }

    internal static void Info(string message)
    {
        Log.Info(message);
        WriteInChat(message, 3, 255, 255, 255);
    }

    internal static void Warn(string message)
    {
        Log.Warn(message);
        WriteInChat(message, 2, 255, 243, 63);
    }

    internal static void Error(string message)
    {
        WriteInChat(message, 2, 255, 58, 58);
        Log.Error(message);
    }

    private static void WriteInChat(string message, int level, byte r, byte g, byte b)
    {
        if (!ChatEnabled || Level < level) return;

        message = $"[TerrariaGearQualityCalculator] {message}";
        Main.NewText(message, r, g, b);
    }
}