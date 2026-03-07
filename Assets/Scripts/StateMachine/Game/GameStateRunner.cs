using CardGame.StateMachine.Game.States;
using UnityEngine;

namespace CardGame.StateMachine.Game
{
    [RequireComponent(typeof(GameStateData))]
    public class GameStateRunner : MonoBehaviour
    {
        private GameStateData _data;
        private StateMachine<GameStateData> _stateMachine;

        private void Awake()
        {
            _data = GetComponent<GameStateData>();
            _stateMachine = new StateMachine<GameStateData>(_data);
            _data.GoToState = _stateMachine.ChangeState;
            _data.Runner = this;
        }

        private void Start()
        {
            _stateMachine.ChangeState(new EnterGameState());
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}