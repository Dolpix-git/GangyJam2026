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
        private const float TooltipWidth = 30f;
        private const float Border = 5f;
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
            FitTooltipToText();
        }

        private void FitTooltipToText()
        {
            var textRect = _descriptionText.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(TooltipWidth, 0f);

            _descriptionText.ForceMeshUpdate();

            var textHeight = _descriptionText.preferredHeight;
            textRect.sizeDelta = new Vector2(TooltipWidth, textHeight);

            var tooltipRect = _tooltipRoot.GetComponent<RectTransform>();
            tooltipRect.sizeDelta = new Vector2(TooltipWidth + Border * 2f, textHeight + Border * 2f);
        }
    }
}