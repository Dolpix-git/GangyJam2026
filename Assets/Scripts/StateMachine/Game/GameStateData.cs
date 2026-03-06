using UnityEngine;

namespace CardGame.StateMachine.Game
{
    /// <summary>
    ///     Game state entity component. Stores shared game state data.
    /// </summary>
    public class GameStateData : MonoBehaviour
    {
        public GamePhase CurrentPhase { get; set; } = GamePhase.None;
        public bool AdvanceRequested { get; set; } = false;
    }
}