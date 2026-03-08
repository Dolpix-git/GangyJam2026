using CardGame.StateMachine.Explore.States;
using UnityEngine;

namespace CardGame.StateMachine.Explore
{
    [RequireComponent(typeof(ExploreStateData))]
    public class ExploreStateRunner : MonoBehaviour
    {
        private ExploreStateData _data;
        private StateMachine<ExploreStateData> _stateMachine;

        private void Awake()
        {
            _data = GetComponent<ExploreStateData>();
            _stateMachine = new StateMachine<ExploreStateData>(_data);
            _data.GoToState = _stateMachine.ChangeState;
        }

        private void Start()
        {
            _stateMachine.ChangeState(new ExploreBuyState());
        }

        private void Update()
        {
            _stateMachine.Update();
        }
    }
}