using Game.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class InventoryView : UIView
    {
        private readonly VisualElement _root;
        private readonly VisualElement _gridContainer;
        private readonly VisualTreeAsset _blockTemplate;

        public InventoryView(VisualElement root) : base(root)
        {
            _root = root;
            _gridContainer = _root.Q<VisualElement>("GridContainer");

            // UXMLテンプレートのロード
            _blockTemplate = Resources.Load<VisualTreeAsset>("GridBlock");
        }

        public void CreateGrid(int gridWidth, int gridHeight, float cellSize, float cellGap)
        {
            _gridContainer.Clear();

            // レイアウト：Flexでグリッド状に並べる
            _gridContainer.style.flexDirection = FlexDirection.Row;
            _gridContainer.style.flexWrap = Wrap.Wrap;
            _gridContainer.style.width = gridWidth * cellSize + (gridWidth - 1) * cellGap;
            _gridContainer.style.height = gridHeight * cellSize + (gridHeight - 1) * cellGap;
            _gridContainer.style.alignContent = Align.FlexStart;

            for (int i = 0; i < gridWidth * gridHeight; i++)
            {
                // ① UXML からテンプレートインスタンスを生成
                var block = _blockTemplate.Instantiate();
                block.name = $"Cell_{i}";
                block.AddToClassList("grid-block"); // USS で調整できるように

                // ② サイズ・マージン補正（USSで上書き可能）
                block.style.width = cellSize;
                block.style.height = cellSize;
                block.style.marginRight = cellGap;
                block.style.marginBottom = cellGap;

                _gridContainer.Add(block);
            }
        }
    }
}