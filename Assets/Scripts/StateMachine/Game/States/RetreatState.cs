using CardGame.Patterns;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class RetreatState : IState<GameStateData>
    {
        private int _activePlayer;

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Retreat] Enter Retreat Phase");
            GameStateSingleton.Instance.SetCurrentState(this);
            _activePlayer = 0;
            AdvanceToNextEligiblePlayer(ctx);
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Retreat] Retreat phase complete.");
            GameStateSingleton.Instance.SetCurrentState(null);
        }

        private void AdvanceToNextEligiblePlayer(GameStateData ctx)
        {
            while (_activePlayer < ctx.Players.Count)
            {
                var board = ctx.Players[_activePlayer].GetComponent<PlayerBoard>();
                if (CardCount(board) > 1)
                {
                    var playerIndex = _activePlayer;
                    ctx.Controllers[playerIndex].DoRetreatPhase(ctx, playerIndex, () =>
                    {
                        _activePlayer++;
                        AdvanceToNextEligiblePlayer(ctx);
                    });
                    return;
                }

                Debug.Log($"[Retreat] Player {_activePlayer + 1} has only 1 card — skipping.");
                _activePlayer++;
            }

            ctx.GoToState(new StruggleState());
        }

        private static int CardCount(PlayerBoard board)
        {
            var count = 0;
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                if (board.GetSlot(i) != null)
                {
                    count++;
                }
            }

            return count;
        }
    }
}