using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Player
{
    public class PlayerGraveyard : MonoBehaviour
    {
        private readonly List<GameObject> _cards = new();

        public IReadOnlyList<GameObject> Cards => _cards;
        public int Count => _cards.Count;
        
        public event Action<GameObject> OnCardAdded;

        public void AddCard(GameObject card)
        {
            _cards.Add(card);
            OnCardAdded?.Invoke(card);
        }
    }
}
