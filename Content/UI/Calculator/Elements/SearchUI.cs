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
using Terraria.Localization;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements
{
    public class SearchUI : UIElement
    {
        private UISearchBar _search;
        private bool hasFocus { get; set; }

        private string _text = "Search...";

        public string Text { get; private set; }

        public override void OnInitialize()
        {
            Width.Set(0, 1f);
            Height.Set(40f, 0f);

            _search = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 1f)
            {
                Width = StyleDimension.Fill,
                Height = StyleDimension.Fill,
                HAlign = 0f
            };

            _search.OnLeftClick += (evt, element) =>
            {
                Main.blockInput = true;
                hasFocus = true;
                // Main.clrInput();
                // PlayerInput.WritingText = true;
                // Main.instance.HandleIME();
                // string prev = Text;
                // string newString = Main.GetInputText(prev);
                // Main.NewText(newString);
                // Text = newString;
                // _search.SetText(newString);
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
            // base.Update(gameTime); // Propagate update to child elements.
            if (hasFocus) HandleTextInput();

            // if (hasFocus)
            // {
            //     _search.SetText(Main.GetInputText(_search.Text));
            // }
            //
            // if (_search.Text != _text)
            // {
            //     _text = _search.Text;
            //
            //     Recalculate();
            // }
            base.Update(gameTime);
        }
        
        private void HandleTextInput()
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string prev = Text;
            string newString = Main.GetInputText(prev);
            // if (newString != prev)
            // {
                int newStringLength = newString.Length;
                // if (prev != Text)
                    // newString += Text[cursorPosition..];
                Text = newString;
                Main.NewText(newString);
                // _search.SetText(newString);
                // _search.SetText(newString);
            // }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}
    