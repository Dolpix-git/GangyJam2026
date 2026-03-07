using System.Collections.Generic;
using CardGame.Abilities;
using CardGame.Data;
using CardGame.ScriptableObjects;
using UnityEngine;

namespace CardGame.Card
{
    public class CardFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _cardPrefab;

        public GameObject CreateCard(CardDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogError("CardFactory: CardDefinition is null.");
                return null;
            }

            var card = Instantiate(_cardPrefab);
            card.name = definition.cardName;

            card.GetComponent<CardIdentity>().Initialize(
                definition.cardId,
                definition.cardName,
                definition.description
            );
            card.GetComponent<HealthData>().Initialize(definition.health);
            card.GetComponent<SpeedData>().Initialize(definition.speed);
            card.GetComponent<AbilityData>().Initialize(new List<ActionPipeline>());

            return card;
        }
    }
}