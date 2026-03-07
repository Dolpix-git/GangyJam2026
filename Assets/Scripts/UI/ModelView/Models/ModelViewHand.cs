using CardGame.Player;
using CardGame.UI.ModelViewPattern;
using UnityEngine;

public class ModelViewHand : ModelViewBase<PlayerHand>
{
    [SerializeField] private PlayerHand hand;

    private void Start()
    {
        if (hand == null)
        {
            Debug.LogWarning("Hand not set in ModelViewHand");
            return;
        }

        SetModel(hand);
    }
}
