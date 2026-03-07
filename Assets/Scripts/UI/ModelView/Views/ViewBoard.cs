using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewBoard : ViewBase<ModelViewBoard, PlayerBoard>
    {
        private PlayerBoard board;

        [SerializeField] private GameObject[] boardSlot = new GameObject[3];

        protected override void HandleModelChanged(PlayerBoard model)
        {
            if (model == null)
            {
                return;
            }

            board = model;
            board.OnCardAdded += OnCardAdded;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (board == null)
            {
                return;
            }

            board.OnCardAdded -= OnCardAdded;
        }

        private void OnCardAdded(GameObject newCard, int slotIndex)
        {
            CardUIManager.Instance.ReParentCard(newCard, boardSlot[slotIndex].transform);
        }
    }
}