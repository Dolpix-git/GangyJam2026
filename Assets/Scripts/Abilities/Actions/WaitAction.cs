using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class WaitAction : IAction
    {
        [JsonProperty] private float _seconds;

        public string Description => $"Wait {_seconds}s.";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            ctx.Runner.StartCoroutine(WaitRoutine(onComplete));
        }

        private IEnumerator WaitRoutine(Action onComplete)
        {
            yield return new WaitForSeconds(_seconds);
            onComplete();
        }
    }
}