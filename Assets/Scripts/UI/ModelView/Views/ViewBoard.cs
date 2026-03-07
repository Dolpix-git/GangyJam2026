using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UI.ModelView.Models;
using UnityEngine;

namespace UI.ModelView.Views
{
    public class ViewBoard : ViewBase<ModelViewBoard, PlayerBoard>
    {
        [SerializeField] private GameObject[] _boardSlot = new GameObject[3];
        private PlayerBoard _board;

        private void OnDestroy()
        {
            Unsubscribe();
        }

        protected override void HandleModelChanged(PlayerBoard model)
        {
            if (model == null)
            {
                return;
            }

            _board = model;
            _board.OnCardAdded += OnCardAdded;
        }

        private void Unsubscribe()
        {
            if (_board == null)
            {
                return;
            }

            _board.OnCardAdded -= OnCardAdded;
        }

        private void OnCardAdded(GameObject newCard, int slotIndex)
        {
            CardUIManager.Instance.ReParentCard(newCard, _boardSlot[slotIndex].transform);
        }
    }
}