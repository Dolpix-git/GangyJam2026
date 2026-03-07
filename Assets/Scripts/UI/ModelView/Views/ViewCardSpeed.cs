using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardSpeed: ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text cardSpeedText;

        private SpeedData cardSpeed;
        
        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            cardSpeed = model.GetComponent<SpeedData>();

            cardSpeedText.text = cardSpeed.CurrentSpeed.ToString();
        }
    }
}