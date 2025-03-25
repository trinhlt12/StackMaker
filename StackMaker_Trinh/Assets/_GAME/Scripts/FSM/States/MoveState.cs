namespace _GAME.Scripts.FSM.States
{
    using UnityEngine;

    public class MoveState : BaseState
    {
        private Vector3 _moveDirection;
        public MoveState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) :
            base(stateMachine, playerStateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            var swipe = this._playerStateMachine.playerBB.currentSwipeDirection;
            this._playerStateMachine.playerBB.ResetSwipe();

            this._moveDirection = swipe switch
            {
                SwipeDirection.Up    => Vector3.forward,
                SwipeDirection.Down  => Vector3.back,
                SwipeDirection.Left  => Vector3.left,
                SwipeDirection.Right => Vector3.right,
                _                    => Vector3.zero
            };
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var controller = this._playerStateMachine.playerBB.playerController;
            controller.Move(this._moveDirection * (this._playerStateMachine.playerBB.moveSpeed * Time.deltaTime));
        }
    }
}