using UnityEngine;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreCombineState : IState<ExploreStateData>
    {
        public void OnEnter(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Combine Phase (placeholder)");
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Combine phase complete.");
        }
    }
}