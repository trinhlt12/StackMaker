namespace _GAME.Scripts.FSM.States
{
    using _GAME.Scripts.GameManager;
    using UnityEngine;

    public abstract class BaseState : IState
    {
        protected readonly PlayerStateMachine _playerStateMachine;
        protected readonly StateMachine       _stateMachine;
        protected float _elapsedTime;
        protected BaseState(StateMachine stateMachine, PlayerStateMachine playerStateMachine)
        {
            _stateMachine       = stateMachine;
            _playerStateMachine = playerStateMachine;
        }

        public virtual void OnEnter()
        {
            this._elapsedTime = 0f;
        }

        public virtual void OnUpdate()
        {
            this._elapsedTime += Time.deltaTime;
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }

        public float GetElapsedTime()
        {
            return this._elapsedTime;
        }


    }
}