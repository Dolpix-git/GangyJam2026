using CardGame.Player.Controllers;
using CardGame.StateMachine.Game.States;
using UnityEngine;

namespace CardGame.StateMachine.Game
{
    public enum ControllerType
    {
        Human,
        RandomAI
    }

    [RequireComponent(typeof(GameStateData))]
    public class GameStateRunner : MonoBehaviour
    {
        [SerializeField] private ControllerType _player1Controller = ControllerType.Human;
        [SerializeField] private ControllerType _player2Controller = ControllerType.RandomAI;

        private GameStateData _data;
        private StateMachine<GameStateData> _stateMachine;

        private void Awake()
        {
            _data = GetComponent<GameStateData>();
            _stateMachine = new StateMachine<GameStateData>(_data);
            _data.GoToState = _stateMachine.ChangeState;
            _data.Runner = this;
            _data.Controllers = new[]
            {
                CreateController(_player1Controller),
                CreateController(_player2Controller)
            };
        }

        private void Start()
        {
            _stateMachine.ChangeState(new EnterGameState());
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private IPlayerController CreateController(ControllerType type)
        {
            return type switch
            {
                ControllerType.RandomAI => new RandomAiController(),
                _ => gameObject.AddComponent<HumanPlayerController>()
            };
        }
    }
}