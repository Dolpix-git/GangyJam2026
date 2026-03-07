using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewGraveyard : ViewBase<ModelViewGraveyard, PlayerGraveyard>
    {
        [SerializeField] private TMP_Text graveNumberCount;

        private PlayerGraveyard graveyard;

        protected override void HandleModelChanged(PlayerGraveyard model)
        {
            if (model == null)
            {
                return;
            }

            if (graveyard != null)
            {
                Unsubscribe();
            }

            graveyard = model;
            graveyard.OnCardAdded += UpdateGraveCount;
            
            UpdateGraveCount(null);
        }

        private void Unsubscribe()
        {
            if (graveyard != null)
            {
                graveyard.OnCardAdded -= UpdateGraveCount;
            }
        }

        private void UpdateGraveCount(GameObject card)
        {
            graveNumberCount.text = graveyard.Count.ToString();

            CardUIManager.Instance.DestroyCard(card);
        }
    }
}