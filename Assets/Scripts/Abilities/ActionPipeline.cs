using System;
using System.Collections.Generic;

namespace CardGame.Abilities
{
    public class ActionPipeline
    {
        private readonly List<IAction> _actions;

        public ActionPipeline(List<IAction> actions)
        {
            _actions = actions;
        }

        public void Run(ActionContext ctx, Action onDone = null)
        {
            Execute(ctx, 0, onDone);
        }

        private void Execute(ActionContext ctx, int index, Action onDone)
        {
            if (index >= _actions.Count)
            {
                onDone?.Invoke();
                return;
            }

            _actions[index].Execute(ctx, () => Execute(ctx, index + 1, onDone));
        }
    }
}