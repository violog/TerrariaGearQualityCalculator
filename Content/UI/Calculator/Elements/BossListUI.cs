using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TerrariaGearQualityCalculator.Content.UI.Calculator.Elements.Part;
using TerrariaGearQualityCalculator.Tools.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements
{
    internal class BossListUI : UIPanel
    {
        private UIList _bossListUi;

        private UIScrollbar _scrollbarUi;

        private const float _heightRow = 30f;
        
        private List<string> _bossData = new List<string>()
        {
            "King Slime",
            "Eye of Cthulhu",
            "Eater of Worlds",
            "Brain of Cthulhu",
            "Queen Bee",
            "Skeletron",
            "Deerclops",
            "Wall of Flesh",
            "The Twins",
            "The Destroyer",
            "Skeletron Prime",
            "Plantera",
            "Golem",
            "Duke Fishron",
            "Lunatic Cultist",
            "Moon Lord"
        };

        private List<string> _bossList;

        public override void OnInitialize()
        {
            _bossListUi = new UIList()
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
            };

            _scrollbarUi = new UIScrollbar();

            _scrollbarUi.Height.Set(0f, 1f);
            _scrollbarUi.Width.Set(20f, 0f);
            _scrollbarUi.Left.Set(-10f, 1f);

            _bossListUi.SetScrollbar(_scrollbarUi);

            UpdateBossListUi();

            Append(_bossListUi);
            Append(_scrollbarUi);
        }

        public void UpdateBossListUi(string search = null)
        {
            _bossList = GetBossList(search);

            LoadBosses();

            _scrollbarUi.SetView(Height.Pixels, _bossListUi.GetTotalHeight());

            _scrollbarUi.Recalculate();
        }

        private void LoadBosses()
        {
            _bossListUi.Clear();

            bool isRight = false;

            int countElementsInRow = 0;

            UIPanel row = Grid.CreateRow(_heightRow);

            // Create a row for every 2 elements
            foreach (var boss in _bossList)
            {
                UIPanel bossPreviewUI = new BossPreviewUI(_heightRow, boss, isRight);

                row.Append(bossPreviewUI);

                countElementsInRow++;

                isRight = !isRight;

                if (countElementsInRow == 2)
                {
                    _bossListUi.Add(row);

                    countElementsInRow = 0;
                    row = Grid.CreateRow(_heightRow);
                }
            }
            
            _bossListUi.UpdateOrder();
            _bossListUi.Recalculate();
        }

        private List<string> GetBossList(string search = null)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return _bossData
                    .Where(word => word.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            
            return _bossData;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _bossListUi.Update(gameTime);
            _scrollbarUi.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}