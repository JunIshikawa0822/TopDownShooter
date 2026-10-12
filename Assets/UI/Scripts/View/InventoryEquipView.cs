using Game.Data;
using Game.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class InventoryEquipView : AUIView
    {
        private VisualElement[] _slots;

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            _slots = new VisualElement[]
            {
                _rootElement.Q<VisualElement>("equip__slot-1"),
                _rootElement.Q<VisualElement>("equip__slot-2"),
                _rootElement.Q<VisualElement>("equip__slot-3"),
                _rootElement.Q<VisualElement>("equip__slot-4"),
                _rootElement.Q<VisualElement>("equip__slot-5"),
                _rootElement.Q<VisualElement>("equip__slot-6"),
                _rootElement.Q<VisualElement>("equip__slot-7"),
                _rootElement.Q<VisualElement>("equip__slot-8"),
                _rootElement.Q<VisualElement>("equip__slot-9"),
                _rootElement.Q<VisualElement>("equip__slot-10")
            };
        }

        protected override void RegisterButtonCallbacks()
        {
            for(int i = 0; i < _slots.Length; ++i)
            {
                _slots[i].RegisterCallback<PointerEnterEvent>(OnPointerEnter);
                _slots[i].RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
            }
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            
        }

    }
}