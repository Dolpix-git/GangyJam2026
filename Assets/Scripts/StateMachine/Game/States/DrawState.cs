using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class DrawState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            ctx.CurrentPhase = GamePhase.Draw;
            Debug.Log("[Draw] Players draw cards. Press SPACE to continue.");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Draw] Draw phase complete.");
        }
    }
}