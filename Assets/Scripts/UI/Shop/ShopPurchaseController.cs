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
        [SerializeField] private int _requiredSelections = 2;

        private readonly List<GameObject> _selected = new();

        public event Action OnSelectionChanged;

        public IReadOnlyList<GameObject> Selected => _selected;
        public bool CanBuy => _selected.Count == _requiredSelections;
        public bool IsSelected(GameObject card) => _selected.Contains(card);

        private void OnEnable()
        {
            EventBus.Subscribe<BuyCardEvent>(OnBuyCardEvent);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<BuyCardEvent>(OnBuyCardEvent);
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
                Debug.Log($"[Shop] Deselected '{card.name}'. ({_selected.Count}/{_requiredSelections})");
            }
            else
            {
                if (_selected.Count >= _requiredSelections)
                {
                    Debug.Log($"[Shop] Already have {_requiredSelections} selected — deselect one first.");
                    return;
                }

                _selected.Add(card);
                Debug.Log($"[Shop] Selected '{card.name}'. ({_selected.Count}/{_requiredSelections})");
            }

            OnSelectionChanged?.Invoke();
        }

        public void Buy()
        {
            if (!CanBuy)
            {
                Debug.Log($"[Shop] Need {_requiredSelections} cards selected to buy.");
                return;
            }

            foreach (var card in _selected)
            {
                var identity = card.GetComponent<CardIdentity>();
                var cardId = identity != null ? identity.CardId : card.name;
                RunContext.Instance.PlayerCardIds.Add(cardId);
                _shop.RemoveCard(card);
                Debug.Log($"[Shop] Bought '{cardId}'.");
            }

            RunContext.Instance.Save();

            _selected.Clear();
            _stateData.GoToState(new ExploreCombineState());
        }
    }
}