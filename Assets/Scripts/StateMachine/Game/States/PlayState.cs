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
            AdvanceToPlayer(ctx, 0);
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

        private void AdvanceToPlayer(GameStateData ctx, int playerIndex)
        {
            _activePlayer = playerIndex;
            _selectedHandIndex = -1;

            var playerObj = ctx.Players[playerIndex];
            var board = playerObj.GetComponent<PlayerBoard>();
            var hand = playerObj.GetComponent<PlayerHand>();

            var boardEmpty = IsBoardEmpty(board);
            var handEmpty = hand.Count == 0;

            if (boardEmpty && handEmpty)
            {
                ctx.GoToState(new EndGameState(playerIndex));
                return;
            }

            PromptSelectCard(ctx, playerIndex);
        }

        private void HandleCardSelection(GameStateData ctx)
        {
            var playerObj = ctx.Players[_activePlayer];
            var hand = playerObj.GetComponent<PlayerHand>();
            var board = playerObj.GetComponent<PlayerBoard>();
            var mustPlay = IsBoardEmpty(board);

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

            if (mustPlay)
            {
                Debug.Log($"[Play] Player {_activePlayer + 1} must play at least one card — board is empty!");
                return;
            }

            Debug.Log($"[Play] Player {_activePlayer + 1} passes.");
            ConfirmPlayer(ctx);
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
                ConfirmPlayer(ctx);
                return;
            }
        }

        private void ConfirmPlayer(GameStateData ctx)
        {
            _done[_activePlayer] = true;
            var next = _activePlayer == 0 ? 1 : 0;

            if (!_done[next])
            {
                AdvanceToPlayer(ctx, next);
            }
            else
            {
                Debug.Log("[Play] Both players done. Press SPACE to continue.");
                _waitingForAdvance = true;
            }
        }

        private void PromptSelectCard(GameStateData ctx, int playerIndex)
        {
            var playerObj = ctx.Players[playerIndex];
            var hand = playerObj.GetComponent<PlayerHand>();
            var board = playerObj.GetComponent<PlayerBoard>();
            var mustPlay = IsBoardEmpty(board);

            Debug.Log(
                $"[Play] Player {playerIndex + 1} — pick a card from hand{(mustPlay ? " (MUST play, board is empty)" : "")}:");

            for (var i = 0; i < hand.Count; i++)
            {
                var identity = hand.Cards[i].GetComponent<CardIdentity>();
                Debug.Log($"  [{i + 1}] {identity?.CardName ?? hand.Cards[i].name}");
            }

            Debug.Log(mustPlay ? "  Press 1/2/3 to select." : "  Press 1/2/3 to select, P to pass.");
        }

        private void PrintBoard(GameStateData ctx, int playerIndex)
        {
            var board = ctx.Players[playerIndex].GetComponent<PlayerBoard>();
            Debug.Log($"[Play] Player {playerIndex + 1}'s board:");

            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                var slot = board.GetSlot(i);
                var label = slot != null ? slot.GetComponent<CardIdentity>()?.CardName ?? slot.name : "(empty)";
                Debug.Log($"  [{i + 1}] {label}");
            }
        }

        private bool IsBoardEmpty(PlayerBoard board)
        {
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                if (board.GetSlot(i) != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}