using CardGame.StateMachine.Game.States;
using UnityEngine;

namespace CardGame.StateMachine.Game.Dev
{
    public class EndBattleDevTool: MonoBehaviour
    {
        [SerializeField] private GameStateData _stateData;

        [ContextMenu("OnEndBattleDevTool")]
        public void OnEndBattleDevTool()
        {
            _stateData.GoToState(new EndGameState(1));
        }
    }
}