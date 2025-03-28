namespace _GAME.Scripts.FSM.States
{
    using DG.Tweening;
    using UnityEngine;

    public class MoveState : BaseState
    {
        private       int     _maxSteps = 50;
        private const int     _stepSize = 1;
        private Vector3 _targetPosition;
        private Vector3 _moveDirection;
        private bool _isMoving = false;
        public MoveState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) :
            base(stateMachine, playerStateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            var swipe = this._playerStateMachine.playerBB.currentSwipeDirection;

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

            MoveTowardToTarget(_targetPosition);

        }

        private void MoveTowardToTarget(Vector3 target)
        {
            if (_isMoving) return;

            _isMoving = true;

            var moveSpeed = this._playerStateMachine.playerBB.moveSpeed;
            var duration  = Vector3.Distance(this._playerStateMachine.transform.position, target) / moveSpeed;

            this._playerStateMachine.transform.DOMove(target, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _isMoving = false;
                this._stateMachine.ChangeState(this._playerStateMachine._idleState);
            });
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
            this._playerStateMachine.transform.DOKill();
        }
    }
}