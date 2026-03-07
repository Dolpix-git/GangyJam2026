using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

namespace UI.ModelView.Models
{
    public class ModelViewDeck: ModelViewBase<PlayerDeck>
    {
        [SerializeField] private PlayerDeck deck;

        private void Start()
        {
            if (deck == null)
            {
                Debug.LogWarning("Deck not set in ModelViewDeck");
                return;
            }

            SetModel(deck);
        }
    }
}