using System;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class HealAction : IAction
    {
        [JsonProperty] private int _amount;

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