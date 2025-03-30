namespace _GAME.Scripts.FSM.States
{
    public class PickUpState : BaseState
    {
        public PickUpState(StateMachine stateMachine, PlayerStateMachine playerStateMachine, string animationName)
            : base(stateMachine, playerStateMachine, animationName) { }
    }
}