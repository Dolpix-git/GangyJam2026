using CardGame.Run;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame.StateMachine.Explore.States
{
    public class ExploreFightState : IState<ExploreStateData>
    {
        public void OnEnter(ExploreStateData ctx)
        {
            Debug.Log("[Explore] Heading to battle...");
            ctx.Run.Save();
            SceneManager.LoadScene(SceneNames.Battle);
        }

        public void OnUpdate(ExploreStateData ctx)
        {
        }

        public void OnExit(ExploreStateData ctx)
        {
        }
    }
}