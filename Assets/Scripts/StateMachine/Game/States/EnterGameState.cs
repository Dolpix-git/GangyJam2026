using System.Collections.Generic;
using CardGame.Card;
using CardGame.Player;
using CardGame.Run;
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
            var run = RunContext.Instance;
            var playerCards = run != null ? run.PlayerCardIds : null;
            var enemyCards = run != null ? run.EnemyCardIds : null;

            for (var i = 0; i < ctx.Players.Count; i++)
            {
                var playerObj = ctx.Players[i];
                playerObj.name = $"Player{i + 1}";
                playerObj.GetComponent<PlayerData>().Initialize($"Player {i + 1}");

                var deck = playerObj.GetComponent<PlayerDeck>();
                var cardIds = i == 0
                    ? playerCards ?? new List<string>(_devCards)
                    : enemyCards ?? new List<string>(_devCards);

                foreach (var cardId in cardIds)
                {
                    var card = CardFactory.Instance.CreateCard(cardId);
                    if (card != null)
                    {
                        deck.AddCard(card);
                    }
                }

                Debug.Log($"[EnterGame] Player {i + 1} initialized with {cardIds.Count} cards.");
            }
        }
    }
}