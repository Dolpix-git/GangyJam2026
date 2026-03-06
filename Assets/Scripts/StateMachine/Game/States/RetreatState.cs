using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class RetreatState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            ctx.CurrentPhase = GamePhase.Retreat;
            Debug.Log("[Retreat] Players may retreat cards. Press SPACE to continue.");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Retreat] Retreat phase complete.");
        }
    }
}