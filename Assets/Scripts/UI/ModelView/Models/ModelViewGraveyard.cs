using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

namespace UI.ModelView.Models
{
    public class ModelViewGraveyard: ModelViewBase<PlayerGraveyard>
    {
        [SerializeField] private PlayerGraveyard graveyard;
        
        private void Start()
        {
            if (graveyard == null)
            {
                Debug.LogWarning("Graveyard not set in ModelViewGraveyard");
                return;
            }

            SetModel(graveyard);
        }
    }
}