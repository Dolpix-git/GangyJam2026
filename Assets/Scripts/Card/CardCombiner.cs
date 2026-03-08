using System;
using System.Collections.Generic;
using System.IO;
using CardGame.Buffs;
using CardGame.Card.CardData;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Card
{
    public static class CardCombiner
    {
        private const int MutationRange = 2;
        private const int StatMin = 0;
        private const int StatMax = 20;
        private const float AbilityInheritChance = 0.6f;

        private static readonly CardRarity[] _rarityOrder =
        {
            CardRarity.Common, CardRarity.Uncommon, CardRarity.Rare, CardRarity.Epic, CardRarity.Legendary
        };

        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            Formatting = Formatting.Indented
        };

        public static string Combine(GameObject cardA, GameObject cardB)
        {
            var identityA = cardA.GetComponent<CardIdentity>();
            var identityB = cardB.GetComponent<CardIdentity>();

            if (identityA == null || identityB == null)
            {
                Debug.LogError("[CardCombiner] One or both cards are missing CardIdentity.");
                return null;
            }

            var (jsonA, dirA) = CardFactory.FindCardDirectory(identityA.CardId);
            var (jsonB, dirB) = CardFactory.FindCardDirectory(identityB.CardId);

            if (jsonA == null || jsonB == null)
            {
                Debug.LogError(
                    $"[CardCombiner] Could not load card data for '{identityA.CardId}' or '{identityB.CardId}'.");
                return null;
            }

            var combinedId = "combined_" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var outDir = Path.Combine(Application.persistentDataPath, "RunCards", combinedId);
            Directory.CreateDirectory(outDir);

            var childJson = new CardJson
            {
                CardId = combinedId,
                CardName = GenerateName(jsonA.CardName, jsonB.CardName),
                Description = $"Born of {jsonA.CardName} and {jsonB.CardName}.",
                MaxHealth = MutateStat(jsonA.MaxHealth, jsonB.MaxHealth),
                Speed = MutateStat(jsonA.Speed, jsonB.Speed),
                StruggleDamage = MutateStat(jsonA.StruggleDamage, jsonB.StruggleDamage),
                Rarity = CombineRarity(jsonA.Rarity, jsonB.Rarity),
                Tags = new List<CardTag>()
            };

            File.WriteAllText(Path.Combine(outDir, "card.json"), JsonConvert.SerializeObject(childJson, _jsonSettings));

            CopyInheritedAbilities(dirA, dirB, outDir);

            Debug.Log($"[CardCombiner] '{jsonA.CardName}' + '{jsonB.CardName}' → '{childJson.CardName}' " +
                      $"(HP:{childJson.MaxHealth} SPD:{childJson.Speed} STR:{childJson.StruggleDamage} " +
                      $"Rarity:{childJson.Rarity}) saved to {outDir}");

            return combinedId;
        }

        private static void CopyInheritedAbilities(string dirA, string dirB, string outDir)
        {
            var abilitiesOut = Path.Combine(outDir, "Abilities");
            var copiedFilenames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var sourceDir in new[] { dirA, dirB })
            {
                var abilitiesIn = Path.Combine(sourceDir, "Abilities");
                if (!Directory.Exists(abilitiesIn))
                {
                    continue;
                }

                foreach (var file in Directory.GetFiles(abilitiesIn, "*.json"))
                {
                    var filename = Path.GetFileName(file);
                    if (copiedFilenames.Contains(filename))
                    {
                        continue;
                    }

                    if (Random.value <= AbilityInheritChance)
                    {
                        Directory.CreateDirectory(abilitiesOut);
                        File.Copy(file, Path.Combine(abilitiesOut, filename), true);
                        copiedFilenames.Add(filename);
                    }
                }
            }
        }

        private static int MutateStat(int a, int b)
        {
            var lo = Mathf.Min(a, b);
            var hi = Mathf.Max(a, b);
            var baseVal = lo == hi ? lo : Random.Range(lo, hi + 1);
            return Mathf.Clamp(baseVal + WeightedMutation(MutationRange), StatMin, StatMax);
        }

        private static int WeightedMutation(int range)
        {
            return Mathf.RoundToInt(Random.Range(0f, range) - Random.Range(0f, range));
        }

        private static CardRarity CombineRarity(CardRarity a, CardRarity b)
        {
            var idxA = Array.IndexOf(_rarityOrder, a);
            var idxB = Array.IndexOf(_rarityOrder, b);
            var promoted = Mathf.Min(Mathf.Max(idxA, idxB) + 1, _rarityOrder.Length - 1);
            return _rarityOrder[promoted];
        }

        private static string GenerateName(string nameA, string nameB)
        {
            var a = nameA.Length > 2 ? nameA[..Mathf.CeilToInt(nameA.Length / 2f)] : nameA;
            var b = nameB.Length > 2 ? nameB[Mathf.FloorToInt(nameB.Length / 2f)..] : nameB;
            return Random.value < 0.5f ? b + a : a + b;
        }
    }
}