using CardGame.Card;
using CardGame.StateMachine.Explore;
using CardGame.StateMachine.Explore.States;
using UnityEngine;

namespace CardGame.UI.Combine
{
    public class CombineResultController : MonoBehaviour
    {
        [SerializeField] private ExploreStateData _stateData;

        public GameObject ParentCardA { get; private set; }
        public GameObject ParentCardB { get; private set; }
        public GameObject ResultCard { get; private set; }

        public void Show(GameObject parentA, GameObject parentB, string combinedId)
        {
            ParentCardA = parentA;
            ParentCardB = parentB;
            ResultCard = CardFactory.Instance.CreateCard(combinedId);
        }

        public void GoToBattle()
        {
            _stateData.GoToState(new ExploreFightState());
        }
    }
}