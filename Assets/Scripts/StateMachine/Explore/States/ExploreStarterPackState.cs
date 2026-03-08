using System.Collections.Generic;
using System.IO;
using CardGame.Card.CardData;
using CardGame.Data;
using CardGame.Run;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreStarterPackState : IState<ExploreStateData>
    {
        private const int StarterCount = 0;
        private static string CardsRoot => Path.Combine(Application.streamingAssetsPath, "Cards");

        public void OnEnter(ExploreStateData ctx)
        {
            if (ctx.Run.PlayerCardIds.Count == 0)
            {
                GiveStarterCards(ctx.Run);
            }

            ctx.GoToState(new ExploreBuyState());
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
        }

        private static void GiveStarterCards(RunContext run)
        {
            var pool = BuildCommonPool();

            if (pool.Count == 0)
            {
                Debug.LogWarning("[StarterPack] No Common cards found in Cards/.");
                return;
            }

            var available = new List<string>(pool);
            var count = Mathf.Min(StarterCount, available.Count);

            for (var i = 0; i < count; i++)
            {
                var idx = Random.Range(0, available.Count);
                run.PlayerCardIds.Add(available[idx]);
                Debug.Log($"[StarterPack] Granted '{available[idx]}'.");
                available.RemoveAt(idx);
            }

            run.Save();
        }

        private static List<string> BuildCommonPool()
        {
            var pool = new List<string>();

            if (!Directory.Exists(CardsRoot))
            {
                Debug.LogWarning($"[StarterPack] Cards root not found: {CardsRoot}");
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
                if (cardJson != null && cardJson.Rarity == CardRarity.Common)
                {
                    pool.Add(cardJson.CardId);
                }
            }

            return pool;
        }
    }
}