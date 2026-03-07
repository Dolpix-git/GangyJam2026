using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewCardAbilities : ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private ModelViewAbility _abilityPrefab;

        protected override void HandleModelChanged(GameObject model)
        {
            ClearAbilities();

            if (model == null)
            {
                return;
            }

            var abilityData = model.GetComponent<AbilityData>();
            if (abilityData == null)
            {
                return;
            }

            foreach (var ability in abilityData.Abilities)
            {
                var instance = Instantiate(_abilityPrefab, transform);
                instance.SetModel(ability);
            }
        }

        private void ClearAbilities()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}