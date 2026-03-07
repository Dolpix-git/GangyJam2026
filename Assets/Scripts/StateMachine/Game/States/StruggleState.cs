using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class StruggleState : IState<GameStateData>
    {
        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Struggle] === STRUGGLE PHASE ===");
            ApplyStruggleDamage(ctx);
            ctx.GoToState(new ModeState());
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Struggle] Struggle phase complete.");
        }

        private void ApplyStruggleDamage(GameStateData ctx)
        {
            foreach (var playerObj in ctx.Players)
            {
                var board = playerObj.GetComponent<PlayerBoard>();
                for (var i = 0; i < PlayerBoard.BoardSize; i++)
                {
                    var card = board.GetSlot(i);
                    if (card == null)
                    {
                        continue;
                    }

                    var abilityData = card.GetComponent<AbilityData>();
                    if (abilityData == null || !abilityData.IsStruggling)
                    {
                        continue;
                    }

                    var struggleData = card.GetComponent<StruggleData>();
                    var health = card.GetComponent<IHealth>() as IDamageable;

                    if (struggleData == null || health == null)
                    {
                        continue;
                    }

                    var identity = card.GetComponent<CardIdentity>();
                    Debug.Log(
                        $"[Struggle] '{identity?.CardName ?? card.name}' is struggling — taking {struggleData.StruggleDamage} damage.");
                    health.TakeDamage(struggleData.StruggleDamage);
                }
            }
        }
    }
}