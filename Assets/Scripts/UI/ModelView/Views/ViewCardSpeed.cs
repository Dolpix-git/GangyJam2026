using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardSpeed : ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text _cardSpeedText;

        private SpeedData _cardSpeed;

        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            _cardSpeed = model.GetComponent<SpeedData>();

            _cardSpeedText.text = _cardSpeed.CurrentSpeed.ToString();
        }
    }
}