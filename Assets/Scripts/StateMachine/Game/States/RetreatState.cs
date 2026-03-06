using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class RetreatState : IState<GameStateData>
    {
        private int _activePlayer;

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Retreat] === RETREAT PHASE ===");
            _activePlayer = 0;
            PromptPlayer(ctx, _activePlayer);
        }

        public void OnUpdate(GameStateData ctx)
        {
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    continue;
                }

                TryRetreat(ctx, _activePlayer, i);
                return;
            }

            if (!Input.GetKeyDown(KeyCode.P))
            {
                return;
            }

            Debug.Log($"[Retreat] Player {_activePlayer + 1} done retreating.");
            _activePlayer++;

            if (_activePlayer < ctx.Players.Count)
            {
                PromptPlayer(ctx, _activePlayer);
            }
            else
            {
                ctx.GoToState(new ModeState());
            }
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Retreat] Retreat phase complete.");
        }

        private void TryRetreat(GameStateData ctx, int playerIndex, int slotIndex)
        {
            var playerObj = ctx.Players[playerIndex];
            var board = playerObj.GetComponent<PlayerBoard>();
            var hand = playerObj.GetComponent<PlayerHand>();

            var card = board.GetSlot(slotIndex);
            if (card == null)
            {
                Debug.Log($"[Retreat] Slot {slotIndex + 1} is empty.");
                return;
            }

            board.RemoveAt(slotIndex);
            hand.AddCard(card);

            var identity = card.GetComponent<CardIdentity>();
            Debug.Log(
                $"[Retreat] Player {playerIndex + 1} retreated '{identity?.CardName ?? card.name}' from slot {slotIndex + 1} back to hand.");
            PromptPlayer(ctx, playerIndex);
        }

        private void PromptPlayer(GameStateData ctx, int playerIndex)
        {
            var board = ctx.Players[playerIndex].GetComponent<PlayerBoard>();
            Debug.Log($"[Retreat] Player {playerIndex + 1}'s board:");

            var anyCards = false;
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                var slot = board.GetSlot(i);
                if (slot == null)
                {
                    continue;
                }

                var identity = slot.GetComponent<CardIdentity>();
                Debug.Log($"  [{i + 1}] {identity?.CardName ?? slot.name}");
                anyCards = true;
            }

            if (!anyCards)
            {
                Debug.Log("  (empty board)");
            }

            Debug.Log("  Press 1/2/3 to retreat a card, P to pass.");
        }
    }
}