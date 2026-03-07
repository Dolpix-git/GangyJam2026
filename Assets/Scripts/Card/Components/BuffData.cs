using System.Collections.Generic;
using System.Linq;
using CardGame.Buffs;
using UnityEngine;

namespace CardGame.Data
{
    public class BuffData : MonoBehaviour
    {
        private readonly List<IBuff> _buffs = new();

        public IReadOnlyList<IBuff> Buffs => _buffs;

        public bool IsParalysed => _buffs.OfType<ParalysisBuff>().Any(b => !b.IsExpired);

        public int ModifyOutgoingDamage(int baseDamage)
        {
            var result = (float)baseDamage;
            foreach (var amp in _buffs.OfType<DamageAmpBuff>())
            {
                result *= amp.Multiplier;
            }

            return Mathf.RoundToInt(result);
        }

        public int AbsorbIncomingDamage(int incomingDamage)
        {
            var remaining = incomingDamage;
            foreach (var shield in _buffs.OfType<ShieldBuff>())
            {
                remaining = shield.AbsorbDamage(remaining);
                if (remaining <= 0)
                {
                    break;
                }
            }

            _buffs.RemoveAll(b => b.IsExpired);
            return remaining;
        }
        
        public bool Add(IBuff buff)
        {
            var tags = GetComponent<TagData>();
            if (buff.ImmunityTag.HasValue && tags != null && tags.HasTag(buff.ImmunityTag.Value))
            {
                Debug.Log($"[BuffData] '{gameObject.name}' is immune to {buff.GetType().Name} ({buff.ImmunityTag}).");
                return false;
            }

            _buffs.Add(buff);
            return true;
        }

        public void TickAll()
        {
            foreach (var buff in _buffs)
            {
                buff.Tick(gameObject);
            }

            _buffs.RemoveAll(b => b.IsExpired);
        }

        public void DispelAll()
        {
            foreach (var buff in _buffs)
            {
                buff.Dispel();
            }

            _buffs.Clear();
        }

        public void Dispel<T>() where T : IBuff
        {
            foreach (var buff in _buffs.OfType<T>())
            {
                buff.Dispel();
            }

            _buffs.RemoveAll(b => b.IsExpired);
        }
    }
}