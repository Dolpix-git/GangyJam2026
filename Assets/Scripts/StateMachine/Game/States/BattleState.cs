using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class BattleState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            ctx.CurrentPhase = GamePhase.Battle;
            Debug.Log("[Battle] Cards battle! Press SPACE to start next round.");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Battle] Battle phase complete. Starting new round.");
        }
    }
}