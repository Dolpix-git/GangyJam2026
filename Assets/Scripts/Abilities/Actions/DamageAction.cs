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
            var target = ActionTargeting.GetOpposingCard(ctx);

            if (target != null)
            {
                var damageable = target.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    var damage = ctx.Caster.GetComponent<BuffData>()?.ModifyOutgoingDamage(_damage) ?? _damage;
                    damageable.TakeDamage(damage);
                }
                else
                {
                    Debug.LogWarning("DamageAction: Target has no IDamageable component.");
                }
            }
            else
            {
                Debug.Log("DamageAction: No card in opposing slot — no damage dealt.");
            }

            onComplete();
        }
    }
}