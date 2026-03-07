using System.Collections.Generic;
using System.Linq;
using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewHand : ViewBase<ModelViewHand, PlayerHand>
    {
        [SerializeField] private ModelViewCard cardDisplayPrefab;

        private PlayerHand hand;
        private List<ModelViewCard> cardsInHand = new();

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
            hand.OnCardRemoved += OnCardRemoved;
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
            hand.OnCardRemoved -= OnCardRemoved;
        }

        private void OnCardAdded(GameObject newCard)
        {
            var modelViewCard = Instantiate(cardDisplayPrefab, transform);
            modelViewCard.SetModel(newCard);

            cardsInHand.Add(modelViewCard);
        }

        private void OnCardRemoved(GameObject removedCard)
        {
            var cardModel = cardsInHand.FirstOrDefault(cardUI => cardUI.Model == removedCard);
            cardsInHand.Remove(cardModel);
        }
    }
}