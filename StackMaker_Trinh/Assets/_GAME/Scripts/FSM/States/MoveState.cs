namespace _GAME.Scripts.FSM.States
{
    using _GAME.Scripts.BrickManager;
    using _GAME.Scripts.GameManager;
    using DG.Tweening;
    using UnityEngine;

    public class MoveState : BaseState
    {
        private const int     _stepSize = 1;
        private       Vector3 _targetPosition;
        private       Vector3 _moveDirection;
        private       bool    _isMoving = false;
        private       bool    _isOnBridge;
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

            GameEvent.OnInputPermissionChanged?.Invoke(false);

            /*
            this._playerStateMachine.playerBB.ResetSwipe();
            */


            this._targetPosition = FindTargetPosition();
            Debug.DrawRay(_targetPosition + Vector3.up * 0.5f, Vector3.up * 0.5f, Color.cyan, 2f);
            Debug.Log("[MoveState] Target: " + _targetPosition);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            this._isOnBridge = BridgeManager.Instance.IsPlayerOnBridge(_playerStateMachine.transform.position);
            MoveTowardToTarget(_targetPosition);

        }

        private void MoveTowardToTarget(Vector3 target)
        {
            if (!_isMoving)
            {
                _isMoving = true;
            }

            Vector3 currentPos = _playerStateMachine.transform.position;

            float step = _playerStateMachine.playerBB.moveSpeed * Time.deltaTime;

            _playerStateMachine.transform.position = Vector3.MoveTowards(currentPos, target, step);


            if (Vector3.Distance(currentPos, target) < 0.01f)
            {
                _playerStateMachine.transform.position = SnapToGridCenter(target);
                _isMoving                              = false;
                _stateMachine.ChangeState(_playerStateMachine._idleState);
            }
        }


        private Vector3 SnapToGridCenter(Vector3 position)
        {
            return new Vector3(
                Mathf.Floor(position.x) + 0.5f,
                Mathf.Floor(position.y),
                Mathf.Floor(position.z) + 0.5f
            );
        }

        private Vector3 FindTargetPosition()
        {
            var raycastHeight   = 0.2f;
            var raycastDistance = 1.5f;
            var bridgeLayer     = this._playerStateMachine.playerBB.bridgeLayer;
            var groundLayer     = this._playerStateMachine.playerBB.groundLayer;
            var validLayer      = bridgeLayer | groundLayer;
            var playerPosition  = this._playerStateMachine.transform.position;

            var currentPosition = playerPosition;

            var safeguard = 100;
            while (safeguard-- > 0)
            {
                var nextPos   = currentPosition + _moveDirection * _stepSize;
                var rayOrigin = nextPos + Vector3.up * raycastHeight;
                var isGroundBelow = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, validLayer);

                //start debug
                var rayColor = isGroundBelow ? Color.green : Color.red;
                Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, rayColor, 1.0f);
                //end debug

                if (!isGroundBelow)
                {
                    return currentPosition;
                }

                currentPosition = nextPos;

                //start debug
                Debug.DrawRay(rayOrigin, Vector3.down * raycastDistance, Color.red);
                //end debug

            }
            return currentPosition;

        }

        public void UpdateTargetPosition(Vector3 newTarget)
        {
            _targetPosition = newTarget;
            _isMoving       = true;
        }



        public override void OnExit()
        {
            base.OnExit();
        }
    }
}