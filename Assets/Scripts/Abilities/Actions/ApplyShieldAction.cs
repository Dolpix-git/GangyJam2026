using System;
using CardGame.Buffs;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class ApplyShieldAction : IAction
    {
        [JsonProperty] private int _absorb;
        [JsonProperty] private TargetSlot _target;

        public string Description => $"Shield {ActionEnumNames.Of(_target)} for {_absorb} absorb.";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var targets = ActionTargeting.Resolve(ctx, _target);
            foreach (var card in targets)
            {
                var buffData = card.GetComponent<BuffData>();
                if (buffData != null)
                {
                    buffData.Add(new ShieldBuff(_absorb));
                }
                else
                {
                    Debug.LogWarning($"ApplyShieldAction: '{card.name}' has no BuffData.");
                }
            }

            onComplete();
        }
    }
}