using UnityEngine;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreCombineState : IState<ExploreStateData>
    {
        public void OnEnter(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Combine Phase");
            ctx.CombineCardsMenu.SetActive(true);
            ctx.Combine?.Populate();
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
            ctx.CombineCardsMenu.SetActive(false);
            Debug.Log("[Explore] Combine phase complete.");
        }
    }
}