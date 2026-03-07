using System;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class FlankDamageAction : IAction
    {
        [JsonProperty] private int _damage;

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var targets = ActionTargeting.GetFlankingCards(ctx);

            if (targets.Count == 0)
            {
                Debug.Log("FlankDamageAction: No flanking targets.");
            }

            foreach (var target in targets)
            {
                var damageable = target.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(_damage);
                }
                else
                {
                    Debug.LogWarning("FlankDamageAction: Target has no IDamageable component.");
                }
            }

            onComplete();
        }
    }
}