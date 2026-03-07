using System.Collections.Generic;
using CardGame.Abilities;
using CardGame.Abilities.Actions;
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
            "Ironback", "Vexling", "Duskpaw", "Grimthorn", "Ashfang",
            "Coldmere", "Brackus", "Nettleclaw", "Siltmaw", "Wraithkin"
        };

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[EnterGame] Bootstrapping game...");
            SpawnPlayers(ctx);
            Debug.Log("[EnterGame] Ready. Press SPACE to begin.");
        }

        public void OnUpdate(GameStateData ctx)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ctx.GoToState(new DrawState());
            }
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[EnterGame] Starting game.");
        }

        private void SpawnPlayers(GameStateData ctx)
        {
            ctx.Players.Clear();

            for (var i = 0; i < PlayerCount; i++)
            {
                var playerObj = Object.Instantiate(ctx.PlayerPrefab);
                playerObj.name = $"Player{i + 1}";

                var playerData = playerObj.GetComponent<PlayerData>();
                playerData.Initialize($"Player {i + 1}");

                var deck = playerObj.GetComponent<PlayerDeck>();
                for (var j = 0; j < CardsPerDeck; j++)
                {
                    var card = Object.Instantiate(ctx.CardPrefab);
                    card.name = $"Card_P{i + 1}_{j + 1}";
                    RandomiseCard(card);
                    deck.AddCard(card);
                }

                ctx.Players.Add(playerObj);
                Debug.Log($"[EnterGame] Spawned {playerData.PlayerName} with {CardsPerDeck} cards in deck.");
            }
        }

        // TODO: Remove once JSON card loading is implemented.
        private static void RandomiseCard(GameObject card)
        {
            var name = CardNames[Random.Range(0, CardNames.Length)];
            var health = Random.Range(3, 10);
            var speed = Random.Range(1, 6);

            card.GetComponent<CardIdentity>().Initialize(name.ToLower(), name, string.Empty);
            card.GetComponent<HealthData>().Initialize(health);
            card.GetComponent<SpeedData>().Initialize(speed);

            var pipeline = Random.value > 0.5f ? BuildDamageAbility() : BuildHealAbility();
            card.GetComponent<AbilityData>().Initialize(new List<ActionPipeline> { pipeline });

            Debug.Log($"[EnterGame] Card '{name}' — HP:{health} SPD:{speed}");
        }

        private static ActionPipeline BuildDamageAbility()
        {
            var damage = Random.Range(1, 5);
            var wait = Random.Range(0.3f, 1.0f);
            return new ActionPipeline(new List<IAction>
            {
                new WaitAction(wait),
                new DamageAction(damage)
            });
        }

        private static ActionPipeline BuildHealAbility()
        {
            var amount = Random.Range(1, 4);
            var wait = Random.Range(0.3f, 1.0f);
            return new ActionPipeline(new List<IAction>
            {
                new WaitAction(wait),
                new HealAction(amount)
            });
        }
    }
}