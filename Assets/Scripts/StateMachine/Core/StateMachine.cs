using UnityEngine;

namespace CardGame.StateMachine
{
    /// <summary>
    /// Generic state machine that can be reused for different gameplay systems.
    /// Manages state transitions and updates.
    /// </summary>
    /// <typeparam name="TContext">The context type that holds shared data between states</typeparam>
    public class StateMachine<TContext>
    {
        private IState<TContext> currentState;
        private TContext context;

        public IState<TContext> CurrentState => currentState;

        public StateMachine(TContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Transitions to a new state.
        /// </summary>
        public void ChangeState(IState<TContext> newState)
        {
            if (newState == null)
            {
                Debug.LogError("Attempted to change to a null state!");
                return;
            }

            currentState?.OnExit(context);
            currentState = newState;
            currentState.OnEnter(context);
        }

        /// <summary>
        /// Updates the current state. Call this in your MonoBehaviour's Update method.
        /// </summary>
        public void Update()
        {
            currentState?.OnUpdate(context);
        }

        /// <summary>
        /// Gets the current context.
        /// </summary>
        public TContext GetContext()
        {
            return context;
        }
    }
}
