using CardGame.Data;
using CardGame.Patterns;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class ModeState : IState<GameStateData>
    {
        private int _activePlayer;

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Mode] Enter Mode Phase");

            GameStateSingleton.Instance.CurrentState = this;
            
            _activePlayer = 0;
            ClearAllModes(ctx);
            AdvanceToNextPlayer(ctx);
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Mode] Mode Selection Complete.");
            
            GameStateSingleton.Instance.CurrentState = null;
        }

        private void AdvanceToNextPlayer(GameStateData ctx)
        {
            if (_activePlayer >= ctx.Players.Count)
            {
                ctx.GoToState(new BattleState());
                return;
            }

            var playerIndex = _activePlayer;
            ctx.Controllers[playerIndex].DoModePhase(ctx, playerIndex, () =>
            {
                _activePlayer++;
                AdvanceToNextPlayer(ctx);
            });
        }

        private static void ClearAllModes(GameStateData ctx)
        {
            foreach (var playerObj in ctx.Players)
            {
                var board = playerObj.GetComponent<PlayerBoard>();
                for (var i = 0; i < PlayerBoard.BoardSize; i++)
                {
                    board.GetSlot(i)?.GetComponent<CardMode>()?.Clear();
                }
            }
        }
    }
}