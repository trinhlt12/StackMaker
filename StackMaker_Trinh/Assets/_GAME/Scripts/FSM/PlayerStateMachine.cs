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

        private void Awake()
        {
            this.playerBB = GetComponent<PlayerBlackboard>();
            this._stateMachine = new StateMachine();

            this._idleState    = new IdleState(this._stateMachine, this);
            this._moveState    = new MoveState(this._stateMachine, this);
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
    }
}