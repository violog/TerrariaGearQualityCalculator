using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements
{
    public class SearchUI : UIPanel
    {
        private UISearchBar _searchBar;
        private UIImageButton uIImageButton2;
        
        // TODO: this should do an actual search and perform filtering
        private void OnSearchContentsChanged(string contents)
        {
        }
        
        private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target.Parent != this)
            {
                // this was the method you were looking for :)
                _searchBar.ToggleTakingText();
            }
        }

        public override void OnInitialize()
        {
            Width.Set(0, 1f);
            Height.Set(40f, 0f);
            
            _searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
            {
                Width = new StyleDimension(0f, 0.5f),
                Height = new StyleDimension(0f, 0.5f),
                HAlign = 0f,
                VAlign = 0.5f,
                Left = new StyleDimension(0f, 0f),
                IgnoresMouseInteraction = true
            };
            OnLeftClick += Click_SearchArea;
            Append(_searchBar);
            
            _searchBar.OnContentsChanged += OnSearchContentsChanged;
            _searchBar.OnStartTakingInput += OnStartTakingInput;
            _searchBar.OnEndTakingInput += OnEndTakingInput;
            _searchBar.OnCanceledTakingInput += OnCanceledInput;
            
            uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", (AssetRequestMode)1))
            {
                HAlign = 1f,
                VAlign = 0.5f,
                Left = new StyleDimension(-2f, 0f)
            };
            uIImageButton2.OnMouseOver += searchCancelButton_OnMouseOver;
            uIImageButton2.OnLeftClick += searchCancelButton_OnClick;
            Append(uIImageButton2);
            
            _searchBar.SetContents(null, forced: true);
        }
        
        // TODO: Play Terraria sounds on hover and click, method signature is somewhy changed
        private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_searchBar.HasContents)
            {
                _searchBar.SetContents(null, forced: true);
                // SoundEngine.PlaySound(11);
            }
            else
            {
                // SoundEngine.PlaySound(12);
            }
        }
        
        private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            // SoundEngine.PlaySound(12);
        }
        
        private void OnStartTakingInput()
        {
            BorderColor = Main.OurFavoriteColor;
        }

        private void OnEndTakingInput()
        {
            BorderColor = new Color(35, 40, 83);
        }
        
        // I assume this is called when you press hotkey to toggle UI, because my inventory was toggling at the same time
        private void OnCanceledInput()
        {
            // Main.LocalPlayer.ToggleInv();
        }
    }
}
    