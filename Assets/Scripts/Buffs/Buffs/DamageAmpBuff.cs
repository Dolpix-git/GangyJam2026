using UnityEngine;

namespace CardGame.Buffs
{
    public class DamageAmpBuff : IBuff
    {
        private int _turnsRemaining;

        public DamageAmpBuff(float multiplier, int turns)
        {
            Multiplier = multiplier;
            _turnsRemaining = turns;
        }

        public float Multiplier { get; }
        public bool IsExpired => _turnsRemaining <= 0;
        public CardTag? ImmunityTag => null;

        public void Tick(GameObject card)
        {
            if (IsExpired)
            {
                return;
            }

            _turnsRemaining--;
            Debug.Log($"[DamageAmp] '{card.name}' has {Multiplier}x damage. ({_turnsRemaining} turns left)");
        }

        public void Dispel()
        {
            _turnsRemaining = 0;
        }
    }
}