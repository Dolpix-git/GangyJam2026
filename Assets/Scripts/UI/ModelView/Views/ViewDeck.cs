using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewDeck: ViewBase<ModelViewDeck, PlayerDeck>
    {
        [SerializeField] private TMP_Text deckNumberCount;
        
        private PlayerDeck deck;
        
        protected override void HandleModelChanged(PlayerDeck model)
        {
            if (model == null)
            {
                return;
            }

            if (deck != null)
            {
                deck.OnDeckChanged -= UpdateDeckCount;
            }

            deck = model;
            deck.OnDeckChanged += UpdateDeckCount;

            UpdateDeckCount();
        }

        private void OnDestroy()
        {
            if (deck == null)
            {
                return;
            }
            
            deck.OnDeckChanged += UpdateDeckCount;
        }

        private void UpdateDeckCount()
        {
            deckNumberCount.text = deck.Count.ToString();
        }
    }
}