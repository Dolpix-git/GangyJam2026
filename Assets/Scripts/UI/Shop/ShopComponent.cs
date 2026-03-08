using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGame.Card;
using CardGame.Card.CardData;
using CardGame.Data;
using CardGame.StateMachine.Explore;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.UI.Shop
{
    public class ShopComponent : MonoBehaviour
    {
        private const int OfferSize = 5;

        private static readonly Dictionary<CardRarity, int> _weights = new()
        {
            { CardRarity.Common, 60 },
            { CardRarity.Uncommon, 25 },
            { CardRarity.Rare, 10 },
            { CardRarity.Epic, 4 },
            { CardRarity.Legendary, 1 }
        };

        [SerializeField] private ExploreStateData _stateData;
        private readonly List<GameObject> _cards = new();

        public IReadOnlyList<GameObject> Cards => _cards;

        private static string CardsRoot => Path.Combine(Application.streamingAssetsPath, "Cards");

        private void OnDestroy()
        {
            ClearCards();
        }

        public event Action OnPopulated;

        public void Populate()
        {
            ClearCards();

            foreach (var id in RollOffer())
            {
                var card = CardFactory.Instance.CreateCard(id);
                if (card == null)
                {
                    continue;
                }

                _cards.Add(card);
                Debug.Log($"[Shop] Created card '{id}'.");
            }

            Debug.Log($"[Shop] Stocked with {_cards.Count} card(s).");
            OnPopulated?.Invoke();
        }

        public void RemoveCard(GameObject card)
        {
            _cards.Remove(card);
        }

        private void ClearCards()
        {
            foreach (var card in _cards)
            {
                if (card != null)
                {
                    Destroy(card);
                }
            }

            _cards.Clear();
        }

        private static List<string> RollOffer()
        {
            var pool = BuildPool();
            if (pool.Count == 0)
            {
                Debug.LogWarning("[Shop] No cards found in pool.");
                return new List<string>();
            }

            var offer = new List<string>();
            var available = new List<(string id, int weight)>(pool);
            var count = Mathf.Min(OfferSize, available.Count);

            for (var i = 0; i < count; i++)
            {
                var picked = WeightedPick(available);
                offer.Add(picked.id);
                available.Remove(picked);
            }

            return offer;
        }

        private static List<(string id, int weight)> BuildPool()
        {
            var pool = new List<(string id, int weight)>();

            if (!Directory.Exists(CardsRoot))
            {
                Debug.LogWarning($"[Shop] Cards root not found: {CardsRoot}");
                return pool;
            }

            foreach (var cardDir in Directory.GetDirectories(CardsRoot))
            {
                var jsonPath = Path.Combine(cardDir, "card.json");
                if (!File.Exists(jsonPath))
                {
                    continue;
                }

                var cardJson = JsonConvert.DeserializeObject<CardJson>(File.ReadAllText(jsonPath));
                if (cardJson == null)
                {
                    continue;
                }

                if (_weights.TryGetValue(cardJson.Rarity, out var weight))
                {
                    pool.Add((cardJson.CardId, weight));
                }
            }

            return pool;
        }

        private static (string id, int weight) WeightedPick(List<(string id, int weight)> pool)
        {
            var total = pool.Sum(e => e.weight);
            var roll = Random.Range(0, total);
            var cumulative = 0;

            foreach (var entry in pool)
            {
                cumulative += entry.weight;
                if (roll < cumulative)
                {
                    return entry;
                }
            }

            return pool[^1];
        }
    }
}