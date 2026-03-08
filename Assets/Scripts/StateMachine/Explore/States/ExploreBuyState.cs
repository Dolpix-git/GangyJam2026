using UnityEngine;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreBuyState : IState<ExploreStateData>
    {
        public void OnEnter(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Buy Phase");
            ctx.Shop.Populate();
            ctx.BuyMenu.SetActive(true);
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
            ctx.BuyMenu.SetActive(false);
            Debug.Log("[Explore] Buy phase complete.");
        }
    }
}