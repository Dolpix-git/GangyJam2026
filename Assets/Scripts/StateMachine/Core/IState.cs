namespace CardGame.StateMachine
{
    /// <summary>
    /// Interface for all states in a state machine.
    /// </summary>
    /// <typeparam name="TContext">The context type that holds shared data between states</typeparam>
    public interface IState<TContext>
    {
        /// <summary>
        /// Called when entering this state.
        /// </summary>
        void OnEnter(TContext context);

        /// <summary>
        /// Called every frame while in this state.
        /// </summary>
        void OnUpdate(TContext context);

        /// <summary>
        /// Called when exiting this state.
        /// </summary>
        void OnExit(TContext context);
    }
}
