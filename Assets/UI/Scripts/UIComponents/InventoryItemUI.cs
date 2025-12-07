using UnityEngine;
using UnityEngine.UIElements;
using System;

public class InventoryItemUI
{
    VisualElement m_Icon;
    public event Action<InventoryItemUI> ItemClicked;
    
    public VisualElement Icon => m_Icon;
    public void SetVisualElements(TemplateContainer itemElement)
    {
        if (itemElement == null)
            return;

        m_Icon = itemElement.Q("inventory-item__icon");
    }

    public void SetVisual(TemplateContainer gearElement, ItemVisualData visualData)
    {
        if (gearElement == null)
            return;

        //StyleBackground は不変（Immutable）であり、画像差し替えが起こると、新しく作り直す必要がある
        m_Icon.style.backgroundImage = new StyleBackground(visualData.Icon);
    }

    public void RegisterButtonCallbacks()
    {
        m_Icon.RegisterCallback<ClickEvent>(ClickItem);
    }

    void ClickItem(ClickEvent evt)
    {
        ItemClicked?.Invoke(this);
    }
}
