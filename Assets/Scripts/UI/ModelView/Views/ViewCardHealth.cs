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
        
        private HealthData _cardHealth;

        protected override void HandleModelChanged(GameObject model)
        {
            if (_cardHealth != null)
            {
                _cardHealth.OnHealthChanged -= OnHealthChanged;
            }

            if (model == null)
            {
                _cardHealth = null;
                return;
            }

            _cardHealth = model.GetComponent<HealthData>();
            _cardHealth.OnHealthChanged += OnHealthChanged;
            cardHealthText.text = _cardHealth.CurrentHealth.ToString();
        }

        private void OnHealthChanged(int current, int max)
        {
            cardHealthText.text = current.ToString();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_cardHealth != null)
            {
                _cardHealth.OnHealthChanged -= OnHealthChanged;
            }
        }
    }
}