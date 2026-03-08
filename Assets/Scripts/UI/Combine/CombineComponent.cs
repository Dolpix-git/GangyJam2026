using System;
using System.Collections.Generic;
using CardGame.Card;
using CardGame.Run;
using UnityEngine;

namespace CardGame.UI.Combine
{
    public class CombineComponent : MonoBehaviour
    {
        private readonly List<GameObject> _cards = new();

        public IReadOnlyList<GameObject> Cards => _cards;

        private void OnDestroy()
        {
            ClearCards();
        }

        public event Action OnPopulated;

        public void Populate()
        {
            ClearCards();

            Debug.Log(RunContext.Instance.PlayerCardIds.Count);

            foreach (var cardId in RunContext.Instance.PlayerCardIds)
            {
                var card = CardFactory.Instance.CreateCard(cardId);
                if (card == null)
                {
                    Debug.LogWarning("Fucked up");
                    continue;
                }

                _cards.Add(card);
                Debug.Log($"[Combine] Loaded player card '{cardId}' for selection.");
            }

            Debug.Log($"[Combine] Showing {_cards.Count} card(s) from player collection.");
            OnPopulated?.Invoke();
        }

        private void ClearCards()
        {
            foreach (var card in _cards)
            {
                if (card != null)
                {
                    Destroy(card);
                }
            }

            _cards.Clear();
        }
    }
}