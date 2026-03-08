using UnityEngine;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreCombineResultState : IState<ExploreStateData>
    {
        private readonly string _combinedId;
        private readonly GameObject _parentA;
        private readonly GameObject _parentB;

        public ExploreCombineResultState(GameObject parentA, GameObject parentB, string combinedId)
        {
            _parentA = parentA;
            _parentB = parentB;
            _combinedId = combinedId;
        }

        public void OnEnter(ExploreStateData ctx)
        {
            ctx.CombineResultMenu.SetActive(true);
            ctx.CombineResult.Show(_parentA, _parentB, _combinedId);
            Debug.Log($"[Explore] Showing combine result for '{_combinedId}'.");
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
            ctx.CombineResultMenu.SetActive(false);
        }
    }
}