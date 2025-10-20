using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements;

internal class SearchUI : UIElement
{
    private bool _isFocused;
    private UITextBox _searchUi;

    private string OldText = "";

    public string Text => _searchUi.Text;

    public string Placeholder { get; } = "Search...";

    public override void OnInitialize()
    {
        _searchUi = new UITextBox(Placeholder)
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            TextHAlign = 0f
        };

        _searchUi.SetTextMaxLength(50);

        _searchUi.OnLeftClick += (evt, element) =>
        {
            if (_searchUi.Text == Placeholder) _searchUi.SetText("");

            Focus();
        };

        Append(_searchUi);
    }

    public bool NeedSearch()
    {
        if ((_searchUi.Text == Placeholder && !_isFocused) || OldText.Equals(_searchUi.Text)) return false;

        return true;
    }

    public void ResetSearch()
    {
        OldText = _searchUi.Text;
    }

    private void Focus()
    {
        if (_isFocused) return;

        Main.clrInput();
        _isFocused = true;
        Main.blockInput = true;
    }

    private void Unfocus()
    {
        if (!_isFocused) return;

        _isFocused = false;
        Main.blockInput = false;
    }

    private static bool JustPressed(Keys key)
    {
        return Main.inputText.IsKeyDown(key) && !Main.oldInputText.IsKeyDown(key);
    }

    public override void Update(GameTime gameTime)
    {
        var MousePosition = new Vector2(Main.mouseX, Main.mouseY);

        if (!ContainsPoint(MousePosition) && (Main.mouseLeft || Main.mouseRight))
        {
            if (string.IsNullOrEmpty(_searchUi.Text)) _searchUi.SetText(Placeholder);

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

            var newText = Main.GetInputText(_searchUi.Text);

            if (!newText.Equals(_searchUi.Text)) _searchUi.SetText(newText);

            if (JustPressed(Keys.Enter) || JustPressed(Keys.Escape))
            {
                Main.drawingPlayerChat = false;

                Unfocus();
            }
        }
    }
}