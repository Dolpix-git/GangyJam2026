using UnityEngine;

namespace CardGame.Data
{
    public class HealthData : MonoBehaviour, IHealth, IDamageable, IHealable
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;

        public void TakeDamage(int amount)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - amount);
            Debug.Log($"[Health] {gameObject.name} took {amount} damage. HP: {_currentHealth}/{_maxHealth}");
        }

        public void Heal(int amount)
        {
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            Debug.Log($"[Health] {gameObject.name} healed {amount}. HP: {_currentHealth}/{_maxHealth}");
        }

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;

        public void Initialize(int max)
        {
            _maxHealth = max;
            _currentHealth = max;
        }
    }
}