using System;
using CardGame.StateMachine.Game;

namespace CardGame.Player.Controllers
{
    public interface IPlayerController
    {
        void DoPlayPhase(GameStateData ctx, int playerIndex, Action onDone);
        void DoRetreatPhase(GameStateData ctx, int playerIndex, Action onDone);
        void DoModePhase(GameStateData ctx, int playerIndex, Action onDone);
    }
}