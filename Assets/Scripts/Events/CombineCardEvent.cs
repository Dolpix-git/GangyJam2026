using UnityEngine;

namespace Events
{
    public class CombineCardEvent
    {
        public CombineCardEvent(GameObject card)
        {
            Card = card;
        }

        public GameObject Card { get; set; }
    }
}