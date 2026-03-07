using CardGame.Abilities;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.ModelView.Views
{
    public class ViewAbilityDescription : ViewBase<ModelViewAbility, Ability>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private GameObject _tooltipRoot;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Model != null)
            {
                _tooltipRoot.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipRoot.SetActive(false);
        }

        protected override void HandleModelChanged(Ability model)
        {
            if (model == null)
            {
                _tooltipRoot.SetActive(false);
                return;
            }

            _descriptionText.text = model.Description;
        }
    }
}