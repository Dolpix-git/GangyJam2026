using UnityEngine;

namespace CardGame.Buffs
{
    public interface IBuff
    {
        /// <summary>Whether this buff has finished and should be removed.</summary>
        bool IsExpired { get; }

        /// <summary>Tag that makes a card immune to this buff. Null means no immunity exists.</summary>
        CardTag? ImmunityTag { get; }

        /// <summary>Called once per turn just before the card acts. Apply effects and advance duration here.</summary>
        void Tick(GameObject card);

        /// <summary>Force-expire this buff immediately (dispel, cure, etc).</summary>
        void Dispel();
    }
}
