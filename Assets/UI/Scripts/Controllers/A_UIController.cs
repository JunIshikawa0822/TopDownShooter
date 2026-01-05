using UnityEngine.UIElements;
using UnityEngine;

public abstract class AUIController
{
    protected UIEvents uiEvents;

    protected AUIController(UIEvents uiEvents)
    {
        this.uiEvents = uiEvents;
    }

    public virtual void Open()
    {
        
    }

    public virtual void Close()
    {
        
    }
}
