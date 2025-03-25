namespace _GAME.Scripts.FSM.States
{
    public class IdleState : BaseState
    {
        public IdleState(StateMachine stateMachine, PlayerStateMachine playerStateMachine) :
            base(stateMachine, playerStateMachine) { }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}