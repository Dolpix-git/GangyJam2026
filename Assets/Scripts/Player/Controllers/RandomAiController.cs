using System;
using CardGame.Card.Components;
using CardGame.Data;
using CardGame.StateMachine.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Player.Controllers
{
    public class RandomAiController : IPlayerController
    {
        public void DoPlayPhase(GameStateData ctx, int playerIndex, Action onDone)
        {
            var playerObj = ctx.Players[playerIndex];
            var hand = playerObj.GetComponent<PlayerHand>();
            var board = playerObj.GetComponent<PlayerBoard>();

            var mustPlay = IsBoardEmpty(board);

            if (hand.Count == 0 || (!mustPlay && Random.value < 0.3f))
            {
                Debug.Log($"[AI {playerIndex + 1}] Passes play phase.");
                onDone();
                return;
            }

            var handIndex = Random.Range(0, hand.Count);
            var slotIndex = FindEmptySlot(board);

            if (slotIndex == -1)
            {
                Debug.Log($"[AI {playerIndex + 1}] Board full, passes.");
                onDone();
                return;
            }

            var card = hand.RemoveAt(handIndex);
            board.TryPlaceCard(card, slotIndex);
            card.GetComponent<CardDeathSystem>().Initialize(playerObj);

            var id = card.GetComponent<CardIdentity>();
            Debug.Log($"[AI {playerIndex + 1}] Placed '{id?.CardName ?? card.name}' in slot {slotIndex + 1}.");
            onDone();
        }

        public void DoRetreatPhase(GameStateData ctx, int playerIndex, Action onDone)
        {
            var board = ctx.Players[playerIndex].GetComponent<PlayerBoard>();
            var hand = ctx.Players[playerIndex].GetComponent<PlayerHand>();

            if (CardCount(board) > 1 && Random.value < 0.3f)
            {
                var slotIndex = FindOccupiedSlot(board);
                var card = board.RemoveAt(slotIndex);
                hand.AddCard(card);
                var id = card.GetComponent<CardIdentity>();
                Debug.Log($"[AI {playerIndex + 1}] Retreated '{id?.CardName ?? card.name}' from slot {slotIndex + 1}.");
            }
            else
            {
                Debug.Log($"[AI {playerIndex + 1}] Passes retreat phase.");
            }

            onDone();
        }

        public void DoModePhase(GameStateData ctx, int playerIndex, Action onDone)
        {
            var board = ctx.Players[playerIndex].GetComponent<PlayerBoard>();

            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                var card = board.GetSlot(i);
                if (card == null)
                {
                    continue;
                }

                var abilities = card.GetComponent<AbilityData>();
                if (abilities == null || abilities.Abilities.Count == 0)
                {
                    continue;
                }

                var abilityIndex = Random.Range(0, abilities.Abilities.Count);
                card.GetComponent<CardMode>().SelectAbility(abilityIndex);

                var id = card.GetComponent<CardIdentity>();
                Debug.Log(
                    $"[AI {playerIndex + 1}] '{id?.CardName ?? card.name}' uses ability {abilityIndex} ({abilities.Abilities[abilityIndex].Name}).");
            }

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

        private static int FindEmptySlot(PlayerBoard board)
        {
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                if (board.GetSlot(i) == null)
                {
                    return i;
                }
            }

            return -1;
        }

        private static int FindOccupiedSlot(PlayerBoard board)
        {
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                if (board.GetSlot(i) != null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}