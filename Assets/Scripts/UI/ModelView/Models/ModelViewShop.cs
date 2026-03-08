using CardGame.UI.Shop;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

namespace UI.ModelView.Models
{
    public class ModelViewShop : ModelViewBase<ShopComponent>
    {
        [SerializeField] private ShopComponent _shop;

        private void OnEnable()
        {
            SetModel(_shop);
        }
    }
}