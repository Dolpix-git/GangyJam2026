using CardGame.Data;
using CardGame.Run;
using TMPro;
using UnityEngine;

namespace CardGame.UI.Shop
{
    /// <summary>
    ///     Displays the player's current coins and updates whenever the shop selection changes.
    ///     Shows coins remaining after the current selection would be bought.
    ///     Place anywhere in the Buy phase UI — no model/prefab required, reads RunContext directly.
    /// </summary>
    public class ViewPlayerCoins : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private ShopPurchaseController _purchaseController;

        private void OnEnable()
        {
            if (_purchaseController != null)
            {
                _purchaseController.OnSelectionChanged += Refresh;
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (_purchaseController != null)
            {
                _purchaseController.OnSelectionChanged -= Refresh;
            }
        }

        private void Refresh()
        {
            var coins = RunContext.Instance != null ? RunContext.Instance.Coins : 0;
            var selectionCost = 0;

            if (_purchaseController != null)
            {
                foreach (var card in _purchaseController.Selected)
                {
                    var cost = card.GetComponent<CostData>();
                    if (cost != null)
                    {
                        selectionCost += cost.Cost;
                    }
                }
            }

            var coinsAfter = coins - selectionCost;
            _coinsText.text = selectionCost > 0
                ? $"{coins} (-{selectionCost} = {coinsAfter})"
                : $"{coins}";
        }
    }
}