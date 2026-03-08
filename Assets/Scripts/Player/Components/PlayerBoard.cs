using System;
using UnityEngine;

namespace CardGame.Player
{
    public class PlayerBoard : MonoBehaviour
    {
        public const int BoardSize = 3;
        private readonly GameObject[] _slots = new GameObject[BoardSize];

        public bool IsFull
        {
            get
            {
                for (var i = 0; i < BoardSize; i++)
                {
                    if (_slots[i] == null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool HasCards
        {
            get
            {
                for (var i = 0; i < BoardSize; i++)
                {
                    if (_slots[i] != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public event Action<GameObject, int> OnCardAdded;
        public event Action<int> OnCardRemoved;

        /// <summary>Fired after a shift. Carries the new slot contents (index → card, null if empty).</summary>
        public event Action<GameObject[]> OnBoardShifted;

        public int GetIndex(GameObject obj)
        {
            return Array.IndexOf(_slots, obj);
        }

        public GameObject GetSlot(int index)
        {
            return _slots[index];
        }

        public bool TryPlaceCard(GameObject card)
        {
            for (var i = 0; i < BoardSize; i++)
            {
                if (_slots[i] != null)
                {
                    continue;
                }

                _slots[i] = card;
                return true;
            }

            Debug.LogWarning("PlayerBoard: board is full, cannot place card.");
            return false;
        }

        public bool TryPlaceCard(GameObject card, int slotIndex)
        {
            if (slotIndex is < 0 or >= BoardSize)
            {
                Debug.LogWarning($"PlayerBoard: slot index {slotIndex} out of range.");
                return false;
            }

            if (_slots[slotIndex] != null)
            {
                Debug.LogWarning($"PlayerBoard: slot {slotIndex} is already occupied.");
                return false;
            }

            _slots[slotIndex] = card;
            OnCardAdded?.Invoke(card, slotIndex);
            return true;
        }

        public void ShiftLeft()
        {
            var wrap = _slots[0];
            for (var i = 0; i < BoardSize - 1; i++)
            {
                _slots[i] = _slots[i + 1];
            }

            _slots[BoardSize - 1] = wrap;

            FireShiftEvents();
        }

        public void ShiftRight()
        {
            var wrap = _slots[BoardSize - 1];
            for (var i = BoardSize - 1; i > 0; i--)
            {
                _slots[i] = _slots[i - 1];
            }

            _slots[0] = wrap;

            FireShiftEvents();
        }

        private void FireShiftEvents()
        {
            for (var i = 0; i < BoardSize; i++)
            {
                if (_slots[i] != null)
                {
                    OnCardAdded?.Invoke(_slots[i], i);
                }
            }

            OnBoardShifted?.Invoke((GameObject[])_slots.Clone());
        }

        public GameObject RemoveAt(int slotIndex)
        {
            if (slotIndex is < 0 or >= BoardSize)
            {
                Debug.LogWarning($"PlayerBoard: slot index {slotIndex} out of range.");
                return null;
            }

            var card = _slots[slotIndex];
            _slots[slotIndex] = null;
            OnCardRemoved?.Invoke(slotIndex);
            return card;
        }
    }
}