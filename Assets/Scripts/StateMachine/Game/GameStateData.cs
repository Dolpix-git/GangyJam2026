using System;
using System.Collections.Generic;
using CardGame.Player.Controllers;
using UnityEngine;

namespace CardGame.StateMachine.Game
{
    public class GameStateData : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] public GameObject PlayerPrefab;

        [SerializeField] public GameObject CardPrefab;
        [SerializeField] private List<GameObject> _players = new();

        [Header("Runtime")]
        public List<GameObject> Players => _players;

        public Action<IState<GameStateData>> GoToState { get; set; }
        public MonoBehaviour Runner { get; set; }

        public IPlayerController[] Controllers { get; set; }
    }
}