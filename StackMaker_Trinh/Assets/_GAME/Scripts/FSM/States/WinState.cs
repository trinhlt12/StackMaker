namespace _GAME.Scripts.FSM.States
{
    using _GAME.Scripts.GameManager;

    public class WinState : BaseState
    {
        public WinState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) : base(stateMachine, playerStateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var player = this._playerStateMachine.playerBB.player;
            player.ClearBricks();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (this.GetElapsedTime() >= 2f)
            {
                this._stateMachine.ChangeState(this._playerStateMachine._idleState);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}