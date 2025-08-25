using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements
{
    internal class SearchUI : UIElement
    {
        private UITextBox _searchUi;

        private bool _isFocused = false;

        private string _placeholder = "Search...";

        public string Text => _searchUi.Text;

        public string Placeholder => _placeholder;

        public override void OnInitialize()
        {
            _searchUi = new UITextBox(_placeholder, 1f, false)
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
                TextHAlign = 0f
            };

            _searchUi.SetTextMaxLength(50);

            _searchUi.OnLeftClick += (evt, element) =>
            {
                if (_searchUi.Text == _placeholder)
                {
                    _searchUi.SetText("");
                }

                Focus();
            };

            Append(_searchUi);
        }

        public void Focus()
        {
            if (!_isFocused)
            {
                Main.clrInput();
                _isFocused = true;
                Main.blockInput = true;
            }
        }

        public void Unfocus()
        {
            if (_isFocused)
            {
                _isFocused = false;
                Main.blockInput = false;
            }
        }

        private static bool JustPressed(Keys key)
        {
            return Main.inputText.IsKeyDown(key) && !Main.oldInputText.IsKeyDown(key);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);

            if (!ContainsPoint(MousePosition) && (Main.mouseLeft || Main.mouseRight))
            {
                if (string.IsNullOrEmpty(_searchUi.Text))
                {
                    _searchUi.SetText(_placeholder);
                }

                Unfocus();
            }

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (_isFocused)
            {
                PlayerInput.WritingText = true;
                Main.instance.HandleIME();

                string newText = Main.GetInputText(_searchUi.Text);

                if (!newText.Equals(_searchUi.Text))
                {
                    _searchUi.SetText(newText);
                }

                if (JustPressed(Keys.Enter) || JustPressed(Keys.Escape))
                {
                    Main.drawingPlayerChat = false;

                    Unfocus();
                }
            }
        }
    }
}
