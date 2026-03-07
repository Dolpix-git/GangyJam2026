using CardGame.Abilities;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewAbilityName : ViewBase<ModelViewAbility, Ability>
    {
        [SerializeField] private TMP_Text _nameText;

        protected override void HandleModelChanged(Ability model)
        {
            if (model == null)
            {
                return;
            }

            _nameText.text = model.Name;
        }
    }
}