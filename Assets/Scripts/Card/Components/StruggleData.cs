using UnityEngine;

namespace CardGame.Data
{
    public class StruggleData : MonoBehaviour
    {
        [SerializeField] private int _struggleDamage;

        public int StruggleDamage => _struggleDamage;

        public void Initialize(int struggleDamage)
        {
            _struggleDamage = struggleDamage;
        }
    }
}