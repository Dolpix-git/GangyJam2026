using UnityEngine;

namespace CardGame.Buffs
{
    public class ShieldBuff : IBuff
    {
        private int _absorb;

        public bool IsExpired => _absorb <= 0;
        public CardTag? ImmunityTag => null;
        public int CurrentAbsorb => _absorb;

        public ShieldBuff(int absorb)
        {
            _absorb = absorb;
        }

        public int AbsorbDamage(int incoming)
        {
            var leftover = Mathf.Max(0, incoming - _absorb);
            _absorb = Mathf.Max(0, _absorb - incoming);
            Debug.Log($"[Shield] Absorbed {incoming - leftover} damage. Shield remaining: {_absorb}");
            return leftover;
        }

        public void Tick(GameObject card) { }

        public void Dispel() => _absorb = 0;
    }
}
