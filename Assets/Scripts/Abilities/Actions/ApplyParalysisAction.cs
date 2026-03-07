using System;
using CardGame.Buffs;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class ApplyParalysisAction : IAction
    {
        [JsonProperty] private TargetSlot _target;
        [JsonProperty] private int _turns;

        public void Execute(ActionContext ctx, Action onComplete)
        {
            foreach (var card in ActionTargeting.Resolve(ctx, _target))
            {
                var buffData = card.GetComponent<BuffData>();
                if (buffData != null)
                {
                    buffData.Add(new ParalysisBuff(_turns));
                }
                else
                {
                    Debug.LogWarning($"ApplyParalysisAction: '{card.name}' has no BuffData.");
                }
            }

            onComplete();
        }
    }
}