using CardGame.Patterns;
using CardGame.Player;
using CardGame.Run;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame.StateMachine.Game.States
{
    public class EndGameState : IState<GameStateData>
    {
        private float _timer;
        private const float EndGameDuration = 5f;
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
            
            GameStateSingleton.Instance.SetCurrentState(this);

            Debug.Log("[EndGame] GAME OVER");
            Debug.Log($"[EndGame] {loser.PlayerName} has lost! {winner.PlayerName} wins!");

            if (winnerIndex == 0)
            {
                Debug.Log($"[EndGame] +{RunContext.CoinsPerWin} coins awarded. Total: {RunContext.Instance.Coins}");
            }
            
            EventBus.Publish(new EndGameEvent(winnerIndex == 0));
        }

        public void OnUpdate(GameStateData ctx)
        {
            _timer += Time.deltaTime;
            if (_timer >= EndGameDuration)
            {
                ctx.GoToState(null);
            }
        }

        public void OnExit(GameStateData ctx)
        {
            GameStateSingleton.Instance.SetCurrentState(null);
            
            var winnerIndex = _losingPlayerIndex == 0 ? 1 : 0;
            if (winnerIndex == 0)
            {
                RunContext.Instance.AwardWinCoins();
                RunContext.Instance.Save();
                SceneManager.LoadScene(SceneNames.Explore);
            }
            else
            {
                SaveSystem.DeleteSave();
                SceneManager.LoadScene(SceneNames.MainMenu);
            }
        }
    }
}