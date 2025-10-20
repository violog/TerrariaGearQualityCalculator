using Terraria.GameContent.UI.Elements;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements.Part;

internal class BossPreviewUI : UIPanel
{
    private readonly string _bossData;
    private readonly float _height;

    private readonly bool _isRight;

    public BossPreviewUI(float height, string bossData, bool isRight)
    {
        _height = height;
        _bossData = bossData;
        _isRight = isRight;

        Width.Set(0, 0.48f);
        Height.Set(_height, 0f);
        HAlign = 0f;

        if (_isRight) HAlign = 0.95f;

        LoadElements();
    }

    private void LoadElements()
    {
        var bossNameUi = new UIText(_bossData);

        bossNameUi.HAlign = 0f;
        bossNameUi.VAlign = 0.5f;

        var bossValueUi = new UIText("0.199");

        bossValueUi.HAlign = 1f;
        bossValueUi.VAlign = 0.5f;

        Append(bossNameUi);
        Append(bossValueUi);
    }
}