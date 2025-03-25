namespace _GAME.Scripts.FSM.States
{
    public class MoveState : BaseState
    {
        public MoveState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) :
            base(stateMachine, playerStateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();

        }
    }
}