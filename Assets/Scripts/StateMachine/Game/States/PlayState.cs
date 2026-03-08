using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class PlayState : IState<GameStateData>
    {
        private readonly bool[] _done = new bool[2];

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Play] Enter Play Phase.");
            _done[0] = false;
            _done[1] = false;
            AdvanceToPlayer(ctx, 0);
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Play] Play phase complete.");
        }

        private void AdvanceToPlayer(GameStateData ctx, int playerIndex)
        {
            var playerObj = ctx.Players[playerIndex];
            var board = playerObj.GetComponent<PlayerBoard>();
            var hand = playerObj.GetComponent<PlayerHand>();

            if (!board.HasCards && hand.Count == 0)
            {
                ctx.GoToState(new EndGameState(playerIndex));
                return;
            }

            ctx.Controllers[playerIndex].DoPlayPhase(ctx, playerIndex, () =>
            {
                _done[playerIndex] = true;
                var next = playerIndex == 0 ? 1 : 0;
                if (!_done[next])
                {
                    AdvanceToPlayer(ctx, next);
                }
                else
                {
                    ctx.GoToState(new RetreatState());
                }
            });
        }
    }
}