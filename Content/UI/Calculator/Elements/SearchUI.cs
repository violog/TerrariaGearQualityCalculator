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
    public class SearchUI : UIElement
    {
        private UITextBox _search;

        private bool _isFocused = false;

        private readonly string _defaultText = "Search...";

        public string Text => _search.Text;

        public string DefaultText => _defaultText;

        public override void OnInitialize()
        {
            Width.Set(0, 1f);
            Height.Set(40f, 0f);

            _search = new UITextBox(_defaultText, 1f, false)
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
                TextHAlign = 0f
            };

            _search.SetTextMaxLength(50);

            _search.OnLeftClick += (evt, element) =>
            {
                if (_search.Text == _defaultText)
                {
                    _search.SetText("");
                }

                Focus();
            };

            Append(_search);
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
                if (string.IsNullOrEmpty(_search.Text))
                {
                    _search.SetText(_defaultText);
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

                string newText = Main.GetInputText(_search.Text);

                if (!newText.Equals(_search.Text))
                {
                    _search.SetText(newText);
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
