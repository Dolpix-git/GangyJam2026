using CardGame.Patterns;
using CardGame.StateMachine.Game.States;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class ProgressButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textbox;
        [SerializeField] private Button _button;

        private void Start()
        {
            GameStateSingleton.Instance.CurrentStateChanged += UpdateButton;
        }

        private void OnDestroy()
        {
            GameStateSingleton.Instance.CurrentStateChanged -= UpdateButton;
        }

        private void UpdateButton()
        {
            switch (GameStateSingleton.Instance.CurrentState)
            {
                case PlayState:
                case RetreatState:
                    _textbox.text = "Pass";
                    _button.gameObject.SetActive(true);
                    break;
                case ModeState:
                    _textbox.text = "Battle";
                    _button.gameObject.SetActive(true);
                    break;
                default:
                    _button.gameObject.SetActive(false);
                    break;
            }
        }

        public void OnClick()
        {
            EventBus.Publish(new ProgressButtonClickedEvent());
        }
    }
}