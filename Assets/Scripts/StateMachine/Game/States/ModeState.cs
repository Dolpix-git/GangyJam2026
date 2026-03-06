using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class ModeState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            ctx.CurrentPhase = GamePhase.Mode;
            Debug.Log("[Mode] Players select card modes / abilities. Press SPACE to continue.");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Mode] Mode selection complete.");
        }
    }
}