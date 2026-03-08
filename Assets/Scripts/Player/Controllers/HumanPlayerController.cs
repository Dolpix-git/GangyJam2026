using System;
using System.Linq;
using CardGame.Card.Components;
using CardGame.Data;
using CardGame.StateMachine.Game;
using CardGame.UI.CardDrop;
using Events;
using UI.ModelView.Models;
using UnityEngine;

namespace CardGame.Player.Controllers
{
    public class HumanPlayerController : MonoBehaviour, IPlayerController
    {
        private Action _onDone;
        private Action _updateAction;

        private void Update()
        {
            _updateAction?.Invoke();
        }

        public void DoPlayPhase(GameStateData ctx, int playerIndex, Action onDone)
        {
            var playerObj = ctx.Players[playerIndex];
            var hand = playerObj.GetComponent<PlayerHand>();
            var board = playerObj.GetComponent<PlayerBoard>();
            var mustPlay = IsBoardEmpty(board);
            var selectedHandIndex = -1;

            Debug.Log($"[Play] Player {playerIndex + 1} — pick a card{(mustPlay ? " (MUST play)" : "")}:");
            LogHand(hand);
            Debug.Log(mustPlay ? "  1/2/3 to select." : "  1/2/3 to select, P to pass.");

            void BeginDrag(CardBeginDragEvent evt)
            {
                var cardUiObj = evt.DragHandler.GetComponent<ModelViewCard>();
                var cardModel = cardUiObj.Model;
                selectedHandIndex = hand.Cards.ToList().IndexOf(cardModel);
            }

            void ReactToPlay(CardDropOnSlotEvent evt)
            {
                if (selectedHandIndex == -1)
                {
                    Debug.Log("[Play] No card selected.");
                    return;
                }

                if (evt.DropSlot is not FieldCardDropSlot fieldSlot)
                {
                    Debug.LogError("This is not a field drop slot.");
                    selectedHandIndex = -1;
                    return;
                }

                int slotIndex = fieldSlot.SlotIndex;

                if (board.GetSlot(slotIndex) != null)
                {
                    Debug.Log($"[Play] Slot {slotIndex + 1} occupied.");
                    selectedHandIndex = -1;
                    return;
                }

                var card = hand.RemoveAt(selectedHandIndex);
                board.TryPlaceCard(card, slotIndex);
                card.GetComponent<CardDeathSystem>().Initialize(playerObj);

                Debug.Log(
                    $"[Play] Player {playerIndex + 1} placed '{card.GetComponent<CardIdentity>()?.CardName}' in slot {slotIndex + 1}.");

                EventBus.Unsubscribe<CardDropOnSlotEvent>(ReactToPlay);
                EventBus.Unsubscribe<CardBeginDragEvent>(BeginDrag);
                Finish(onDone);
            }

            EventBus.Subscribe<CardDropOnSlotEvent>(ReactToPlay);
            EventBus.Subscribe<CardBeginDragEvent>(BeginDrag);

            _updateAction = () =>
            {
                if (selectedHandIndex == -1)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                        {
                            continue;
                        }

                        if (i >= hand.Count)
                        {
                            Debug.Log($"[Play] No card at [{i + 1}].");
                            return;
                        }

                        selectedHandIndex = i;
                        Debug.Log(
                            $"[Play] Selected '{hand.Cards[i].GetComponent<CardIdentity>()?.CardName}'. Pick a slot:");
                        LogBoard(board);
                        return;
                    }

                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        if (mustPlay)
                        {
                            Debug.Log("[Play] Must play — board is empty!");
                            return;
                        }

                        Finish(onDone);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        selectedHandIndex = -1;
                        return;
                    }

                    for (var i = 0; i < 3; i++)
                    {
                        if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                        {
                            continue;
                        }

                        if (board.GetSlot(i) != null)
                        {
                            Debug.Log($"[Play] Slot {i + 1} occupied.");
                            return;
                        }

                        var card = hand.RemoveAt(selectedHandIndex);
                        board.TryPlaceCard(card, i);
                        card.GetComponent<CardDeathSystem>().Initialize(playerObj);
                        Debug.Log(
                            $"[Play] Player {playerIndex + 1} placed '{card.GetComponent<CardIdentity>()?.CardName}' in slot {i + 1}.");
                        Finish(onDone);
                        return;
                    }
                }
            };
        }

        public void DoRetreatPhase(GameStateData ctx, int playerIndex, Action onDone)
        {
            var playerObj = ctx.Players[playerIndex];
            var board = playerObj.GetComponent<PlayerBoard>();
            var hand = playerObj.GetComponent<PlayerHand>();
            var selectedCardBoardIndex = -1;


            LogRetreatPrompt(playerIndex, board);
            
            void BeginDrag(CardBeginDragEvent evt)
            {
                var cardUiObj = evt.DragHandler.GetComponent<ModelViewCard>();
                var cardModel = cardUiObj.Model;
                selectedCardBoardIndex = board.GetIndex(cardModel);
            }
            
            void ReactToRetreat(CardDropOnSlotEvent evt)
            {
                if (CardCount(board) == 1)
                {
                    Debug.Log("[Retreat] Need at least one card.");
                    return;
                }
                
                if (selectedCardBoardIndex == -1)
                {
                    Debug.Log("[Play] No card selected.");
                    return;
                }

                if (evt.DropSlot is not HandCardDropSlot handSlot)
                {
                    Debug.LogError("This is not a hand drop slot.");
                    selectedCardBoardIndex = -1;
                    return;
                }

                var card = board.GetSlot(selectedCardBoardIndex);
                if (card == null)
                {
                    Debug.Log($"[Retreat] Slot {selectedCardBoardIndex + 1} empty.");
                    return;
                }

                board.RemoveAt(selectedCardBoardIndex);
                hand.AddCard(card);
                Debug.Log(
                    $"[Retreat] Player {playerIndex + 1} retreated '{card.GetComponent<CardIdentity>()?.CardName}' from slot {selectedCardBoardIndex + 1}.");
                LogRetreatPrompt(playerIndex, board);
                
                EventBus.Unsubscribe<CardDropOnSlotEvent>(ReactToRetreat);
                EventBus.Unsubscribe<CardBeginDragEvent>(BeginDrag);
                Finish(onDone);
            }
            
            EventBus.Subscribe<CardDropOnSlotEvent>(ReactToRetreat);
            EventBus.Subscribe<CardBeginDragEvent>(BeginDrag);

            _updateAction = () =>
            {
                for (var i = 0; i < PlayerBoard.BoardSize; i++)
                {
                    if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                    {
                        continue;
                    }

                    var card = board.GetSlot(i);
                    if (card == null)
                    {
                        Debug.Log($"[Retreat] Slot {i + 1} empty.");
                        return;
                    }

                    if (CardCount(board) == 1)
                    {
                        Debug.Log("[Retreat] Need at least one card.");
                        return;
                    }

                    board.RemoveAt(i);
                    hand.AddCard(card);
                    Debug.Log(
                        $"[Retreat] Player {playerIndex + 1} retreated '{card.GetComponent<CardIdentity>()?.CardName}' from slot {i + 1}.");
                    LogRetreatPrompt(playerIndex, board);
                    return;
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    Finish(onDone);
                }
            };
        }

        public void DoModePhase(GameStateData ctx, int playerIndex, Action onDone)
        {
            var board = ctx.Players[playerIndex].GetComponent<PlayerBoard>();
            var slot = 0;

            void AdvanceSlot()
            {
                while (slot < PlayerBoard.BoardSize)
                {
                    var card = board.GetSlot(slot);
                    if (card == null)
                    {
                        slot++;
                        continue;
                    }

                    var abilities = card.GetComponent<AbilityData>();
                    if (abilities == null || abilities.Abilities.Count == 0)
                    {
                        Debug.Log(
                            $"[Mode] '{card.GetComponent<CardIdentity>()?.CardName}' has no abilities — skipping.");
                        slot++;
                        continue;
                    }

                    Debug.Log(
                        $"[Mode] Player {playerIndex + 1} — '{card.GetComponent<CardIdentity>()?.CardName}' (slot {slot + 1}). Pick ability:");
                    for (var i = 0; i < abilities.Abilities.Count; i++)
                    {
                        Debug.Log($"  [{i + 1}] {abilities.Abilities[i].Name}");
                    }

                    Debug.Log("  ENTER to confirm.");
                    return;
                }

                Finish(onDone);
            }

            AdvanceSlot();

            _updateAction = () =>
            {
                var card = board.GetSlot(slot);
                if (card == null)
                {
                    return;
                }

                var abilities = card.GetComponent<AbilityData>();

                for (var i = 0; i < abilities.Abilities.Count; i++)
                {
                    if (!Input.GetKeyDown(KeyCode.Alpha1 + i))
                    {
                        continue;
                    }

                    card.GetComponent<CardMode>().SelectedAbilityIndex = i;
                    Debug.Log($"[Mode] Ability {i} selected. Press ENTER to confirm.");
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    slot++;
                    AdvanceSlot();
                }
            };
        }

        private void Finish(Action onDone)
        {
            _updateAction = null;
            onDone();
        }

        private static bool IsBoardEmpty(PlayerBoard board)
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

        private static void LogHand(PlayerHand hand)
        {
            for (var i = 0; i < hand.Count; i++)
            {
                Debug.Log($"  [{i + 1}] {hand.Cards[i].GetComponent<CardIdentity>()?.CardName ?? hand.Cards[i].name}");
            }
        }

        private static void LogBoard(PlayerBoard board)
        {
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                var slot = board.GetSlot(i);
                Debug.Log(
                    $"  [{i + 1}] {(slot != null ? slot.GetComponent<CardIdentity>()?.CardName ?? slot.name : "(empty)")}");
            }
        }

        private static void LogRetreatPrompt(int playerIndex, PlayerBoard board)
        {
            Debug.Log($"[Retreat] Player {playerIndex + 1}'s board:");
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                var slot = board.GetSlot(i);
                if (slot != null)
                {
                    Debug.Log($"  [{i + 1}] {slot.GetComponent<CardIdentity>()?.CardName ?? slot.name}");
                }
            }

            Debug.Log("  1/2/3 to retreat, P to pass.");
        }
    }
}