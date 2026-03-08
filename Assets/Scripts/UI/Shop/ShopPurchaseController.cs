using System;
using System.Collections.Generic;
using CardGame.Data;
using CardGame.Run;
using CardGame.StateMachine.Explore;
using CardGame.StateMachine.Explore.States;
using Events;
using UnityEngine;

namespace CardGame.UI.Shop
{
    public class ShopPurchaseController : MonoBehaviour
    {
        [SerializeField] private ShopComponent _shop;
        [SerializeField] private ExploreStateData _stateData;

        private readonly List<GameObject> _selected = new();

        public IReadOnlyList<GameObject> Selected => _selected;

        private void OnEnable()
        {
            EventBus.Subscribe<BuyCardEvent>(OnBuyCardEvent);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<BuyCardEvent>(OnBuyCardEvent);
        }

        public event Action OnSelectionChanged;

        public bool IsSelected(GameObject card)
        {
            return _selected.Contains(card);
        }

        public bool CanAffordSelected()
        {
            var total = 0;
            foreach (var card in _selected)
            {
                var cost = card.GetComponent<CostData>();
                total += cost != null ? cost.Cost : 0;
            }

            return RunContext.Instance.Coins >= total;
        }

        private void OnBuyCardEvent(BuyCardEvent evt)
        {
            SelectCard(evt.BoughtCard);
        }

        public void SelectCard(GameObject card)
        {
            if (_selected.Contains(card))
            {
                _selected.Remove(card);
                Debug.Log($"[Shop] Deselected '{card.name}'.");
            }
            else
            {
                _selected.Add(card);
                Debug.Log($"[Shop] Selected '{card.name}'.");
            }

            OnSelectionChanged?.Invoke();
        }

        public void Buy()
        {
            if (_selected.Count == 0)
            {
                Debug.Log("[Shop] Nothing selected.");
                return;
            }

            if (!CanAffordSelected())
            {
                Debug.Log($"[Shop] Not enough coins. Have {RunContext.Instance.Coins}.");
                return;
            }

            foreach (var card in _selected)
            {
                var identity = card.GetComponent<CardIdentity>();
                var cost = card.GetComponent<CostData>();
                var cardId = identity != null ? identity.CardId : card.name;
                var price = cost != null ? cost.Cost : 0;

                RunContext.Instance.TrySpendCoins(price);
                RunContext.Instance.PlayerCardIds.Add(cardId);
                _shop.RemoveCard(card);
                Debug.Log($"[Shop] Bought '{cardId}' for {price} coins. Remaining: {RunContext.Instance.Coins}");
            }

            RunContext.Instance.Save();
            _selected.Clear();
            _stateData.GoToState(new ExploreCombineState());
        }

        public void Skip()
        {
            _selected.Clear();
            _stateData.GoToState(new ExploreCombineState());
        }
    }
}