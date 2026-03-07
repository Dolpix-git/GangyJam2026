using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;
using TMPro;

namespace UI.ModelView.Views
{
    public class ViewCardIdentity: ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text cardNameText;
        [SerializeField] private TMP_Text descriptionText;

        private CardIdentity cardIdentity;
        
        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            cardIdentity = model.GetComponent<CardIdentity>();

            cardNameText.text = cardIdentity.CardName;
            descriptionText.text = cardIdentity.Description;
        }
    }
}