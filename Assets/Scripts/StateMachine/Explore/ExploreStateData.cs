using System;
using CardGame.Run;
using UnityEngine;

namespace CardGame.StateMachine.Explore
{
    public class ExploreStateData : MonoBehaviour
    {
        public Action<IState<ExploreStateData>> GoToState { get; set; }
        public RunContext Run => RunContext.Instance;
    }
}