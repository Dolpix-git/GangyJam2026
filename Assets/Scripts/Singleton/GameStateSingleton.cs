using System;
using CardGame.StateMachine;
using CardGame.StateMachine.Game;

namespace CardGame.Patterns
{
    public class GameStateSingleton: MonoSingleton<GameStateSingleton>
    {
        private IState<GameStateData> _currentState = null;
 
        public IState<GameStateData> CurrentState => _currentState;

        public event Action CurrentStateChanged;

        public void SetCurrentState(IState<GameStateData> newState)
        {
            _currentState = newState;
            CurrentStateChanged?.Invoke();
        }
    }
}