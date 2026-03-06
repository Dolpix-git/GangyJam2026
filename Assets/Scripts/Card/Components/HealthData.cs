using UnityEngine;

namespace CardGame.Data
{
    /// <summary>
    /// Pure data component for health values.
    /// No behavior, only data storage.
    /// </summary>
    public class HealthData : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public void Initialize(int max)
        {
            maxHealth = max;
            currentHealth = max;
        }
    }
}
