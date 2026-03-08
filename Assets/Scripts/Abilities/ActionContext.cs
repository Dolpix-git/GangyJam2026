using CardGame.StateMachine.Game;
using CardGame.UI.Battle;
using UnityEngine;

namespace CardGame.Abilities
{
    public class ActionContext
    {
        public CardAnimator Animator;
        public GameObject Caster;
        public int CasterPlayerIndex;
        public int CasterSlotIndex;
        public GameStateData GameState;
        public MonoBehaviour Runner;
    }
}