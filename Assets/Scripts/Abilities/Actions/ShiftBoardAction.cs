using System;
using CardGame.Player;
using Newtonsoft.Json;

namespace CardGame.Abilities.Actions
{
    public class ShiftBoardAction : IAction
    {
        [JsonProperty] private ShiftTarget _target;

        public string Description => $"Shift the {ActionEnumNames.Of(_target)}.";

        public void Execute(ActionContext ctx, Action onComplete)
        {
            if (ctx.GameState == null)
            {
                onComplete();
                return;
            }

            var playerIndex = _target switch
            {
                ShiftTarget.EnemyLeft => ctx.CasterPlayerIndex == 0 ? 1 : 0,
                ShiftTarget.EnemyRight => ctx.CasterPlayerIndex == 0 ? 1 : 0,
                ShiftTarget.FriendlyLeft => ctx.CasterPlayerIndex,
                ShiftTarget.FriendlyRight => ctx.CasterPlayerIndex,
                _ => -1
            };

            if (playerIndex < 0)
            {
                onComplete();
                return;
            }

            var board = ctx.GameState.Players[playerIndex].GetComponent<PlayerBoard>();

            switch (_target)
            {
                case ShiftTarget.EnemyLeft:
                case ShiftTarget.FriendlyLeft:
                    board.ShiftLeft();
                    break;
                case ShiftTarget.EnemyRight:
                case ShiftTarget.FriendlyRight:
                    board.ShiftRight();
                    break;
            }

            onComplete();
        }
    }
}