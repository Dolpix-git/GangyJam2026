using System;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class HealAction : IAction
    {
        [JsonProperty] private int _amount;
        [JsonProperty] private TargetSlot _target = TargetSlot.Self;

        public string Description => $"Restore {_amount} HP to {ActionEnumNames.Of(_target)}.";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var targets = ActionTargeting.Resolve(ctx, _target);

            if (targets.Count == 0)
            {
                Debug.Log("HealAction: No targets found — no healing applied.");
            }

            foreach (var target in targets)
            {
                var healable = target.GetComponent<HealthData>();
                if (healable != null)
                {
                    healable.Heal(_amount);
                }
                else
                {
                    Debug.LogWarning($"HealAction: '{target.name}' has no HealthData component.");
                }
            }

            onComplete();
        }
    }
}