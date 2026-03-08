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
        _originalParent = _cardVisual.parent;
        _cardVisual.SetParent(_canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardVisual.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cardVisual.SetParent(_originalParent);
        _cardVisual.anchoredPosition = Vector2.zero;
    }
}
