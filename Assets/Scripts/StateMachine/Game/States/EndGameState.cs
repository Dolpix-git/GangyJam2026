using CardGame.Player;
using CardGame.Run;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            Debug.Log("[EndGame] GAME OVER");
            Debug.Log($"[EndGame] {loser.PlayerName} has lost! {winner.PlayerName} wins!");

            if (winnerIndex == 0)
            {
                RunContext.Instance.Save();
                SceneManager.LoadScene(SceneNames.Explore);
            }
            else
            {
                SaveSystem.DeleteSave();
                SceneManager.LoadScene(SceneNames.MainMenu);
            }
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
        }
    }
}