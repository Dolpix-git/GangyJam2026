using System;
using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class DamageAction : IAction
    {
        private readonly int _damage;

        public DamageAction(int damage)
        {
            _damage = damage;
        }

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var target = ResolveTarget(ctx);

            if (target != null)
            {
                var damageable = target.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(_damage);
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

        private static GameObject ResolveTarget(ActionContext ctx)
        {
            if (ctx.GameState == null)
            {
                return null;
            }

            var enemyPlayerIndex = ctx.CasterPlayerIndex == 0 ? 1 : 0;
            var enemyPlayer = ctx.GameState.Players[enemyPlayerIndex];
            return enemyPlayer.GetComponent<PlayerBoard>().GetSlot(ctx.CasterSlotIndex);
        }
    }
}