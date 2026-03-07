using CardGame.Abilities;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewAbilityPp : ViewBase<ModelViewAbility, Ability>
    {
        [SerializeField] private TMP_Text _ppText;

        protected override void HandleModelChanged(Ability model)
        {
            if (model == null)
            {
                return;
            }

            _ppText.text = $"{model.CurrentPp}/{model.MaxPp}";
        }
    }
}