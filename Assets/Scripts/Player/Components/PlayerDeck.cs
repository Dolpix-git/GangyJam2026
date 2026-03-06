using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Player
{
    public class PlayerDeck : MonoBehaviour
    {
        private readonly List<GameObject> _cards = new();

        public int Count => _cards.Count;

        public void AddCard(GameObject card)
        {
            _cards.Add(card);
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
            return drawn;
        }
    }
}
