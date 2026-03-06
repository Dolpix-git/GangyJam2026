using UnityEngine;

namespace CardGame.Data
{
    /// <summary>
    /// Pure data component for card identification.
    /// No behavior, only data storage.
    /// </summary>
    public class CardIdentity : MonoBehaviour
    {
        [SerializeField] private string cardId;
        [SerializeField] private string cardName;
        [SerializeField] private string description;

        public string CardId
        {
            get => cardId;
            set => cardId = value;
        }

        public string CardName
        {
            get => cardName;
            set => cardName = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public void Initialize(string id, string name, string desc)
        {
            cardId = id;
            cardName = name;
            description = desc;
        }
    }
}
