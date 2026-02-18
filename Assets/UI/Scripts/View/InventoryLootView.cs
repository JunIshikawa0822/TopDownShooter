using System.Collections.Generic;
using Game.Data;
using Game.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;

namespace Game.UI
{
    public class InventoryLootView : AContainerGridProvider
    {
        private ScrollView _scrollViewParent;
        private VisualElement _scrollContentContainer;

        private VisualTreeAsset _cellTemplate;
        private VisualTreeAsset _containerTemplate;
        private VisualTreeAsset _itemTemplate;

        protected override void SetVisualElements()
        {
            _scrollViewParent = _rootElement.Q<ScrollView>("lootcontainer__scrollview");
            _scrollContentContainer = _scrollViewParent.Q<VisualElement>("unity-content-container");

            //グリッドセルのロード
            // _cellTemplate = Resources.Load<VisualTreeAsset>("GridBlockCell");
            //グリッドブロックのロード
            // _gridBlockTemplate = Resources.Load<VisualTreeAsset>("GridBlock");
            //コンテナのロード
            // _containerTemplate = Resources.Load<VisualTreeAsset>("Container");
            //アイテムUIのロード
            _itemTemplate = Resources.Load<VisualTreeAsset>("InventoryItem");
        }

        protected override void RegisterButtonCallbacks()
        {

        }

        private InventoryItemComponent CreateItemComponent(TemplateContainer itemTemplate, ItemVisualData visualData, Guid itemGuid)
        {
            InventoryItemComponent itemComponent = new InventoryItemComponent(itemTemplate, itemGuid);

            //TODO:AddressableでSpriteロードしなきゃ！！
            Sprite sprite = null;
            itemComponent.SetVisualData(sprite, visualData.Width, visualData.Height, _cellSize);
            //TODO: OnPointerEnterの登録
            //TODO: OnPointerLeaveの登録

            return itemComponent;
        }

        public void LoadItemToContainer(Guid containerGuid, int gridBlockIndex, int x, int y, int rotationDeg, InventoryItemComponent itemComponent)
        {
            _containerUIDic[containerGuid].PlaceItem(gridBlockIndex, x, y, rotationDeg, itemComponent);
        }

        public override void AddContainer(VisualElement containerElementRoot, GridBlockData[] gridBlockDatas, Guid containerGuid)
        {
            base.AddContainer(containerElementRoot, gridBlockDatas, containerGuid);
            _scrollContentContainer.Add(containerElementRoot);
        }
    }
}