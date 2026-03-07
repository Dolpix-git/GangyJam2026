using CardGame.Player;
using UnityEngine;

namespace CardGame.Card
{
    public class CardSpawnerTest : MonoBehaviour
    {
        [SerializeField] private string _cardName;
        [SerializeField] private PlayerDeck _playerDeck;

        [ContextMenu("Spawn Card")]
        private void SpawnCard()
        {
            var newCard = CardFactory.Instance.CreateCard(_cardName);
            _playerDeck?.AddCard(newCard);
        }
    }
}
