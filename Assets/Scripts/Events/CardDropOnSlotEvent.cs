namespace Events
{
    public class CardBeginDragEvent
    {
        public CardDragHandler DragHandler;

        public CardBeginDragEvent(CardDragHandler dragHandler)
        {
            DragHandler = dragHandler;
        }
    }

    public class CardDropOnSlotEvent
    {
        public CardDropSlot DropSlot;

        public CardDropOnSlotEvent(CardDropSlot dropSlot)
        {
            DropSlot = dropSlot;
        }
    }
}