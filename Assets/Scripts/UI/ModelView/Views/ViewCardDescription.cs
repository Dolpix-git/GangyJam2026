using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardDescription : ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text _descriptionText;

        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            _descriptionText.text = model.GetComponent<CardIdentity>().Description;
        }
    }
}