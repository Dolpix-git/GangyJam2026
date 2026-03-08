using System.Collections.Generic;
using CardGame.UI.ModelViewPattern;
using CardGame.UI.Shop;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewShop : ViewBase<ModelViewShop, ShopComponent>
    {
        [SerializeField] private ModelViewCard _cardPrefab;
        [SerializeField] private ShopPurchaseController _purchaseController;
        [SerializeField] private float _scale;
        [SerializeField] private float _selectedOffsetY = 40f;

        private readonly Dictionary<GameObject, RectTransform> _cardUIs = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_purchaseController != null)
            {
                _purchaseController.OnSelectionChanged += OnSelectionChanged;
            }

            if (Model != null)
            {
                Refresh();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Model != null)
            {
                Model.OnPopulated -= Refresh;
            }

            if (_purchaseController != null)
            {
                _purchaseController.OnSelectionChanged -= OnSelectionChanged;
            }
        }

        protected override void HandleModelChanged(ShopComponent model)
        {
            ClearCards();
            if (model == null)
            {
                return;
            }

            model.OnPopulated += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            ClearCards();
            foreach (var card in Model.Cards)
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
                var isSelected = _purchaseController.IsSelected(card);
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