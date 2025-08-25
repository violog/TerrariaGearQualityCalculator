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
        public UIPanel Form => _form;

        private int _formPadding = 10;

        private int _formWidth = 600;   

        private int _formHeight = 300;

        // private SearchUI search;
        private UISearchBar _searchBar;
        private UIPanel _searchBoxPanel;
        public event Action OnStartTakingInput;
        public event Action OnEndTakingInput;
        public event Action OnCanceledTakingInput;
        
        public void AddSearchBar(UIElement searchArea)
        {
            UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search", (AssetRequestMode)1))
            {
                VAlign = 0.5f,
                HAlign = 0f
            };
            uIImageButton.OnLeftClick += Click_SearchArea;
            uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border", (AssetRequestMode)1));
            uIImageButton.SetVisibility(1f, 1f);
            uIImageButton.SetSnapPoint("CreativeInfinitesSearch", 0);
            searchArea.Append(uIImageButton);
            UIPanel uIPanel = (_searchBoxPanel = new UIPanel
            {
                Width = new StyleDimension(0f - uIImageButton.Width.Pixels - 3f, 1f),
                Height = new StyleDimension(0f, 1f),
                VAlign = 0.5f,
                HAlign = 1f
            });
            uIPanel.BackgroundColor = new Color(35, 40, 83);
            uIPanel.BorderColor = new Color(35, 40, 83);
            uIPanel.SetPadding(0f);
            searchArea.Append(uIPanel);
            UISearchBar uISearchBar = (_searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                HAlign = 0f,
                VAlign = 0.5f,
                Left = new StyleDimension(0f, 0f),
                IgnoresMouseInteraction = true
            });
            uIPanel.OnLeftClick += Click_SearchArea;
            uISearchBar.OnContentsChanged += OnSearchContentsChanged;
            uIPanel.Append(uISearchBar);
            uISearchBar.OnStartTakingInput += OnStartTakingInput;
            uISearchBar.OnEndTakingInput += OnEndTakingInput;
            uISearchBar.OnCanceledTakingInput += OnCanceledTakingInput;
            UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", (AssetRequestMode)1))
            {
                HAlign = 1f,
                VAlign = 0.5f,
                Left = new StyleDimension(-2f, 0f)
            };
            // uIImageButton2.OnMouseOver += searchCancelButton_OnMouseOver;
            // uIImageButton2.OnLeftClick += searchCancelButton_OnClick;
            uIPanel.Append(uIImageButton2);
        }
        
        private string _searchString;
        
        private void OnSearchContentsChanged(string contents)
        {
            _searchString = contents;
        }
        
        private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target.Parent != _searchBoxPanel)
            {
                _searchBar.ToggleTakingText();
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            base.LeftClick(evt);
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
        }


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
            // AddSearchBar(_searchBoxPanel);

            LoadBody();
        }

        public void LoadSearch()
        {
            // search = new SearchUI();

            // _form.Append(search);
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
