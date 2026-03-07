namespace CardGame.Data
{
    /// <summary>
    ///     Read-only view of a health state. Use for UI, death checks, anything that only reads.
    /// </summary>
    public interface IHealth
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }

        /// <summary>Fires whenever CurrentHealth changes. Args: (current, max).</summary>
        event System.Action<int, int> OnHealthChanged;

        /// <summary>Fires once when the card's health reaches zero.</summary>
        event System.Action OnDied;
    }

    /// <summary>
    ///     Can receive damage. Implement this on any component that takes hits.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }

    /// <summary>
    ///     Can receive healing. Implement this on any component that can be restored.
    /// </summary>
    public interface IHealable
    {
        void Heal(int amount);
    }
}