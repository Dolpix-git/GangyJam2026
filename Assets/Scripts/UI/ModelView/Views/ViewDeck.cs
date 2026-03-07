using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewDeck : ViewBase<ModelViewDeck, PlayerDeck>
    {
        [SerializeField] private TMP_Text _deckNumberCount;

        private PlayerDeck _deck;

        protected override void HandleModelChanged(PlayerDeck model)
        {
            if (model == null)
            {
                return;
            }

            if (_deck != null)
            {
                _deck.OnDeckChanged -= UpdateDeckCount;
            }

            _deck = model;
            _deck.OnDeckChanged += UpdateDeckCount;

            UpdateDeckCount();
        }

        private void OnDestroy()
        {
            if (_deck == null)
            {
                return;
            }

            _deck.OnDeckChanged += UpdateDeckCount;
        }

        private void UpdateDeckCount()
        {
            _deckNumberCount.text = _deck.Count.ToString();
        }
    }
}