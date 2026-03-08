using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform _originalParent;
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private GameObject _placeholder;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            CreatePlaceholder();
            transform.SetParent(_canvas.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            UpdatePlaceholderPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var siblingIndex = _placeholder.transform.GetSiblingIndex();
            Destroy(_placeholder);
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(siblingIndex);
        }

        private void CreatePlaceholder()
        {
            _placeholder = new GameObject("Placeholder");
            _placeholder.transform.SetParent(_originalParent);
            var newRect = _placeholder.AddComponent<RectTransform>();
            newRect.sizeDelta = _rectTransform.sizeDelta;

            _placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        private void UpdatePlaceholderPosition()
        {
            var draggedItemScreenPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);

            var newSiblingIndex = _originalParent.childCount;

            for (var i = 0; i < _originalParent.childCount; i++)
            {
                var child = _originalParent.GetChild(i);

                if (child == _placeholder.transform)
                {
                    continue;
                }

                var childScreenPosition = RectTransformUtility.WorldToScreenPoint(null, child.position);

                if (!(draggedItemScreenPosition.x < childScreenPosition.x))
                {
                    continue;
                }

                newSiblingIndex = i;

                if (_placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }

            _placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }
    }
