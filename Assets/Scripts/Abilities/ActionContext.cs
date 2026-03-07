using CardGame.StateMachine.Game;
using UnityEngine;

namespace CardGame.Abilities
{
    public class ActionContext
    {
        public GameObject Caster;
        public int CasterPlayerIndex;
        public int CasterSlotIndex;
        public GameStateData GameState;
        public MonoBehaviour Runner;
    }
}