using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGame.Card.CardData;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreSetupEnemyState : IState<ExploreStateData>
    {
        private static string CardsRoot => Path.Combine(Application.streamingAssetsPath, "Cards");

        public void OnEnter(ExploreStateData ctx)
        {
            ctx.Run.IncrementRunCount();

            var enemyCards = RollEnemyDeck(ctx.Run.RunCount);
            ctx.Run.EnemyCardIds.Clear();
            ctx.Run.EnemyCardIds.AddRange(enemyCards);

            Debug.Log($"[EnemySetup] Run {ctx.Run.RunCount}: enemy deck = [{string.Join(", ", enemyCards)}]");

            ctx.GoToState(new ExploreFightState());
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
        }

        private static List<string> RollEnemyDeck(int runCount)
        {
            var pool = BuildWeightedPool(runCount);
            if (pool.Count == 0)
            {
                Debug.LogWarning("[EnemySetup] No cards found in pool.");
                return new List<string>();
            }

            var cardCount = Mathf.Clamp(1 + runCount / 2, 1, 8);
            var result = new List<string>();
            var available = new List<(string id, int weight)>(pool);

            for (var i = 0; i < cardCount && available.Count > 0; i++)
            {
                var picked = WeightedPick(available);
                result.Add(picked.id);
                available.Remove(picked);
            }

            return result;
        }

        private static List<(string id, int weight)> BuildWeightedPool(int runCount)
        {
            var pool = new List<(string id, int weight)>();

            if (!Directory.Exists(CardsRoot))
            {
                Debug.LogWarning($"[EnemySetup] Cards root not found: {CardsRoot}");
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

                var weight = RarityWeight(cardJson.Rarity, runCount);
                if (weight > 0)
                {
                    pool.Add((cardJson.CardId, weight));
                }
            }

            return pool;
        }

        private static int RarityWeight(CardRarity rarity, int runCount)
        {
            return rarity switch
            {
                CardRarity.Common => Mathf.Max(60 - runCount * 4, 5),
                CardRarity.Uncommon => 25 + runCount * 1,
                CardRarity.Rare => 10 + runCount * 2,
                CardRarity.Epic => 4 + runCount * 2,
                CardRarity.Legendary => 1 + runCount * 1,
                _ => 0
            };
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