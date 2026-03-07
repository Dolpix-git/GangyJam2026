using System;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class DamageAction : IAction
    {
        [JsonProperty] private int _damage;
        [JsonProperty] private TargetSlot _target = TargetSlot.EnemyOpposing;

        public string Description => $"Deal {_damage} damage to {ActionEnumNames.Of(_target)}.";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var damage = ctx.Caster.GetComponent<BuffData>()?.ModifyOutgoingDamage(_damage) ?? _damage;
            var targets = ActionTargeting.Resolve(ctx, _target);

            if (targets.Count == 0)
            {
                Debug.Log("DamageAction: No targets found — no damage dealt.");
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
                    Debug.LogWarning($"DamageAction: '{target.name}' has no IDamageable component.");
                }
            }

            onComplete();
        }
    }
}