namespace _GAME.Scripts.FSM
{
    public class StateMachine
    {
        public IState currentState;
        public IState previousState;

        public void InitializeStateMachine(IState initialState)
        {
            this.currentState = initialState;
            this.previousState = null;
            this.currentState.OnEnter();
        }
        public void ChangeState(IState newState)
        {
            if(this.currentState == newState) return;

            if (this.currentState != null)
            {
                this.previousState = this.currentState;
                this.currentState?.OnExit();
            }

            this.currentState  = newState;
            this.currentState?.OnEnter();
        }

        public void StateUpdate()
        {
            this.currentState?.OnUpdate();
        }

        public void StateFixedUpdate()
        {
            this.currentState?.OnFixedUpdate();
        }

        public void RevertToPreviousState()
        {
            if (this.previousState != null)
            {
                ChangeState(this.previousState);
            }
        }


    }
}