using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _cardVisual;

    private Canvas _canvas;
    private Transform _originalParent;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        EventBus.Publish(new CardBeginDragEvent(this));
        
        _originalParent = _cardVisual.parent;
        _cardVisual.SetParent(_canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardVisual.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
    
    private CardDropSlot GetCardDropSlotUnderPointer(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject)
            {
                continue;
            }

            var slot = result.gameObject.GetComponentInParent<CardDropSlot>();

            if (slot != null)
            {
                return slot;
            }
        }

        return null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var dropSlot = GetCardDropSlotUnderPointer(eventData);

        if (dropSlot != null)
        {
            EventBus.Publish(new CardDropOnSlotEvent(dropSlot));
        }
        
        _cardVisual.SetParent(_originalParent);
        _cardVisual.anchoredPosition = Vector2.zero;
    }
}
