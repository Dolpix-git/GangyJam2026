using System;

namespace CardGame.Abilities
{
    public class Ability
    {
        private readonly ActionPipeline _pipeline;

        public Ability(ActionPipeline pipeline, int maxPP)
        {
            _pipeline = pipeline;
            MaxPP = maxPP;
            CurrentPP = maxPP;
        }

        public string Description => _pipeline.Description;

        public int MaxPP { get; }
        public int CurrentPP { get; private set; }
        public bool HasPP => CurrentPP > 0;

        public void Run(ActionContext ctx, Action onDone = null)
        {
            if (!HasPP)
            {
                return;
            }

            CurrentPP--;
            _pipeline.Run(ctx, onDone);
        }

        public void RestorePP(int amount)
        {
            CurrentPP = Math.Min(MaxPP, CurrentPP + amount);
        }

        public void RestoreAllPP()
        {
            CurrentPP = MaxPP;
        }
    }
}