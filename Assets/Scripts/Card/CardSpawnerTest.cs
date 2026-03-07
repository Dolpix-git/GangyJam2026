using UnityEngine;

namespace CardGame.Card
{
    public class CardSpawnerTest : MonoBehaviour
    {
        [SerializeField] private string _cardName;

        [ContextMenu("Spawn Card")]
        private void SpawnCard()
        {
            CardFactory.Instance.CreateCard(_cardName);
        }
    }
}
