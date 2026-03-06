using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class EndGameState : IState<GameStateData>
    {
        private readonly int _losingPlayerIndex;

        public EndGameState(int losingPlayerIndex)
        {
            _losingPlayerIndex = losingPlayerIndex;
        }

        public void OnEnter(GameStateData ctx)
        {
            var loser = ctx.Players[_losingPlayerIndex].GetComponent<PlayerData>();
            var winnerIndex = _losingPlayerIndex == 0 ? 1 : 0;
            var winner = ctx.Players[winnerIndex].GetComponent<PlayerData>();

            Debug.Log("[EndGame] === GAME OVER ===");
            Debug.Log($"[EndGame] {loser.PlayerName} cannot play a card and has lost!");
            Debug.Log($"[EndGame] {winner.PlayerName} wins!");
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
        }
    }
}