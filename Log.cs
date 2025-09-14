using log4net;
using Terraria;

namespace TerrariaGearQualityCalculator;

// Log wraps Mod.Logger to format messages and log in chat on need
internal class Log
{
    private const int Level = 4;
    private const bool ChatEnabled = true;
    private readonly ILog _log = LogManager.GetLogger("TerrariaGearQualityCalculator");

    internal void Debug(string message)
    {
        _log.Debug(message);
        WriteInChat(message, 4, 175, 175, 175);
    }

    internal void Info(string message)
    {
        _log.Info(message);
        WriteInChat(message, 3, 255, 255, 255);
    }

    internal void Warn(string message)
    {
        _log.Warn(message);
        WriteInChat(message, 2, 255, 243, 63);
    }

    internal void Error(string message)
    {
        WriteInChat(message, 2, 255, 58, 58);
        _log.Error(message);
    }

    private static void WriteInChat(string message, int level, byte r, byte g, byte b)
    {
        if (!ChatEnabled || Level < level) return;

        message = $"[TerrariaGearQualityCalculator] {message}";
        Main.NewText(message, r, g, b);
    }
}