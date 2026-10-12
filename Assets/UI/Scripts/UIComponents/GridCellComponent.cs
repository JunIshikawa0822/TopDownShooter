using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class GridCellComponent
    {
        private VisualElement _cellRoot;
        public event Action OnPointerEnterEvent;
        public event Action OnPointerLeaveEvent;
        public void SetVisualElements(VisualElement cellVisual)
        {
            if (cellVisual == null)return;
            _cellRoot = cellVisual.Q("gridcell__root");
        }
        public void RegisterPointerCallbacks()
        {
            _cellRoot.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            _cellRoot.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        public void UnregisterPointerCallbacks()
        {
            _cellRoot.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            _cellRoot.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }
        public void SetColor(Color color)
        {
            _cellRoot.style.backgroundColor = new StyleColor(color);
        }

        public void SetImage(Sprite sprite)
        {
            _cellRoot.style.backgroundImage = new StyleBackground(sprite);
        }

        public void OnPointerEnter(PointerEnterEvent evt)
        {
            OnPointerEnterEvent?.Invoke();
        }

        public void OnPointerLeave(PointerLeaveEvent evt)
        {
            OnPointerLeaveEvent?.Invoke();
        }
    }
}