using System;

namespace CardGame.Abilities
{
    /// <summary>
    /// A single step in an action pipeline.
    /// Call onComplete() when finished — this advances the pipeline to the next action.
    /// Timed/animated actions should call onComplete() at the end of their coroutine.
    /// </summary>
    public interface IAction
    {
        string Description { get; }
        void Execute(ActionContext ctx, Action onComplete);
    }
}
