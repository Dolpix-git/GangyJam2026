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

        public string Description => $"Paralyse {ActionEnumNames.Of(_target)} for {_turns} turn(s).";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            var targets = ActionTargeting.Resolve(ctx, _target);
            foreach (var card in targets)
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