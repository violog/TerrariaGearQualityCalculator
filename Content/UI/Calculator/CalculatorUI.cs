using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using TerrariaGearQualityCalculator.Content.UI.Calculator.Elements;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator
{
    public class CalculatorUI : UIState
    {
        private UIPanel _formUi;

        private int _formPadding = 10;

        private int _formWidth = 600;   

        private int _formHeight = 300;

        private SearchUI _searchUi;

        private BossListUI _bossListUi;

        public override void OnInitialize()
        {
            CreateForm();

            LoadElements();
        }

        public void CreateForm()
        {
            _formUi = new UIPanel();

            _formUi.Width.Set(_formWidth, 0);
            _formUi.Height.Set(_formHeight, 0);
            _formUi.SetPadding(_formPadding);
            _formUi.VAlign = 0.5f;
            _formUi.HAlign = 0.5f;

            Append(_formUi);
        }

        public void LoadElements()
        {
            LoadSearch();

            LoadBossList();
        }

        public void LoadSearch()
        {
            _searchUi = new SearchUI();

            _searchUi.Width.Set(0, 1f);
            _searchUi.Height.Set(40f, 0f);

            _formUi.Append(_searchUi);
        }

        public void LoadBossList()
        {
            _bossListUi = new BossListUI();

            _bossListUi.Top.Set(50, 0);
            _bossListUi.Width = StyleDimension.Fill;
            _bossListUi.Height = StyleDimension.FromPixels(_formHeight - 50 - 2*_formPadding);

            _formUi.Append(_bossListUi);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // Preventing mouse clicks from using selected item
            if (_formUi.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            // Preventing scroll wheel from shifting selected Hotbar item
            //if (list.IsMouseHovering)
            //{
            //    PlayerInput.LockVanillaMouseScroll("MyMod/ScrollListB");
            //}
        }
    }
}
