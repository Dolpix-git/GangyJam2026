using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class PlayState : IState<GameStateData>
    {
        private readonly bool[] _done = new bool[2];
        private int _activePlayer;
        private int _selectedHandIndex;
        private bool _waitingForAdvance;

        public void OnEnter(GameStateData ctx)
        {
            _done[0] = false;
            _done[1] = false;
            _activePlayer = 0;
            _selectedHandIndex = -1;
            _waitingForAdvance = false;

            Debug.Log("[Play] === PLAY PHASE ===");
            PromptSelectCard(ctx, _activePlayer);
        }

        public void OnUpdate(GameStateData ctx)
        {
            if (_waitingForAdvance)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ctx.GoToState(new RetreatState());
                }

                return;
            }

            if (_selectedHandIndex == -1)
            {
                HandleCardSelection(ctx);
            }
            else
            {
                HandleSlotSelection(ctx);
            }
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Play] Play phase complete.");
        }

        private void HandleCardSelection(GameStateData ctx)
        {
            var hand = ctx.Players[_activePlayer].GetComponent<PlayerHand>();

            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    continue;
                }

                if (i >= hand.Count)
                {
                    Debug.Log($"[Play] No card at [{i + 1}]. Try again.");
                    return;
                }

                _selectedHandIndex = i;
                var identity = hand.Cards[i].GetComponent<CardIdentity>();
                Debug.Log(
                    $"[Play] Selected '{identity?.CardName ?? hand.Cards[i].name}'. Now pick a board slot (1/2/3):");
                PrintBoard(ctx, _activePlayer);
                return;
            }

            if (!Input.GetKeyDown(KeyCode.P))
            {
                return;
            }

            Debug.Log($"[Play] Player {_activePlayer + 1} passes.");
            ConfirmPlayer(ctx, _activePlayer);
        }

        private void HandleSlotSelection(GameStateData ctx)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Debug.Log("[Play] Cancelled. Pick a card again:");
                _selectedHandIndex = -1;
                PromptSelectCard(ctx, _activePlayer);
                return;
            }

            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    continue;
                }

                var playerObj = ctx.Players[_activePlayer];
                var hand = playerObj.GetComponent<PlayerHand>();
                var board = playerObj.GetComponent<PlayerBoard>();

                if (board.GetSlot(i) != null)
                {
                    Debug.Log($"[Play] Slot {i + 1} is occupied. Pick another slot.");
                    return;
                }

                var card = hand.RemoveAt(_selectedHandIndex);
                board.TryPlaceCard(card, i);

                var identity = card.GetComponent<CardIdentity>();
                Debug.Log(
                    $"[Play] Player {_activePlayer + 1} placed '{identity?.CardName ?? card.name}' in slot {i + 1}.");

                _selectedHandIndex = -1;
                ConfirmPlayer(ctx, _activePlayer);
                return;
            }
        }

        private void ConfirmPlayer(GameStateData ctx, int playerIndex)
        {
            _done[playerIndex] = true;
            var next = playerIndex == 0 ? 1 : 0;

            if (!_done[next])
            {
                _activePlayer = next;
                _selectedHandIndex = -1;
                PromptSelectCard(ctx, _activePlayer);
            }
            else
            {
                Debug.Log("[Play] Both players done. Press SPACE to continue.");
                _waitingForAdvance = true;
            }
        }

        private void PromptSelectCard(GameStateData ctx, int playerIndex)
        {
            var hand = ctx.Players[playerIndex].GetComponent<PlayerHand>();
            Debug.Log($"[Play] Player {playerIndex + 1} — pick a card from hand:");

            if (hand.Count == 0)
            {
                Debug.Log("  (empty hand) — Press P to pass.");
                return;
            }

            for (var i = 0; i < hand.Count; i++)
            {
                var identity = hand.Cards[i].GetComponent<CardIdentity>();
                Debug.Log($"  [{i + 1}] {identity?.CardName ?? hand.Cards[i].name}");
            }

            Debug.Log("  Press 1/2/3 to select, P to pass.");
        }

        private void PrintBoard(GameStateData ctx, int playerIndex)
        {
            var board = ctx.Players[playerIndex].GetComponent<PlayerBoard>();
            Debug.Log($"[Play] Player {playerIndex + 1}'s board:");

            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                var slot = board.GetSlot(i);
                var label = slot != null
                    ? slot.GetComponent<CardIdentity>()?.CardName ?? slot.name
                    : "(empty)";
                Debug.Log($"  [{i + 1}] {label}");
            }
        }
    }
}