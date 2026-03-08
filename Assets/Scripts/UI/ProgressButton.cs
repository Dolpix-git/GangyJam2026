using Events;
using UnityEngine;

namespace CardGame.UI
{
    public class ProgressButton: MonoBehaviour
    {
        public void OnClick()
        {
            EventBus.Publish(new ProgressButtonClickedEvent());
        }
    }
}