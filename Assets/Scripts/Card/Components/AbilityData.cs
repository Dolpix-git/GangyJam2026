using System;
using System.Collections.Generic;
using CardGame.Abilities;
using UnityEngine;

namespace CardGame.Data
{
    /// <summary>
    ///     Holds the list of ability pipelines belonging to a card.
    ///     Each pipeline is a named sequence of actions that execute in order.
    /// </summary>
    public class AbilityData : MonoBehaviour
    {
        private readonly List<ActionPipeline> _abilities = new();

        public IReadOnlyList<ActionPipeline> Abilities => _abilities;

        public void Initialize(List<ActionPipeline> abilities)
        {
            _abilities.Clear();
            _abilities.AddRange(abilities);
        }

        public void AddAbility(ActionPipeline pipeline)
        {
            _abilities.Add(pipeline);
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