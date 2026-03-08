using CardGame.Data;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModelView.Views
{
    public class ViewCardRarity : ViewBase<ModelViewCard, GameObject>
    {
        [SerializeField] private Image _image;

        [Header("Rarity Colours")]
        [SerializeField] private Color _common = new(0.75f, 0.75f, 0.75f);

        [SerializeField] private Color _uncommon = new(0.25f, 0.75f, 0.25f);
        [SerializeField] private Color _rare = new(0.25f, 0.50f, 1.00f);
        [SerializeField] private Color _epic = new(0.65f, 0.25f, 1.00f);
        [SerializeField] private Color _legendary = new(1.00f, 0.65f, 0.00f);

        protected override void HandleModelChanged(GameObject model)
        {
            if (model == null)
            {
                return;
            }

            var rarity = model.GetComponent<RarityData>();
            if (rarity == null)
            {
                return;
            }

            _image.color = rarity.Rarity switch
            {
                CardRarity.Common => _common,
                CardRarity.Uncommon => _uncommon,
                CardRarity.Rare => _rare,
                CardRarity.Epic => _epic,
                CardRarity.Legendary => _legendary,
                _ => _common
            };
        }
    }
}