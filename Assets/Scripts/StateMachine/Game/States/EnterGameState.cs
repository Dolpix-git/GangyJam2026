using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class EnterGameState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            ctx.CurrentPhase = GamePhase.EnterGame;
            Debug.Log("[EnterGame] Game is starting. Press SPACE to begin.");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[EnterGame] Leaving enter game phase.");
        }
    }
    
}