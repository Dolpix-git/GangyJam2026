using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using TMPro;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardCost : ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private TMP_Text _costText;

        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null) return;

            var cost = model.GetComponent<CostData>();
            _costText.text = cost != null ? cost.Cost.ToString() : "0";
        }
    }
}
