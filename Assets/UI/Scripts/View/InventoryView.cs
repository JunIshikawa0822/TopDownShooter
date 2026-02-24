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
    public class InventoryView : AContainerGridProvider
    {
        private ScrollView _scrollViewParent;
        private VisualElement _scrollContentContainer;

        private VisualTreeAsset _cellTemplate;
        private VisualTreeAsset _containerTemplate;
        private VisualTreeAsset _itemTemplate;

        protected override void SetVisualElements()
        {
            _scrollViewParent = _rootElement.Q<ScrollView>("container__scrollview");
            _scrollContentContainer = _scrollViewParent.Q<VisualElement>("unity-content-container");

            // //グリッドセルのロード
            // _cellTemplate = Resources.Load<VisualTreeAsset>("GridBlockCell");
            // //グリッドブロックのロード
            // _gridBlockTemplate = Resources.Load<VisualTreeAsset>("GridBlock");
            // //コンテナのロード
            // _containerTemplate = Resources.Load<VisualTreeAsset>("Container");
            // //アイテムUIのロード
            _itemTemplate = Resources.Load<VisualTreeAsset>("InventoryItem");
        }

        protected override void RegisterButtonCallbacks()
        {

        }

        //ContainerのUIに関する初期化のクラス
        public override void AddContainerComponent(VisualElement containerElementRoot, GridBlockData[] gridBlockDatas, Guid containerGuid)
        {
            base.AddContainerComponent(containerElementRoot, gridBlockDatas, containerGuid);
            _scrollContentContainer.Add(containerElementRoot);
        }
    }
}