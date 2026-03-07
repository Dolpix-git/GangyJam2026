using System.Collections.Generic;
using CardGame.Player;
using UnityEngine;

namespace CardGame.Abilities
{
    public static class ActionTargeting
    {
        public static List<GameObject> Resolve(ActionContext ctx, TargetSlot slot)
        {
            return slot switch
            {
                TargetSlot.Self => Single(ctx.Caster),
                TargetSlot.FriendlyLeft => Single(GetFriendlySlot(ctx, ctx.CasterSlotIndex - 1)),
                TargetSlot.FriendlyRight => Single(GetFriendlySlot(ctx, ctx.CasterSlotIndex + 1)),
                TargetSlot.FriendlyBoth => FriendlyFlanks(ctx),
                TargetSlot.EnemyOpposing => Single(GetOpposingCard(ctx)),
                TargetSlot.EnemyLeft => Single(GetEnemySlot(ctx, ctx.CasterSlotIndex - 1)),
                TargetSlot.EnemyRight => Single(GetEnemySlot(ctx, ctx.CasterSlotIndex + 1)),
                TargetSlot.EnemyBoth => GetFlankingCards(ctx),
                _ => new List<GameObject>()
            };
        }

        public static GameObject GetOpposingCard(ActionContext ctx)
        {
            return GetEnemyBoard(ctx)?.GetSlot(ctx.CasterSlotIndex);
        }

        public static List<GameObject> GetFlankingCards(ActionContext ctx)
        {
            var board = GetEnemyBoard(ctx);
            if (board == null)
            {
                return new List<GameObject>();
            }

            var targets = new List<GameObject>();
            TryAddSlot(targets, board, ctx.CasterSlotIndex - 1);
            TryAddSlot(targets, board, ctx.CasterSlotIndex + 1);
            return targets;
        }

        private static List<GameObject> FriendlyFlanks(ActionContext ctx)
        {
            var board = GetFriendlyBoard(ctx);
            if (board == null)
            {
                return new List<GameObject>();
            }

            var targets = new List<GameObject>();
            TryAddSlot(targets, board, ctx.CasterSlotIndex - 1);
            TryAddSlot(targets, board, ctx.CasterSlotIndex + 1);
            return targets;
        }

        private static GameObject GetEnemySlot(ActionContext ctx, int index)
        {
            return IsValidSlot(index) ? GetEnemyBoard(ctx)?.GetSlot(index) : null;
        }

        private static GameObject GetFriendlySlot(ActionContext ctx, int index)
        {
            return IsValidSlot(index) ? GetFriendlyBoard(ctx)?.GetSlot(index) : null;
        }

        private static void TryAddSlot(List<GameObject> list, PlayerBoard board, int index)
        {
            if (!IsValidSlot(index))
            {
                return;
            }

            var card = board.GetSlot(index);
            if (card != null)
            {
                list.Add(card);
            }
        }

        private static bool IsValidSlot(int index)
        {
            return index >= 0 && index < PlayerBoard.BoardSize;
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

        private static PlayerBoard GetFriendlyBoard(ActionContext ctx)
        {
            if (ctx.GameState == null)
            {
                return null;
            }

            return ctx.GameState.Players[ctx.CasterPlayerIndex].GetComponent<PlayerBoard>();
        }

        private static List<GameObject> Single(GameObject card)
        {
            var list = new List<GameObject>();
            if (card != null)
            {
                list.Add(card);
            }

            return list;
        }
    }
}