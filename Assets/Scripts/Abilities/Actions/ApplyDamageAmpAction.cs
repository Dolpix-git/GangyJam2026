using System;
using CardGame.Buffs;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class ApplyDamageAmpAction : IAction
    {
        [JsonProperty] private float _multiplier;
        [JsonProperty] private TargetSlot _target;
        [JsonProperty] private int _turns;

        public string Description => $"Boost {ActionEnumNames.Of(_target)} damage by {_multiplier}x for {_turns} turn(s).";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            foreach (var card in ActionTargeting.Resolve(ctx, _target))
            {
                var buffData = card.GetComponent<BuffData>();
                if (buffData != null)
                {
                    buffData.Add(new DamageAmpBuff(_multiplier, _turns));
                }
                else
                {
                    Debug.LogWarning($"ApplyDamageAmpAction: '{card.name}' has no BuffData.");
                }
            }

            onComplete();
        }
    }
}