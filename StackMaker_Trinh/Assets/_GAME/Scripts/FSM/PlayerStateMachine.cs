namespace _GAME.Scripts.FSM
{
    using System;
    using System.Net.NetworkInformation;
    using _GAME.Scripts.FSM.States;
    using UnityEngine;

    public class PlayerStateMachine : MonoBehaviour
    {
        public PlayerBlackboard playerBB;

        public StateMachine _stateMachine;

        public IdleState _idleState;
        public MoveState _moveState;
        public WinState _winState;

        private void Awake()
        {
            this.playerBB = GetComponent<PlayerBlackboard>();
            this._stateMachine = new StateMachine();

            this._idleState    = new IdleState(this._stateMachine, this, "Idle");
            this._moveState    = new MoveState(this._stateMachine, this, "Move");
            this._winState     = new WinState(this._stateMachine, this, "Win");
        }

        private void Start()
        {
            this._stateMachine.ChangeState(this._idleState);
        }

        private void Update()
        {
            this._stateMachine.StateUpdate();
        }

        private void FixedUpdate()
        {
            this._stateMachine.StateFixedUpdate();
        }

        public void UpdateMoveStateTarget(Vector3 newTargetPosition)
        {
            if (_stateMachine.currentState is MoveState moveState)
            {
                var offset = this.playerBB.GetComponent<CapsuleCollider>().bounds.size.y / 2;
                moveState.UpdateTargetPosition(newTargetPosition + Vector3.up * offset);
            }
        }

        public void ChangeToWinState()
        {
            this._stateMachine.ChangeState(this._winState);
        }

    }
}