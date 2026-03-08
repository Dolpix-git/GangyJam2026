using CardGame.StateMachine.Explore;
using CardGame.StateMachine.Explore.States;
using UnityEngine;

namespace CardGame.UI
{
    public class ExploreUIController : MonoBehaviour
    {
        [SerializeField] private ExploreStateData _stateData;

        [ContextMenu("OnBuyDonePressed")]
        public void OnBuyDonePressed()
        {
            _stateData.GoToState(new ExploreCombineState());
        }

        [ContextMenu("OnCombineDonePressed")]
        public void OnCombineDonePressed()
        {
            _stateData.GoToState(new ExploreFightState());
        }
    }
}