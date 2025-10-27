using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;

namespace TerrariaGearQualityCalculator.Tools.UI;

internal class Grid
{
    internal const float RowHeight = 30f;

    public static UIPanel CreateRow(float heightRow = RowHeight)
    {
        var row = new UIPanel();

        row.Width.Set(0, 1f);
        row.Height.Set(heightRow, 0f);
        row.SetPadding(0);
        row.BorderColor = Color.Transparent;

        return row;
    }
}