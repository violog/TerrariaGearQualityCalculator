using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator
{
    public class CalculatorUISystem : ModSystem
    {
        internal UserInterface CalculatorInterface;

        internal CalculatorUI CalculatorUi;

        private GameTime _lastUpdateUiGameTime;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                CalculatorInterface = new UserInterface();

                CalculatorUi = new CalculatorUI();

                CalculatorUi.Activate();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;

            if (CalculatorInterface?.CurrentState != null)
            {
                CalculatorInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrariaGearQualityCalculator: TerrariaGearQualityCalculatorInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && CalculatorInterface?.CurrentState != null)
                        {
                            CalculatorInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public void ChangeDisplayUI(bool isOpen = false)
        {
            if (isOpen)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }

        public void ShowUI()
        {
            CalculatorInterface?.SetState(CalculatorUi);
        }

        public void HideUI()
        {
            CalculatorInterface?.SetState(null);
        }

        public override void Unload()
        {
     
        }
    }
}
