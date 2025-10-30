using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements;

internal class SearchUI : UIElement
{
    private bool _isFocused;
    private UITextBox _searchUi;
    private string _oldText = "";

    internal string Text => _searchUi == null || _searchUi.Text == Placeholder ? "" : _searchUi.Text;

    private string Placeholder = "Search...";

    public override void OnInitialize()
    {
        _searchUi = new UITextBox(Placeholder)
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill,
            TextHAlign = 0f,
        };

        _searchUi.SetTextMaxLength(50);
        _searchUi.OnLeftClick += (_, _) => { SetFocus(true); };
        Append(_searchUi);
        SetFocus(false);
    }

    public bool NeedSearch()
    {
        return _searchUi.Text != Placeholder && _isFocused && !_oldText.Equals(_searchUi.Text);
    }

    public void ResetSearch()
    {
        _oldText = _searchUi.Text;
    }

    private void SetFocus(bool v)
    {
        if (v)
        {
            Main.clrInput();
            _searchUi.SetText("");
            _searchUi.TextColor = Color.White;
        }
        else
        {
            if (Text == "") _searchUi.SetText(Placeholder);
            _searchUi.TextColor = Color.Gray;
        }

        _isFocused = v;
        _searchUi.ShowInputTicker = v;
        Main.blockInput = v;
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
            SetFocus(false);
        }

        base.Update(gameTime);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        if (!_isFocused) return;

        // must run every tick, just like DrawSelf does
        PlayerInput.WritingText = true;
        Main.instance.HandleIME();
        var newText = Main.GetInputText(_searchUi.Text);

        if (!newText.Equals(_searchUi.Text)) _searchUi.SetText(newText);

        if (JustPressed(Keys.Enter) || JustPressed(Keys.Escape))
        {
            Main.drawingPlayerChat = false;
            SetFocus(false);
        }
    }

    // OnDeactivate is called whenever parent state is changed to null with UI.SetState call
    public override void OnDeactivate()
    {
        SetFocus(false);
    }
}