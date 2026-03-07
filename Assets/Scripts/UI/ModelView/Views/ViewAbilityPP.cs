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

        private Ability _ability;

        protected override void HandleModelChanged(Ability model)
        {
            if (_ability != null)
            {
                _ability.OnPpChanged -= OnPpChanged;
            }

            _ability = model;

            if (_ability == null)
            {
                return;
            }

            _ability.OnPpChanged += OnPpChanged;
            _ppText.text = $"{_ability.CurrentPp}/{_ability.MaxPp}";
        }

        private void OnPpChanged(int current, int max)
        {
            _ppText.text = $"{current}/{max}";
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_ability != null)
            {
                _ability.OnPpChanged -= OnPpChanged;
            }
        }
    }
}