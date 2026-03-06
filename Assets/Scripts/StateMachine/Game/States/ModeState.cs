using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class ModeState : IState<GameStateData>
    {
        private int _activePlayer;
        private int _activeSlot;
        private bool _awaitingTargeting;
        private bool _waitingForAdvance;

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Mode] === MODE PHASE ===");
            _activePlayer = 0;
            _activeSlot = 0;
            _awaitingTargeting = false;
            _waitingForAdvance = false;

            ClearAllModes(ctx);
            AdvanceToNextCard(ctx);
        }

        public void OnUpdate(GameStateData ctx)
        {
            if (_waitingForAdvance)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ctx.GoToState(new BattleState());
                }

                return;
            }

            if (_awaitingTargeting)
            {
                HandleTargetingInput(ctx);
            }
            else
            {
                HandleAbilitySelection(ctx);
            }
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Mode] Mode selection complete.");
        }

        private void HandleAbilitySelection(GameStateData ctx)
        {
            var board = ctx.Players[_activePlayer].GetComponent<PlayerBoard>();
            var card = board.GetSlot(_activeSlot);
            var abilities = card.GetComponent<AbilityData>();

            for (var i = 0; i < abilities.AbilityIds.Count; i++)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    continue;
                }

                var selectedAbility = abilities.AbilityIds[i];
                card.GetComponent<CardMode>().SelectedAbilityId = selectedAbility;

                Debug.Log(
                    $"[Mode] Selected ability '{selectedAbility}' for '{card.GetComponent<CardIdentity>()?.CardName ?? card.name}'.");
                Debug.Log("[Mode] Enter targeting info and press ENTER, or press ENTER to skip:");

                _awaitingTargeting = true;
                return;
            }
        }

        private void HandleTargetingInput(GameStateData ctx)
        {
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return;
            }

            var board = ctx.Players[_activePlayer].GetComponent<PlayerBoard>();
            var card = board.GetSlot(_activeSlot);
            var mode = card.GetComponent<CardMode>();

            Debug.Log(!string.IsNullOrEmpty(mode.TargetingData)
                ? $"[Mode] Targeting data set: '{mode.TargetingData}'."
                : "[Mode] No targeting data.");

            _awaitingTargeting = false;
            _activeSlot++;
            AdvanceToNextCard(ctx);
        }

        private void AdvanceToNextCard(GameStateData ctx)
        {
            while (_activePlayer < ctx.Players.Count)
            {
                var board = ctx.Players[_activePlayer].GetComponent<PlayerBoard>();

                while (_activeSlot < PlayerBoard.BoardSize)
                {
                    var card = board.GetSlot(_activeSlot);
                    if (card != null)
                    {
                        PromptAbilitySelection(ctx, _activePlayer, _activeSlot, card);
                        return;
                    }

                    _activeSlot++;
                }

                _activePlayer++;
                _activeSlot = 0;
            }

            Debug.Log("[Mode] All modes selected. Press SPACE to continue.");
            _waitingForAdvance = true;
        }

        private void PromptAbilitySelection(GameStateData ctx, int playerIndex, int slotIndex, GameObject card)
        {
            var identity = card.GetComponent<CardIdentity>();
            var abilities = card.GetComponent<AbilityData>();

            Debug.Log(
                $"[Mode] Player {playerIndex + 1} — '{identity?.CardName ?? card.name}' (slot {slotIndex + 1}). Pick an ability:");

            if (abilities == null || abilities.AbilityIds.Count == 0)
            {
                Debug.Log("  (no abilities) — skipping.");
                _activeSlot++;
                AdvanceToNextCard(ctx);
                return;
            }

            for (var i = 0; i < abilities.AbilityIds.Count; i++)
            {
                Debug.Log($"  [{i + 1}] {abilities.AbilityIds[i]}");
            }
        }

        private void ClearAllModes(GameStateData ctx)
        {
            foreach (var playerObj in ctx.Players)
            {
                var board = playerObj.GetComponent<PlayerBoard>();
                for (var i = 0; i < PlayerBoard.BoardSize; i++)
                {
                    var card = board.GetSlot(i);
                    card?.GetComponent<CardMode>()?.Clear();
                }
            }
        }
    }
}