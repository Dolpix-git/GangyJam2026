using System;
using System.Collections;
using UnityEngine;

namespace CardGame.Abilities.Actions
{
    public class WaitAction : IAction
    {
        private readonly float _seconds;

        public WaitAction(float seconds)
        {
            _seconds = seconds;
        }

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