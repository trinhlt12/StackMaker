namespace _GAME.Scripts.FSM.States
{
    using _GAME.Scripts.GameManager;
    using UnityEngine;

    public abstract class BaseState : IState
    {
        protected readonly PlayerStateMachine _playerStateMachine;
        protected readonly StateMachine       _stateMachine;
        protected readonly string _animationName;
        protected float _elapsedTime;
        protected BaseState(StateMachine stateMachine, PlayerStateMachine playerStateMachine, string animationName)
        {
            _stateMachine       = stateMachine;
            _playerStateMachine = playerStateMachine;
            _animationName      = animationName;
        }

        public virtual void OnEnter()
        {
            this._elapsedTime = 0f;
            PlayAnimation(this._animationName);
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

        private void PlayAnimation(string animationName)
        {
            this._playerStateMachine.playerBB.animator.Play(animationName);
        }


    }
}