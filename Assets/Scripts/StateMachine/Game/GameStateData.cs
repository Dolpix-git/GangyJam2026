using UnityEngine;

namespace CardGame.StateMachine.Game
{
    public class GameStateData : MonoBehaviour
    {
        public GamePhase CurrentPhase { get; set; } = GamePhase.None;
        public bool AdvanceRequested { get; set; } = false;
    }
}