using CardGame.Card;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class EnterGameState : IState<GameStateData>
    {
        private static readonly string[] _devCards = { "001_Concept", "002_Concept", "003_Concept" };

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[EnterGame] Bootstrapping game...");
            SpawnPlayers(ctx);
            Debug.Log("[EnterGame] Ready. Press SPACE to begin.");
        }

        public void OnUpdate(GameStateData ctx)
        {
            ctx.GoToState(new DrawState());
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[EnterGame] Starting game.");
        }

        private void SpawnPlayers(GameStateData ctx)
        {
            for (var i = 0; i < ctx.Players.Count; i++)
            {
                var playerObj = ctx.Players[i];
                playerObj.name = $"Player{i + 1}";

                var playerData = playerObj.GetComponent<PlayerData>();
                playerData.Initialize($"Player {i + 1}");

                var deck = playerObj.GetComponent<PlayerDeck>();
                foreach (var cardName in _devCards)
                {
                    var newCard = CardFactory.Instance.CreateCard(cardName);
                    deck.AddCard(newCard);
                }

                Debug.Log($"[EnterGame] Initialized {playerData.PlayerName} with {_devCards.Length} cards in deck.");
            }
        }
    }
}