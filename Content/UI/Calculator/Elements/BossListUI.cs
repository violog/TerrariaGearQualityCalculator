using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using TerrariaGearQualityCalculator.Calculators;
using TerrariaGearQualityCalculator.Content.UI.Calculator.Elements.Part;
using TerrariaGearQualityCalculator.Tools.UI;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator.Elements;

internal class BossListUI : UIPanel
{
    private List<ICalculationModel> _bossList;
    private UIList _bossListUi;

    private UIScrollbar _scrollbarUi;

    public override void OnInitialize()
    {
        _bossListUi = new UIList
        {
            Width = StyleDimension.Fill,
            Height = StyleDimension.Fill
        };

        _scrollbarUi = new UIScrollbar();

        _scrollbarUi.Height.Set(0f, 1f);
        _scrollbarUi.Width.Set(20f, 0f);
        _scrollbarUi.Left.Set(-10f, 1f);

        _bossListUi.SetScrollbar(_scrollbarUi);

        Append(_bossListUi);
        Append(_scrollbarUi);
    }

    // Must run after OnInitialize to avoid NullReferenceException
    public override void OnActivate() => Update();

    internal void Update(string search = null)
    {
        _bossList = FilterList(search);

        LoadBosses();

        _scrollbarUi.SetView(Height.Pixels, _bossListUi.GetTotalHeight());

        _scrollbarUi.Recalculate();
    }

    private void LoadBosses()
    {
        _bossListUi.Clear();
        var isRight = false;
        var countElementsInRow = 0;
        var row = Grid.CreateRow();

        // Create a row for every 2 elements
        foreach (var boss in _bossList)
        {
            UIPanel bossPreviewUI = new BossPreviewUI(boss.Name, boss.Sr, isRight);

            row.Append(bossPreviewUI);

            countElementsInRow++;

            isRight = !isRight;

            if (countElementsInRow == 2)
            {
                _bossListUi.Add(row);

                countElementsInRow = 0;
                row = Grid.CreateRow();
            }
        }

        if (countElementsInRow > 0) _bossListUi.Add(row);

        _bossListUi.UpdateOrder();
        _bossListUi.Recalculate();
    }

    private static List<ICalculationModel> FilterList(string search = null)
    {
        TGQC.Log.Info(
            $"Contains {TGQC.Storage.BossList.Count} bosses, search: {search}. Values:");
        foreach (var boss in TGQC.Storage.BossList)
        {
            TGQC.Log.Debug(boss.ToString());
        }

        if (!string.IsNullOrEmpty(search))
        {
            return TGQC.Storage.BossList
                .Where(boss => boss != null && boss.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return TGQC.Storage.BossList.ToList();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _bossListUi.Update(gameTime);
        _scrollbarUi.Update(gameTime);
    }
}