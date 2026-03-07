using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardName : ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text _cardNameText;

        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            _cardNameText.text = model.GetComponent<CardIdentity>().CardName;
        }
    }
}