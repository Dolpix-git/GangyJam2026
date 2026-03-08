using CardGame.Patterns;
using CardGame.Run;
using CardGame.StateMachine.Game.States;
using Events;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private GameObject _textParent;
    [SerializeField] private TMP_Text _textbox;

    private void Start()
    {
        GameStateSingleton.Instance.CurrentStateChanged += UpdateButton;
        
        EventBus.Subscribe<EndGameEvent>(EndGameText);
    }

    private void OnDestroy()
    {
        GameStateSingleton.Instance.CurrentStateChanged -= UpdateButton;
        
        EventBus.Unsubscribe<EndGameEvent>(EndGameText);
    }

    private void EndGameText(EndGameEvent endGameEvent)
    {
        _textbox.text = $"END GAME \n " +
                        $"You {(endGameEvent.Result ? "WON" : "LOST")} \n" +
                        $"{(endGameEvent.Result ? $"+{RunContext.CoinsPerWin} coins awarded. Total: {RunContext.Instance.Coins}" : "")}";
        _textParent.gameObject.SetActive(true);
    }

    private void UpdateButton()
    {
        switch (GameStateSingleton.Instance.CurrentState)
        {
            case DrawState:
                _textbox.text = "DRAW PHASE";
                _textParent.gameObject.SetActive(true);
                break;
            case PlayState:
                _textbox.text = "Drag a Kreet from your hand to a slot on your field to play them \n If you have at least 1 Kreet on the field, you may PASS";
                _textParent.gameObject.SetActive(true);
                break;
            case RetreatState:
                _textbox.text = "You may retreat a Kreet from the field back into your hand \n Click PASS to go to MODE PHASE \n You MUST have at least 1 Kreet on the field ";
                _textParent.gameObject.SetActive(true);
                break;
            case ModeState:
                _textbox.text = "Select 1 Ability on EACH Kreet then press BATTLE";
                _textParent.gameObject.SetActive(true);
                break;
            default:
                _textParent.gameObject.SetActive(false);
                break;
        }
    }
}
