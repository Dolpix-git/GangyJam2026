using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Player
{
    public class PlayerDeck : MonoBehaviour
    {
        private readonly List<GameObject> _cards = new();

        public int Count => _cards.Count;

        public event Action OnDeckChanged;

        public void AddCard(GameObject card)
        {
            _cards.Add(card);
            OnDeckChanged?.Invoke();
        }

        public void Shuffle()
        {
            for (var i = _cards.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
            }
        }

        public GameObject DrawTop()
        {
            if (_cards.Count == 0)
            {
                Debug.LogWarning("PlayerDeck: draw pile is empty.");
                return null;
            }

            var drawn = _cards[0];
            _cards.RemoveAt(0);
            
            OnDeckChanged?.Invoke();
            return drawn;
        }
    }
}
