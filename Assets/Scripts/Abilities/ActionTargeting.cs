using System.Collections.Generic;
using CardGame.Player;
using UnityEngine;

namespace CardGame.Abilities
{
    public static class ActionTargeting
    {
        public static GameObject GetOpposingCard(ActionContext ctx)
        {
            var board = GetEnemyBoard(ctx);
            return board?.GetSlot(ctx.CasterSlotIndex);
        }

        public static List<GameObject> GetFlankingCards(ActionContext ctx)
        {
            var board = GetEnemyBoard(ctx);
            if (board == null)
            {
                return new List<GameObject>();
            }

            var targets = new List<GameObject>();
            var left = ctx.CasterSlotIndex - 1;
            var right = ctx.CasterSlotIndex + 1;

            if (left >= 0)
            {
                var card = board.GetSlot(left);
                if (card != null)
                {
                    targets.Add(card);
                }
            }

            if (right < PlayerBoard.BoardSize)
            {
                var card = board.GetSlot(right);
                if (card != null)
                {
                    targets.Add(card);
                }
            }

            return targets;
        }

        private static PlayerBoard GetEnemyBoard(ActionContext ctx)
        {
            if (ctx.GameState == null)
            {
                return null;
            }

            var enemyIndex = ctx.CasterPlayerIndex == 0 ? 1 : 0;
            return ctx.GameState.Players[enemyIndex].GetComponent<PlayerBoard>();
        }
    }
}