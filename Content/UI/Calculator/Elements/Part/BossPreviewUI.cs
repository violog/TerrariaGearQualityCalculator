using Terraria.GameContent.UI.Elements;
using TerrariaGearQualityCalculator.Tools.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements.Part;

internal class BossPreviewUI : UIPanel
{
    private readonly string _name;
    private readonly string _sr;

    public BossPreviewUI(string name, string sr, bool isRight)
    {
        _name = name;
        _sr = sr;

        Width.Set(0, 0.48f);
        Height.Set(Grid.RowHeight, 0f);
        HAlign = 0f;

        if (isRight) HAlign = 0.95f;

        LoadElements();
    }

    private void LoadElements()
    {
        var name = new UIText(_name);
        name.HAlign = 0f;
        name.VAlign = 0.5f;

        var sr = new UIText(_sr);
        sr.HAlign = 1f;
        sr.VAlign = 0.5f;

        Append(name);
        Append(sr);
    }
}