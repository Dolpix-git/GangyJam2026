using System.Collections.Generic;
using CardGame.Abilities;
using CardGame.Abilities.Actions;
using CardGame.Card;
using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class EnterGameState : IState<GameStateData>
    {
        private const int PlayerCount = 2;
        private const int CardsPerDeck = 3;

        private static readonly string[] CardNames =
        {
            "SnowFlake", "IccCubeWorm", "SnowFox", "PineNeedle",
            "Gomdorr", "Rogdolf", "SnowGolem", "Stimta"
        };

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
                for (var j = 0; j < CardsPerDeck; j++)
                {
                    var newCard = CardFactory.Instance.CreateCard("ExampleCard");
                    deck.AddCard(newCard);

                    //ToDo Implement loading cards into the player deck
                }
                
                Debug.Log($"[EnterGame] Initialized {playerData.PlayerName} with {CardsPerDeck} cards in deck.");
            }
        }
    }
}