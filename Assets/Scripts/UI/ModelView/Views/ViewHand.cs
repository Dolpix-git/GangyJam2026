using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewHand : ViewBase<ModelViewHand, PlayerHand>
    {
        private PlayerHand hand;

        protected override void HandleModelChanged(PlayerHand model)
        {
            if (model == null)
            {
                return;
            }

            if (hand != null)
            {
                Unsubscribe();
            }

            hand = model;

            hand.OnCardAdded += OnCardAdded;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (hand == null)
            {
                return;
            }

            hand.OnCardAdded -= OnCardAdded;
        }

        private void OnCardAdded(GameObject newCard)
        {
            CardUIManager.Instance.ReParentCard(newCard, transform);
        }
    }
}