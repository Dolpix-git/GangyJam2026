using System;
using CardGame.Run;
using CardGame.UI.Combine;
using CardGame.UI.Shop;
using UnityEngine;

namespace CardGame.StateMachine.Explore
{
    public class ExploreStateData : MonoBehaviour
    {
        public ShopComponent Shop;
        public CombineComponent Combine;
        public Action<IState<ExploreStateData>> GoToState { get; set; }
        public RunContext Run => RunContext.Instance;

        public GameObject BuyMenu;
        public GameObject CombineCardsMenu;
        public GameObject CombineResultMenu;
        public CombineResultController CombineResult;
    }
}