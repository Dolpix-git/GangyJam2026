using System;
using CardGame.Buffs;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class ApplyFrostAction : IAction
    {
        [JsonProperty] private int _damagePerTurn;
        [JsonProperty] private TargetSlot _target;
        [JsonProperty] private int _turns;

        public void Execute(ActionContext ctx, Action onComplete)
        {
            foreach (var card in ActionTargeting.Resolve(ctx, _target))
            {
                var buffData = card.GetComponent<BuffData>();
                if (buffData != null)
                {
                    buffData.Add(new FrostBuff(_damagePerTurn, _turns));
                }
                else
                {
                    Debug.LogWarning($"ApplyFrostAction: '{card.name}' has no BuffData.");
                }
            }

            onComplete();
        }
    }
}