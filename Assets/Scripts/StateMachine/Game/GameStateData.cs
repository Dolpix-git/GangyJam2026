using System;
using UnityEngine;

namespace CardGame.StateMachine.Game
{
    public class GameStateData : MonoBehaviour
    {
        public Action<IState<GameStateData>> GoToState { get; set; }
    }
}