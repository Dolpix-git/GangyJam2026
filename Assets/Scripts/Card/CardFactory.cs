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

        public GameObject CreateCard(string cardName)
        {
            var cardDir = Path.Combine(CardsRoot, cardName);

            if (!Directory.Exists(cardDir))
            {
                Debug.LogError($"[CardFactory] No card folder found for '{cardName}' at {cardDir}");
                return null;
            }

            var cardJson = LoadJson<CardJson>(Path.Combine(cardDir, "card.json"));
            if (cardJson == null)
            {
                Debug.LogError($"[CardFactory] Missing card.json for '{cardName}'");
                return null;
            }

            var card = Instantiate(_cardPrefab);
            card.name = cardName;

            card.GetComponent<CardIdentity>().Initialize(cardJson.CardId, cardJson.CardName, cardJson.Description);
            card.GetComponent<HealthData>().Initialize(cardJson.MaxHealth);
            card.GetComponent<SpeedData>().Initialize(cardJson.Speed);
            card.GetComponent<StruggleData>().Initialize(cardJson.StruggleDamage);
            card.GetComponent<TagData>().Initialize(cardJson.Tags);
            card.GetComponent<AbilityData>().Initialize(LoadAbilities(cardDir));

            return card;
        }

        private static List<Ability> LoadAbilities(string cardDir)
        {
            var abilitiesDir = Path.Combine(cardDir, "Abilities");
            if (!Directory.Exists(abilitiesDir))
                return new List<Ability>();

            var abilities = new List<Ability>();

            foreach (var file in Directory.GetFiles(abilitiesDir, "*.json"))
            {
                var abilityJson = LoadJson<AbilityJson>(file);
                if (abilityJson == null) continue;

                var actions = abilityJson.Steps
                    .Select(s => s.Action)
                    .Where(a => a != null)
                    .ToList();

                abilities.Add(new Ability(new ActionPipeline(actions), abilityJson.MaxPP));
            }

            return abilities;
        }

        private static T LoadJson<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[CardFactory] File not found: {path}");
                return null;
            }

            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), _settings);
        }
    }
}