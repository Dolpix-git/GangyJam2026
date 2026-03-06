using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class PlayState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            ctx.CurrentPhase = GamePhase.Play;
            Debug.Log("[Play] Players place cards on the board. Press SPACE to continue.");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Play] Play phase complete.");
        }
    }
}