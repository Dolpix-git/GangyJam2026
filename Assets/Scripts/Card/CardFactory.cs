using CardGame.Data;
using CardGame.ScriptableObjects;
using UnityEngine;

namespace CardGame.Card
{
    /// <summary>
    /// Creates card GameObjects from CardDefinitions.
    /// Attaches the required DOD components to each instantiated card.
    /// </summary>
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;

        public GameObject CreateCard(CardDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogError("CardFactory: CardDefinition is null.");
                return null;
            }

            GameObject card = Instantiate(cardPrefab);
            card.name = definition.cardName;

            card.GetComponent<CardIdentity>().Initialize(definition.cardId, definition.cardName, definition.description);
            card.GetComponent<HealthData>().Initialize(definition.health);
            card.GetComponent<SpeedData>().Initialize(definition.speed);
            card.GetComponent<AbilityData>().Initialize(definition.abilityIds);

            return card;
        }
    }
}
