using CardGame.UI.ModelViewPattern;
using Events;
using UI.ModelView.Models;
using UnityEngine;

namespace CardGame.UI.Shop
{
    public class ViewCardBuyButton : ViewBase<ModelViewCard, GameObject>
    {
        public void BuyCardButton()
        {
            EventBus.Publish(new BuyCardEvent(Model));
        }

        protected override void HandleModelChanged(GameObject model)
        {
        }
    }
}