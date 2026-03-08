using CardGame.StateMachine;
using CardGame.StateMachine.Game;

namespace CardGame.Patterns
{
    public class GameStateSingleton: MonoSingleton<GameStateSingleton>
    {
        public IState<GameStateData> CurrentState = null;
    }
}