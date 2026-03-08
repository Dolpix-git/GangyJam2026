using System;
using CardGame.Abilities;
using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModelView.Views
{
    public class ViewAbilityButton : ViewBase<ModelViewAbility, Ability>
    {
        private ModelViewCard _modelViewCard;
        private AbilityData _abilityData;
        private CardMode _cardMode;
        private Ability _ability;

        [SerializeField] private Button _button;

        protected override void Awake()
        {
            base.Awake();

            if (_button == null)
            {
                Debug.LogWarning($"{nameof(ViewAbilityButton)}: Button reference is missing.");
            }
        }

        private void Start()
        {
            _modelViewCard = GetComponentInParent<ModelViewCard>();
            _modelViewCard.OnModelChanged += OnCardModelChanged;

            if (_modelViewCard.Model != null)
            {
                OnCardModelChanged(_modelViewCard.Model);
            }
        }

        private void OnDestroy()
        {
            if (_modelViewCard)
            {
                _modelViewCard.OnModelChanged -= OnCardModelChanged;
            }

            if (_cardMode)
            {
                _cardMode.OnAbilitySelect -= HighlightButton;
            }
        }

        private void OnCardModelChanged(GameObject card)
        {
            if (_cardMode)
            {
                _cardMode.OnAbilitySelect -= HighlightButton;
            }

            if (card == null)
            {
                Debug.LogError($"{nameof(ViewAbilityButton)}: card is null in {nameof(OnCardModelChanged)}");
                _cardMode = null;
                _abilityData = null;
                return;
            }


            _cardMode = card.GetComponent<CardMode>();
            if (!_cardMode)
            {
                Debug.LogError("No CardMode in CardModel");
                _abilityData = null;
                return;
            }

            _cardMode.OnAbilitySelect += HighlightButton;
            _abilityData = card.GetComponent<AbilityData>();

            if (_abilityData == null)
            {
                Debug.LogError("No AbilityData in CardModel");
            }
        }

        private void HighlightButton()
        {
            if (_abilityData == null)
            {
                Debug.LogWarning($"{nameof(ViewAbilityButton)}: AbilityData is null.");
                return;
            }

            if (_cardMode == null)
            {
                Debug.LogWarning($"{nameof(ViewAbilityButton)}: CardMode is null.");
                return;
            }

            if (_abilityData.Abilities == null)
            {
                Debug.LogWarning($"{nameof(ViewAbilityButton)}: Abilities list is null.");
                return;
            }

            int index = _cardMode.SelectedAbilityIndex;
            Ability selectedAbility;
            if (index < 0 || index >= _abilityData.Abilities.Count)
            {
                selectedAbility = null;
            }
            else
            {
                selectedAbility = _abilityData.Abilities[index];
            }

            var thisAbilitySelected = selectedAbility == _ability;

            var buttonColors = _button.colors;
            buttonColors.normalColor = thisAbilitySelected ? Color.green : Color.white;

            _button.colors = buttonColors;
        }

        protected override void HandleModelChanged(Ability model)
        {
            if (model == null)
            {
                return;
            }

            _ability = model;
        }
    }
}