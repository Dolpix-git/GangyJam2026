using System.Collections.Generic;
using UnityEngine;

namespace CardGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewCardDefinition", menuName = "Card Game/Card Definition")]
    public class CardDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string cardId;
        public string cardName;
        [TextArea(3, 5)]
        public string description;

        [Header("Stats")]
        public int health;
        public int speed;

        [Header("Abilities")]
        public List<string> abilityIds = new List<string>();
    }
}
