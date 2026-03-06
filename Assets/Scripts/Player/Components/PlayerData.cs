using UnityEngine;

namespace CardGame.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private string _playerName;

        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        public void Initialize(string playerName)
        {
            _playerName = playerName;
        }
    }
}
