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
            return true;
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
            return card;
        }
    }
}