using UnityEngine;

namespace CardGame.Buffs
{
    public class ParalysisBuff : IBuff
    {
        private int _turnsRemaining;

        public ParalysisBuff(int turns)
        {
            _turnsRemaining = turns;
        }

        public bool IsExpired => _turnsRemaining <= 0;
        public CardTag? ImmunityTag => CardTag.ImmuneParalysis;

        public void Tick(GameObject card)
        {
            if (IsExpired)
            {
                return;
            }

            _turnsRemaining--;
            Debug.Log($"[Paralysis] '{card.name}' cannot act. ({_turnsRemaining} turns left)");
        }

        public void Dispel()
        {
            _turnsRemaining = 0;
        }
    }
}