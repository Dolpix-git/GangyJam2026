using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

namespace UI.ModelView.Models
{
    public class ModelViewBoard : ModelViewBase<PlayerBoard>
    {
        [SerializeField] private PlayerBoard board;
        
        private void Start()
        {
            if (board == null)
            {
                Debug.LogWarning("Board not set in ModelViewBoard");
                return;
            }

            SetModel(board);
        }
    }
}