namespace _GAME.Scripts.FSM.States
{
    public class IdleState : BaseState
    {
        public IdleState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) :
            base(stateMachine, playerStateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
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