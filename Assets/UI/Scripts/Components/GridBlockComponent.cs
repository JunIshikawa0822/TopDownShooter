using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    // Manages a single Gear Item UI component on the Inventory Screen
    public class GridBlockComponent
    {
        VisualElement gridBlock;

        public void SetVisualElements(TemplateContainer gearElement)
        {
            if (gearElement == null)
                return;

            gridBlock = gearElement.Q("inventory-grid-block");
        }
    }
}