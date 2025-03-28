namespace _GAME.Scripts.FSM.States
{
    using _GAME.Scripts.GameManager;

    public abstract class BaseState : IState
    {
        protected readonly PlayerStateMachine _playerStateMachine;
        protected readonly StateMachine       _stateMachine;

        protected BaseState(StateMachine stateMachine, PlayerStateMachine playerStateMachine)
        {
            _stateMachine       = stateMachine;
            _playerStateMachine = playerStateMachine;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }


    }
}