using CardGame.Data;
using UnityEngine;

namespace CardGame.Buffs
{
    public class FrostBuff : IBuff
    {
        private readonly int _damagePerTurn;
        private int _turnsRemaining;

        public FrostBuff(int damagePerTurn, int turns)
        {
            _damagePerTurn = damagePerTurn;
            _turnsRemaining = turns;
        }

        public bool IsExpired => _turnsRemaining <= 0;
        public CardTag? ImmunityTag => CardTag.ImmuneFrost;

        public void Tick(GameObject card)
        {
            if (IsExpired)
            {
                return;
            }

            _turnsRemaining--;
            card.GetComponent<IDamageable>()?.TakeDamage(_damagePerTurn);
            Debug.Log($"[Frost] '{card.name}' takes {_damagePerTurn} frost damage. ({_turnsRemaining} turns left)");
        }

        public void Dispel()
        {
            _turnsRemaining = 0;
        }
    }
}