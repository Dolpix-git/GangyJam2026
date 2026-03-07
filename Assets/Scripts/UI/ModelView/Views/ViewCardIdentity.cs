using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;
using TMPro;

namespace UI.ModelView.Views
{
    public class ViewCardIdentity: ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text _cardNameText;
        [SerializeField] private TMP_Text _descriptionText;

        
        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            var cardIdentity = model.GetComponent<CardIdentity>();

            _cardNameText.text = cardIdentity.CardName;
            _descriptionText.text = cardIdentity.Description;
        }
    }
}