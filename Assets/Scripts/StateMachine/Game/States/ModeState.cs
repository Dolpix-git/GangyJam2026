using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class ModeState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Mode] Players select card modes / abilities. Press SPACE to continue.");
        }

        public void OnUpdate(GameStateData ctx)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ctx.GoToState(new BattleState());
            }
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Mode] Mode selection complete.");
        }
    }
}