using System;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class DamageAction : IAction
    {
        [JsonProperty] private int _damage;

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var damage = ctx.Caster.GetComponent<BuffData>()?.ModifyOutgoingDamage(_damage) ?? _damage;
            var targets = ActionTargeting.Resolve(ctx, TargetSlot.EnemyOpposing);

            if (targets.Count == 0)
            {
                Debug.Log("DamageAction: No card in opposing slot — no damage dealt.");
            }

            foreach (var target in targets)
            {
                var damageable = target.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
                else
                {
                    Debug.LogWarning("DamageAction: Target has no IDamageable component.");
                }
            }

            onComplete();
        }
    }
}