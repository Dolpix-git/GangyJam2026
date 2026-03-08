using System.Collections;
using System.Collections.Generic;
using CardGame.Abilities;
using CardGame.Data;
using CardGame.Patterns;
using CardGame.Player;
using UnityEngine;

namespace CardGame.StateMachine.Game.States
{
    public class BattleState : IState<GameStateData>
    {
        private bool _battleRunning;

        public void OnEnter(GameStateData ctx)
        {
            Debug.Log("[Battle] === BATTLE PHASE ===");
            GameStateSingleton.Instance.SetCurrentState(this);
            _battleRunning = true;
            ctx.Runner.StartCoroutine(RunBattle(ctx));
        }

        public void OnUpdate(GameStateData ctx)
        {
        }

        public void OnExit(GameStateData ctx)
        {
            Debug.Log("[Battle] Battle phase complete.");
            GameStateSingleton.Instance.SetCurrentState(null);
            ResetSelectedAbilities(ctx);
        }

        private IEnumerator RunBattle(GameStateData ctx)
        {
            var groups = BuildSpeedGroups(ctx);

            foreach (var group in groups)
            foreach (var entry in group)
            {
                entry.Card.GetComponent<BuffData>()?.TickAll();
            }

            foreach (var group in groups)
            {
                Debug.Log($"[Battle] Executing {group.Count} card(s) at speed {group[0].Speed}.");

                var done = 0;
                var total = group.Count;

                foreach (var entry in group)
                {
                    var buffData = entry.Card.GetComponent<BuffData>();

                    if (buffData != null && buffData.IsParalysed)
                    {
                        done++;
                        continue;
                    }

                    var abilityIndex = entry.Card.GetComponent<CardMode>().SelectedAbilityIndex;
                    if (abilityIndex < 0)
                    {
                        Debug.Log($"[Battle] '{entry.Card.name}' has no ability selected — skipping.");
                        done++;
                        continue;
                    }

                    var abilityData = entry.Card.GetComponent<AbilityData>();
                    if (abilityIndex >= abilityData.Abilities.Count)
                    {
                        Debug.LogWarning(
                            $"[Battle] '{entry.Card.name}' selected ability index {abilityIndex} out of range.");
                        done++;
                        continue;
                    }

                    var actionCtx = new ActionContext
                    {
                        Caster = entry.Card,
                        CasterPlayerIndex = entry.PlayerIndex,
                        CasterSlotIndex = entry.SlotIndex,
                        GameState = ctx,
                        Runner = ctx.Runner,
                        Animator = ctx.CardAnimator
                    };

                    abilityData.RunAbility(abilityIndex, actionCtx, () => done++);
                }

                yield return new WaitUntil(() => done >= total);

                // Pause between each speed group so the player can read what happened.
                if (ctx.CardAnimator != null)
                {
                    var pauseDone = false;
                    ctx.CardAnimator.PlayBetweenAttackPause(() => pauseDone = true);
                    yield return new WaitUntil(() => pauseDone);
                }
            }

            Debug.Log("[Battle] All abilities resolved.");
            ctx.GoToState(new DrawState());
        }

        private static void ResetSelectedAbilities(GameStateData ctx)
        {
            for (var p = 0; p < ctx.Players.Count; p++)
            {
                var board = ctx.Players[p].GetComponent<PlayerBoard>();

                for (var s = 0; s < PlayerBoard.BoardSize; s++)
                {
                    var card = board.GetSlot(s);
                    if (card == null)
                    {
                        continue;
                    }

                    var cardMode = card.GetComponent<CardMode>();
                    if (!cardMode)
                    {
                        Debug.LogError("Could not find CardMode");
                        continue;
                    }

                    cardMode.Clear();
                }
            }
        }

        private static List<List<BattleEntry>> BuildSpeedGroups(GameStateData ctx)
        {
            var entries = new List<BattleEntry>();

            for (var p = 0; p < ctx.Players.Count; p++)
            {
                var board = ctx.Players[p].GetComponent<PlayerBoard>();
                for (var s = 0; s < PlayerBoard.BoardSize; s++)
                {
                    var card = board.GetSlot(s);
                    if (card == null)
                    {
                        continue;
                    }

                    var speed = card.GetComponent<SpeedData>()?.CurrentSpeed ?? 0;
                    entries.Add(new BattleEntry(card, p, s, speed));
                }
            }

            entries.Sort((a, b) => b.Speed.CompareTo(a.Speed));

            var groups = new List<List<BattleEntry>>();
            var i = 0;
            while (i < entries.Count)
            {
                var group = new List<BattleEntry> { entries[i] };
                var j = i + 1;
                while (j < entries.Count && entries[j].Speed == entries[i].Speed)
                {
                    group.Add(entries[j++]);
                }

                groups.Add(group);
                i = j;
            }

            return groups;
        }

        private class BattleEntry
        {
            public readonly GameObject Card;
            public readonly int PlayerIndex;
            public readonly int SlotIndex;
            public readonly int Speed;

            public BattleEntry(GameObject card, int playerIndex, int slotIndex, int speed)
            {
                Card = card;
                PlayerIndex = playerIndex;
                SlotIndex = slotIndex;
                Speed = speed;
            }
        }
    }
}