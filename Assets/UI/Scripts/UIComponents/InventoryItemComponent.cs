using UnityEngine;
using UnityEngine.UIElements;
using System;

//インベントリUIの基底クラス
//アイテムの種類によって説明や必要なパラメータは変わるので
public class InventoryItemComponent
{
    private VisualElement _icon;
    private Guid _itemDataGuid;

    // --- 通知イベント ---
    public event Action OnPointerEnterEvent;
    public event Action OnPointerLeaveEvent;
    public event Action OnPointerDownEvent;
    public event Action OnPointerUpEvent;
    public event Action OnDragStartEvent;
    public event Action OnDraggingEvent;
    public event Action OnDragEndEvent;

    public VisualElement Icon => _icon;
    public Guid ItemDataGuid => _itemDataGuid;

    //内部状態
    private bool _isPointerDown = false;
    private bool _isDragging = false;
    private Vector2 _pointerDownPos;
    private const float DragThreshold = 5f; //px

    public InventoryItemComponent(TemplateContainer inventoryItemElement, Guid itemDataGuid)
    {
        if (inventoryItemElement == null)return;
        if(_itemDataGuid == itemDataGuid) return;

        _itemDataGuid = itemDataGuid;
        _icon = inventoryItemElement.Q("inventoryitem__container");
    }
    public virtual void SetVisualData(Sprite icon, int width, int height, int cellSize)
    {
        _icon.style.backgroundImage = new StyleBackground(icon);
        _icon.style.width  = width * cellSize;
        _icon.style.height = height * cellSize;
    }

    public void RegisterPointerCallbacks()
    {
        _icon.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
        _icon.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        _icon.RegisterCallback<PointerDownEvent>(OnPointerDown);
        _icon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        _icon.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    void OnPointerEnter(PointerEnterEvent evt)
    {
        OnPointerEnterEvent?.Invoke();
    }

    void OnPointerLeave(PointerLeaveEvent evt)
    {
        OnPointerLeaveEvent?.Invoke();
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        _isPointerDown = true;
        _isDragging = false;
        _pointerDownPos = evt.localPosition;

        OnPointerDownEvent?.Invoke();
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (!_isPointerDown)
            return;

        Vector2 currentPos = evt.localPosition;
        Vector2 delta = currentPos - _pointerDownPos;

        // ドラッグ開始判定
        // ドラッグし始めた場所からの差分があるときにドラッグしたとみなす
        if (!_isDragging && delta.magnitude > DragThreshold)
        {
            _isDragging = true;
            OnDragStartEvent?.Invoke();
        }

        // ドラッグ中
        if (_isDragging)
        {
            OnDraggingEvent?.Invoke();
        }
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        // ドラッグ終了
        if (_isDragging)
        {
            OnDragEndEvent?.Invoke();
        }
        else
        {
            OnPointerUpEvent?.Invoke();
        }

        _isPointerDown = false;
        _isDragging = false;
    }
}
