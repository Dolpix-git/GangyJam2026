using CardGame.UI.ModelViewPattern;
using Events;
using UI.ModelView.Models;
using UnityEngine;

namespace CardGame.UI.Combine
{
    public class ViewCardCombineButton : ViewBase<ModelViewCard, GameObject>
    {
        public void CombineCardButton()
        {
            EventBus.Publish(new CombineCardEvent(Model));
        }

        protected override void HandleModelChanged(GameObject model)
        {
        }
    }
}