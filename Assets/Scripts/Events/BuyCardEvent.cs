using UnityEngine;

namespace Events
{
    public class BuyCardEvent
    {
        public GameObject BoughtCard { get; set; }

        public BuyCardEvent(GameObject boughtCard)
        {
            BoughtCard = boughtCard;
        }
    }
}