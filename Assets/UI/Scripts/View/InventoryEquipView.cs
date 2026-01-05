using Game.Data;
using Game.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class InventoryEquipView : AUIView
    {
        private InventoryEquipUIEvents _equipEvents;
        private VisualElement[] _slots;

        public InventoryEquipView(VisualElement root, InventoryEquipUIEvents equipEvents) : base(root)
        {
            _equipEvents = equipEvents;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            _slots = new VisualElement[]
            {
                _topElement.Q<VisualElement>("equip__slot-1"),
                _topElement.Q<VisualElement>("equip__slot-2"),
                _topElement.Q<VisualElement>("equip__slot-3"),
                _topElement.Q<VisualElement>("equip__slot-4"),
                _topElement.Q<VisualElement>("equip__slot-5"),
                _topElement.Q<VisualElement>("equip__slot-6"),
                _topElement.Q<VisualElement>("equip__slot-7"),
                _topElement.Q<VisualElement>("equip__slot-8"),
                _topElement.Q<VisualElement>("equip__slot-9"),
                _topElement.Q<VisualElement>("equip__slot-10")
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
            ((VisualElement)evt.currentTarget).style.backgroundColor = new StyleColor(new Color32(255, 0, 0, 100));
        }

        private void OnPointerLeave(PointerLeaveEvent evt)
        {
            ((VisualElement)evt.currentTarget).style.backgroundColor = new StyleColor(new Color32(255, 255, 255, 100));
        }

    }
}