using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements.Part
{
    internal class BossPreviewUI : UIPanel
    {
        private readonly float _height;

        private readonly string _bossData;

        private readonly bool _isRight = false;

        public BossPreviewUI(float height, string bossData, bool isRight)
        {
            _height = height;
            _bossData = bossData;
            _isRight = isRight;
        }

        public override void OnInitialize()
        {
            Width.Set(0, 0.48f);
            Height.Set(_height, 0f);
            HAlign = 0f;

            if (_isRight)
            {
                HAlign = 0.95f;
            }

            LoadElements();
        }

        private void LoadElements()
        {
            UIText bossNameUi = new UIText(_bossData, 1f);

            bossNameUi.HAlign = 0f;
            bossNameUi.VAlign = 0.5f;

            UIText bossValueUi = new UIText("0.199", 1f);

            bossValueUi.HAlign = 1f;
            bossValueUi.VAlign = 0.5f;

            Append(bossNameUi);
            Append(bossValueUi);
        }
    }
}
