using CardGame.Data;
using CardGame.Player;
using UnityEngine;

namespace CardGame.Card.Components
{
    public class CardDeathHandler : MonoBehaviour
    {
        private GameObject _ownerPlayer;

        private void OnDestroy()
        {
            var health = GetComponent<HealthData>();
            if (health != null)
            {
                health.OnDied -= HandleDeath;
            }
        }

        public void Initialize(GameObject ownerPlayer)
        {
            _ownerPlayer = ownerPlayer;
            GetComponent<HealthData>().OnDied += HandleDeath;
        }

        private void HandleDeath()
        {
            var identity = GetComponent<CardIdentity>();
            Debug.Log($"[Death] '{identity?.CardName ?? gameObject.name}' has died.");

            var board = _ownerPlayer.GetComponent<PlayerBoard>();
            for (var i = 0; i < PlayerBoard.BoardSize; i++)
            {
                if (board.GetSlot(i) != gameObject)
                {
                    continue;
                }

                board.RemoveAt(i);
                break;
            }

            _ownerPlayer.GetComponent<PlayerGraveyard>().AddCard(gameObject);
            Debug.Log($"[Death] '{identity?.CardName ?? gameObject.name}' moved to graveyard.");
        }
    }
}