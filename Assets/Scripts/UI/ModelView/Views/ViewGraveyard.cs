using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewGraveyard : ViewBase<ModelViewGraveyard, PlayerGraveyard>
    {
        [SerializeField] private TMP_Text _graveNumberCount;

        private PlayerGraveyard _graveyard;

        protected override void HandleModelChanged(PlayerGraveyard model)
        {
            if (model == null)
            {
                return;
            }

            if (_graveyard != null)
            {
                Unsubscribe();
            }

            _graveyard = model;
            _graveyard.OnCardAdded += UpdateGraveCount;

            UpdateGraveCount(null);
        }

        private void Unsubscribe()
        {
            if (_graveyard != null)
            {
                _graveyard.OnCardAdded -= UpdateGraveCount;
            }
        }

        private void UpdateGraveCount(GameObject card)
        {
            _graveNumberCount.text = _graveyard.Count.ToString();

            CardUIManager.Instance.DestroyCard(card);
        }
    }
}