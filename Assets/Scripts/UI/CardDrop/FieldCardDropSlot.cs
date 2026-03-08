using UnityEngine;

namespace CardGame.UI.CardDrop
{
    public class FieldCardDropSlot: CardDropSlot
    {
        [SerializeField] private int _slotIndex = -1;
        
        public int SlotIndex => _slotIndex;
    }
}