using CardGame.StateMachine.Game.States;
using UnityEngine;

namespace CardGame.StateMachine.Game
{
    [RequireComponent(typeof(GameStateData))]
    public class GameStateRunner : MonoBehaviour
    {
        private const int LoopBackIndex = 1;

        private readonly IState<GameStateData>[] _phases =
        {
            new EnterGameState(),
            new DrawState(),
            new PlayState(),
            new RetreatState(),
            new ModeState(),
            new BattleState()
        };

        private int _currentPhaseIndex;
        private GameStateData _data;
        private StateMachine<GameStateData> _stateMachine;

        private void Awake()
        {
            _data = GetComponent<GameStateData>();
            _stateMachine = new StateMachine<GameStateData>(_data);
        }

        private void Start()
        {
            _stateMachine.ChangeState(_phases[_currentPhaseIndex]);
        }

        private void Update()
        {
            _stateMachine.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AdvancePhase();
            }
        }

        private void AdvancePhase()
        {
            _currentPhaseIndex++;

            if (_currentPhaseIndex >= _phases.Length)
            {
                _currentPhaseIndex = LoopBackIndex;
            }

            _stateMachine.ChangeState(_phases[_currentPhaseIndex]);
        }
    }
}