using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Player
{
    public class PlayerHand : MonoBehaviour
    {
        public IReadOnlyList<GameObject> Cards => _cards;
        public int Count => _cards.Count;

        private readonly List<GameObject> _cards = new();

        public void AddCard(GameObject card)
        {
            _cards.Add(card);
        }

        public bool RemoveCard(GameObject card)
        {
            return _cards.Remove(card);
        }

        public GameObject RemoveAt(int index)
        {
            if (index < 0 || index >= _cards.Count)
            {
                Debug.LogWarning($"PlayerHand: index {index} out of range.");
                return null;
            }

            var card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }
    }
}