using System;

namespace CardGame.Abilities
{
    public class Ability
    {
        private readonly ActionPipeline _pipeline;

        public Ability(ActionPipeline pipeline, int maxPp, string name)
        {
            _pipeline = pipeline;
            MaxPp = maxPp;
            CurrentPp = maxPp;
            Name = name;
        }

        public string Description => _pipeline.Description;

        public string Name { get; private set; }
        public int MaxPp { get; }
        public int CurrentPp { get; private set; }
        public bool HasPp => CurrentPp > 0;

        public void Run(ActionContext ctx, Action onDone = null)
        {
            if (!HasPp)
            {
                return;
            }

            CurrentPp--;
            _pipeline.Run(ctx, onDone);
        }

        public void RestorePp(int amount)
        {
            CurrentPp = Math.Min(MaxPp, CurrentPp + amount);
        }

        public void RestoreAllPp()
        {
            CurrentPp = MaxPp;
        }
    }
}