using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements
{
    public class SearchUI : UIElement
    {
        private UITextBox _search;

        private string _text = "Search...";

        public string Text { get; }

        public override void OnInitialize()
        {
            Width.Set(0, 1f);
            Height.Set(40f, 0f);

            _search = new UITextBox(_text, 1f, false)
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
                TextHAlign = 0f
            };

            _search.OnLeftClick += (evt, element) =>
            {
                Main.clrInput();
                PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                Main.blockInput = true;
            };

            _search.OnRightClick += (evt, element) =>
            {
                Main.clrInput();
                PlayerInput.WritingText = false;
                Main.blockInput = false;
            };

            Append(_search);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Propagate update to child elements.

            if (PlayerInput.WritingText)
            {
                _search.SetText(Main.GetInputText(_search.Text));
            }

            if (_search.Text != _text)
            {
                _text = _search.Text;

                Recalculate();
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}
    