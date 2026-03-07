using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewHand : ViewBase<ModelViewHand, PlayerHand>
    {
        private PlayerHand _hand;

        private void OnDestroy()
        {
            Unsubscribe();
        }

        protected override void HandleModelChanged(PlayerHand model)
        {
            if (model == null)
            {
                return;
            }

            if (_hand != null)
            {
                Unsubscribe();
            }

            _hand = model;

            _hand.OnCardAdded += OnCardAdded;
        }

        private void Unsubscribe()
        {
            if (_hand == null)
            {
                return;
            }

            _hand.OnCardAdded -= OnCardAdded;
        }

        private void OnCardAdded(GameObject newCard)
        {
            CardUIManager.Instance.ReParentCard(newCard, transform);
        }
    }
}