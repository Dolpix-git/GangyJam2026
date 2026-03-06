using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class DrawState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
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

            Debug.Log("[Draw] Press SPACE to continue.");
        }

        public void OnUpdate(GameStateData ctx)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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