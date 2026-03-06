using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.StateMachine.Game
{
    public class GameStateData : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] public GameObject playerPrefab;
        [SerializeField] public GameObject cardPrefab;

        [Header("Runtime")]
        public List<GameObject> Players { get; } = new();

        public Action<IState<GameStateData>> GoToState { get; set; }
    }
}
