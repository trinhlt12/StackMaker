namespace _GAME.Scripts.FSM.States
{
    using UnityEngine;

    public class MoveState : BaseState
    {
        private       int     _maxSteps = 50;
        private const int     _stepSize = 1;
        private Vector3 _targetPosition;
        private Vector3 _moveDirection;
        public MoveState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) :
            base(stateMachine, playerStateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            var swipe = this._playerStateMachine.playerBB.currentSwipeDirection;

            InputManager.Instance.SetCanAcceptInput(false);

            _moveDirection = swipe switch
            {
                SwipeDirection.Forward  => Vector3.forward,
                SwipeDirection.Backward => Vector3.back,
                SwipeDirection.Left     => Vector3.left,
                SwipeDirection.Right    => Vector3.right,
                SwipeDirection.None     => Vector3.zero
            };

            this._playerStateMachine.playerBB.ResetSwipe();


            this._targetPosition = FindTargetPosition();
            Debug.DrawRay(_targetPosition + Vector3.up * 0.5f, Vector3.up * 0.5f, Color.cyan, 2f);
            Debug.Log("[MoveState] Target: " + _targetPosition);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            InputManager.Instance.SetCanAcceptInput(false);


            var   currentPos       = this._playerStateMachine.transform.position;
            float distanceToTarget = Vector3.Distance(currentPos, _targetPosition);

            if (distanceToTarget < 0.05f)
            {
                this._playerStateMachine.playerBB.canMove = false;
                this._stateMachine.ChangeState(this._playerStateMachine._idleState);
                return;
            }

            MoveTowardToTarget(_targetPosition);

            if (this._playerStateMachine.playerBB.playerController.velocity.magnitude < 0.1f)
            {
                this._stateMachine.ChangeState(this._playerStateMachine._idleState);
            }

        }

        private void MoveTowardToTarget(Vector3 target)
        {
            if (!this._playerStateMachine.playerBB.canMove)
                return;
            var controller = this._playerStateMachine.playerBB.playerController;
            var playerPosition = this._playerStateMachine.transform.position;
            var moveSpeed = this._playerStateMachine.playerBB.moveSpeed;
            var direction = (target - playerPosition).normalized;
            Debug.Log(direction);

            controller.Move(direction * (moveSpeed * Time.deltaTime));
        }

        private Vector3 FindTargetPosition()
        {
            var raycastHeight   = 0.2f;
            var raycastDistance = 1.5f;
            var groundLayer     = this._playerStateMachine.playerBB.groundLayer;
            var playerPosition  = this._playerStateMachine.transform.position;


            var currentPosition = playerPosition;

            var safeguard = 100;
            while (safeguard-- > 0)
            {
                var nextPos   = currentPosition + _moveDirection * _stepSize;
                var rayOrigin = nextPos + Vector3.up * raycastHeight;

                var isGroundBelow = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);

                //start debug
                var rayColor = isGroundBelow ? Color.green : Color.red;
                Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, rayColor, 1.0f);
                //end debug

                if (!isGroundBelow)
                {
                    this._playerStateMachine.playerBB.canMove = currentPosition != playerPosition;
                    return currentPosition;
                }

                currentPosition = nextPos;

                //start debug
                Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, Color.red);
                //end debug

            }
            return currentPosition;

        }

        public override void OnExit()
        {
            base.OnExit();
            this._playerStateMachine.playerBB.canMove = true;
        }
    }
}