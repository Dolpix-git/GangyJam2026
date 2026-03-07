using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class DrawState : IState<GameStateData>
    {
        private float _timer;
        private const float DrawDuration = 2f;

        public void OnEnter(GameStateData ctx)
        {
            _timer = 0f;
            Debug.Log("[Draw] === DRAW PHASE ===");

            foreach (var playerObj in ctx.Players)
            {
                var deck = playerObj.GetComponent<PlayerDeck>();
                var hand = playerObj.GetComponent<PlayerHand>();
                var playerData = playerObj.GetComponent<PlayerData>();

                var card = deck.DrawTop();
                if (card != null)
                {
                    hand.AddCard(card);
                    var identity = card.GetComponent<CardIdentity>();
                    Debug.Log($"[Draw] {playerData.PlayerName} drew '{identity?.CardName ?? card.name}'. Hand size: {hand.Count}");
                }
                else
                {
                    Debug.Log($"[Draw] {playerData.PlayerName}'s deck is empty, nothing to draw.");
                }
            }

            Debug.Log("[Draw] Advancing in 2 seconds...");
        }

        public void OnUpdate(GameStateData ctx)
        {
            _timer += Time.deltaTime;
            if (_timer >= DrawDuration)
            {
                ctx.GoToState(new PlayState());
            }
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Draw] Draw phase complete.");
        }
    }
}