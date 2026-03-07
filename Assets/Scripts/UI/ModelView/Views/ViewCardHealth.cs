using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardHealth: ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text cardHealthText;
        
        private HealthData cardHealth;
        
        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            cardHealth = model.GetComponent<HealthData>();

            cardHealthText.text = cardHealth.CurrentHealth.ToString();
        }
    }
}