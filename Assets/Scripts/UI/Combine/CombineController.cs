using System;
using System.Collections.Generic;
using CardGame.Card;
using CardGame.Data;
using CardGame.Run;
using CardGame.StateMachine.Explore;
using CardGame.StateMachine.Explore.States;
using Events;
using UnityEngine;

namespace CardGame.UI.Combine
{
    public class CombineController : MonoBehaviour
    {
        [SerializeField] private ExploreStateData _stateData;
        [SerializeField] private int _requiredSelections = 2;

        private readonly List<GameObject> _selected = new();

        public IReadOnlyList<GameObject> Selected => _selected;
        public bool CanCombine => _selected.Count == _requiredSelections;

        private void OnEnable()
        {
            EventBus.Subscribe<CombineCardEvent>(OnCombineCardEvent);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<CombineCardEvent>(OnCombineCardEvent);
        }

        public event Action OnSelectionChanged;

        public bool IsSelected(GameObject card)
        {
            return _selected.Contains(card);
        }

        private void OnCombineCardEvent(CombineCardEvent evt)
        {
            SelectCard(evt.Card);
        }

        public void SelectCard(GameObject card)
        {
            if (_selected.Contains(card))
            {
                _selected.Remove(card);
            }
            else
            {
                if (_selected.Count >= _requiredSelections)
                {
                    return;
                }

                _selected.Add(card);
            }

            OnSelectionChanged?.Invoke();
        }

        public void Combine()
        {
            if (!CanCombine)
            {
                return;
            }

            var combinedId = CardCombiner.Combine(_selected[0], _selected[1]);
            if (combinedId == null)
            {
                return;
            }

            var idA = _selected[0].GetComponent<CardIdentity>().CardId;
            var idB = _selected[1].GetComponent<CardIdentity>().CardId;
            RunContext.Instance.PlayerCardIds.Remove(idA);
            RunContext.Instance.PlayerCardIds.Remove(idB);
            RunContext.Instance.PlayerCardIds.Add(combinedId);
            RunContext.Instance.Save();

            var parentA = _selected[0];
            var parentB = _selected[1];
            _selected.Clear();
            OnSelectionChanged?.Invoke();

            _stateData.GoToState(new ExploreCombineResultState(parentA, parentB, combinedId));
        }
    }
}