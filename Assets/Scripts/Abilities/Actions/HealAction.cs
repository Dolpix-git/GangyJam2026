using System;
using CardGame.Data;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class HealAction : IAction
    {
        private readonly int _amount;

        public HealAction(int amount)
        {
            _amount = amount;
        }

        public void Execute(ActionContext ctx, Action onComplete)
        {
            if (ctx.Caster != null)
            {
                var healable = ctx.Caster.GetComponent<IHealable>();
                if (healable != null)
                {
                    healable.Heal(_amount);
                }
                else
                {
                    Debug.LogWarning("HealAction: Caster has no IHealable component.");
                }
            }

            onComplete();
        }
    }
}