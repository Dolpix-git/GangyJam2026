using UnityEngine;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreBuyState : IState<ExploreStateData>
    {
        public void OnEnter(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Buy Phase (placeholder)");
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Buy phase complete.");
        }
    }
}