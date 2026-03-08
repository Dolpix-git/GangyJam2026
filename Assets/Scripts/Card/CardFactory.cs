using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGame.Abilities;
using CardGame.Card.CardData;
using CardGame.Data;
using CardGame.Patterns;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Card
{
    public class CardFactory : MonoSingleton<CardFactory>
    {
        private static readonly JsonSerializerSettings _settings = new()
        {
            Converters = { new ActionStepConverter() }
        };

        [SerializeField] private GameObject _cardPrefab;

        private static string CardsRoot => Path.Combine(Application.streamingAssetsPath, "Cards");
        private static string DevCardsRoot => Path.Combine(Application.streamingAssetsPath, "DevCards");
        private static string RunCardsRoot => Path.Combine(Application.persistentDataPath, "RunCards");

        public GameObject CreateCard(string cardId)
        {
            var cardDir = ResolveCardDirectory(cardId);
            if (cardDir == null)
            {
                Debug.LogError($"[CardFactory] No card folder found for '{cardId}'.");
                return null;
            }

            var cardJson = LoadJson<CardJson>(Path.Combine(cardDir, "card.json"));
            if (cardJson == null)
            {
                Debug.LogError($"[CardFactory] Missing card.json for '{cardId}'.");
                return null;
            }

            var card = Instantiate(_cardPrefab);
            card.name = cardId;

            card.GetComponent<CardIdentity>().Initialize(cardJson.CardId, cardJson.CardName, cardJson.Description);
            card.GetComponent<HealthData>().Initialize(cardJson.MaxHealth);
            card.GetComponent<SpeedData>().Initialize(cardJson.Speed);
            card.GetComponent<StruggleData>().Initialize(cardJson.StruggleDamage);
            card.GetComponent<RarityData>().Initialize(cardJson.Rarity);
            card.GetComponent<CostData>().Initialize(cardJson.Cost);
            card.GetComponent<TagData>().Initialize(cardJson.Tags);
            card.GetComponent<AbilityData>().Initialize(LoadAbilities(cardDir));

            return card;
        }

        public static (CardJson cardJson, string cardDir) FindCardDirectory(string cardId)
        {
            var dir = ResolveCardDirectory(cardId);
            if (dir == null)
            {
                return (null, null);
            }

            var json = LoadJson<CardJson>(Path.Combine(dir, "card.json"));
            return (json, dir);
        }

        private static string ResolveCardDirectory(string cardId)
        {
            var inCards = Path.Combine(CardsRoot, cardId);
            if (Directory.Exists(inCards))
            {
                return inCards;
            }

            var inDev = Path.Combine(DevCardsRoot, cardId);
            if (Directory.Exists(inDev))
            {
                Debug.LogWarning($"[CardFactory] '{cardId}' not found in Cards — falling back to DevCards.");
                return inDev;
            }

            var inRun = Path.Combine(RunCardsRoot, cardId);
            if (Directory.Exists(inRun))
            {
                return inRun;
            }

            return null;
        }

        private static List<Ability> LoadAbilities(string cardDir)
        {
            var abilitiesDir = Path.Combine(cardDir, "Abilities");
            if (!Directory.Exists(abilitiesDir))
            {
                return new List<Ability>();
            }

            var abilities = new List<Ability>();
            foreach (var file in Directory.GetFiles(abilitiesDir, "*.json"))
            {
                var abilityJson = LoadJson<AbilityJson>(file);
                if (abilityJson == null)
                {
                    continue;
                }

                var actions = abilityJson.Steps.Select(s => s.Action).Where(a => a != null).ToList();
                abilities.Add(new Ability(new ActionPipeline(actions), abilityJson.MaxPp, abilityJson.Name));
            }

            return abilities;
        }

        private static T LoadJson<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), _settings);
            }

            Debug.LogWarning($"[CardFactory] File not found: {path}");
            return null;
        }
    }
}