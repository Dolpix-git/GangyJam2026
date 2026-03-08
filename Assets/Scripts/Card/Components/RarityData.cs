using UnityEngine;

namespace CardGame.Data
{
    public class RarityData : MonoBehaviour
    {
        [SerializeField] private CardRarity _rarity;

        public CardRarity Rarity => _rarity;

        public void Initialize(CardRarity rarity)
        {
            _rarity = rarity;
        }
    }
}
