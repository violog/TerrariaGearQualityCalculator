using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;

namespace TerrariaGearQualityCalculator.Tools.UI;

internal class Grid
{
    public static UIPanel CreateRow(float heightRow)
    {
        var row = new UIPanel();

        row.Width.Set(0, 1f);
        row.Height.Set(heightRow, 0f);
        row.SetPadding(0);
        row.BorderColor = Color.Transparent;

        return row;
    }
}