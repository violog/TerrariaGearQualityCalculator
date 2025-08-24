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
        private UIPanel _form;

        private int _formPadding = 10;

        private int _formWidth = 600;   

        private int _formHeight = 300;

        private SearchUI search;
        
        public override void OnInitialize()
        {
            CreateForm();

            LoadElements();
        }

        public void CreateForm()
        {
            _form = new UIPanel();

            _form.Width.Set(_formWidth, 0);
            _form.Height.Set(_formHeight, 0);
            _form.SetPadding(_formPadding);
            _form.VAlign = 0.5f;
            _form.HAlign = 0.5f;

            Append(_form);
        }

        public void LoadElements()
        {
            LoadSearch();

            LoadBody();
        }

        public void LoadSearch()
        {
            search = new SearchUI();

            _form.Append(search);
        }

        public UIText myTextDisplay;

        public void LoadBody()
        {
            UIPanel body = new UIPanel();

            body.Top.Set(50, 0);
            body.Width = StyleDimension.Fill;
            body.Height = StyleDimension.FromPixels(_formHeight - 50 - 2* _formPadding);

            myTextDisplay = new UIText("Текст: ");
            myTextDisplay.Top.Set(60, 0);
            myTextDisplay.HAlign = 0.5f;
            myTextDisplay.Left.Set(10, 0);
            body.Append(myTextDisplay);

            _form.Append(body);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // myTextDisplay.SetText($"Текст: {search.Text}");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            // Preventing mouse clicks from using selected item
            if (_form.ContainsPoint(Main.MouseScreen))
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
