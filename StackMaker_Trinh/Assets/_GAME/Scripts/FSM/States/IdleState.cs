namespace _GAME.Scripts.FSM.States
{
    using _GAME.Scripts.BrickManager;
    using _GAME.Scripts.GameManager;

    public class IdleState : BaseState
    {
        public IdleState(StateMachine stateMachine, PlayerStateMachine playerStateMachine, string animationName) :
            base(stateMachine, playerStateMachine, animationName) { }

        public override void OnEnter()
        {
            base.OnEnter();
            this._playerStateMachine.playerBB.ResetSwipe();
            GameEvent.OnInputPermissionChanged?.Invoke(true);
            BridgeManager.Instance.UnlockAll();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(this._playerStateMachine.playerBB.currentSwipeDirection != SwipeDirection.None)
            {
                this._stateMachine.ChangeState(this._playerStateMachine._moveState);
                return;
            }
        }
    }
}