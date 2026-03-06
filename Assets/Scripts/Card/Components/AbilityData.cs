using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Data
{
    /// <summary>
    /// Pure data component for storing ability references.
    /// No behavior, only data storage.
    /// </summary>
    public class AbilityData : MonoBehaviour
    {
        [SerializeField] private List<string> abilityIds = new List<string>();

        public IReadOnlyList<string> AbilityIds => abilityIds;

        public void Initialize(List<string> abilities)
        {
            abilityIds = new List<string>(abilities);
        }

        public void AddAbility(string abilityId)
        {
            if (!abilityIds.Contains(abilityId))
            {
                abilityIds.Add(abilityId);
            }
        }

        public void RemoveAbility(string abilityId)
        {
            abilityIds.Remove(abilityId);
        }

        public bool HasAbility(string abilityId)
        {
            return abilityIds.Contains(abilityId);
        }
    }
}
