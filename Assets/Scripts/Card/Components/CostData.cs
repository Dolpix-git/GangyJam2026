using UnityEngine;

namespace CardGame.Data
{
    public class CostData : MonoBehaviour
    {
        [SerializeField] private int _cost;

        public int Cost => _cost;

        public void Initialize(int cost)
        {
            _cost = cost;
        }
    }
}