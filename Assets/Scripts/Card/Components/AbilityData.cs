using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Abilities;
using UnityEngine;

namespace CardGame.Data
{
    public class AbilityData : MonoBehaviour
    {
        private readonly List<Ability> _abilities = new();

        public IReadOnlyList<Ability> Abilities => _abilities;
        public bool IsStruggling => _abilities.Count > 0 && _abilities.All(a => !a.HasPp);

        public void Initialize(List<Ability> abilities)
        {
            _abilities.Clear();
            _abilities.AddRange(abilities);
        }

        public void RunAbility(int index, ActionContext ctx, Action onDone = null)
        {
            if (index < 0 || index >= _abilities.Count)
            {
                Debug.LogWarning($"AbilityData: No ability at index {index}.");
                return;
            }

            _abilities[index].Run(ctx, onDone);
        }
    }
}