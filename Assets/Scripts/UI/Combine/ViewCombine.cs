using System.Collections.Generic;
using UI.ModelView.Models;
using UnityEngine;

namespace CardGame.UI.Combine
{
    public class ViewCombine : MonoBehaviour
    {
        [SerializeField] private ModelViewCard _cardPrefab;
        [SerializeField] private CombineComponent _combineComponent;
        [SerializeField] private CombineController _combineController;
        [SerializeField] private float _scale = 1f;
        [SerializeField] private float _selectedOffsetY = 40f;

        private readonly Dictionary<GameObject, RectTransform> _cardUIs = new();

        private void OnEnable()
        {
            if (_combineController != null)
            {
                _combineController.OnSelectionChanged += OnSelectionChanged;
            }

            if (_combineComponent != null)
            {
                _combineComponent.OnPopulated += Refresh;
                Refresh();
            }
        }

        private void OnDisable()
        {
            if (_combineController != null)
            {
                _combineController.OnSelectionChanged -= OnSelectionChanged;
            }

            if (_combineComponent != null)
            {
                _combineComponent.OnPopulated -= Refresh;
            }
        }

        private void Refresh()
        {
            ClearCards();
            if (_combineComponent == null)
            {
                return;
            }

            foreach (var card in _combineComponent.Cards)
            {
                var instance = Instantiate(_cardPrefab, transform);
                instance.SetModel(card);
                instance.transform.localScale = Vector3.one * _scale;
                _cardUIs[card] = instance.GetComponent<RectTransform>();
            }
        }

        private void OnSelectionChanged()
        {
            foreach (var (card, rect) in _cardUIs)
            {
                var isSelected = _combineController.IsSelected(card);
                var pos = rect.localPosition;
                rect.localPosition = new Vector2(pos.x, isSelected ? _selectedOffsetY : 0f);
            }
        }

        private void ClearCards()
        {
            _cardUIs.Clear();
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}