using Unity.VisualScripting;

namespace DTIS
{
    /// <summary>
    /// Classic Factory for EntityState types
    /// </summary>
    public static class ESP
    {
        /*  
            To add new states there are two steps:
            1. add an identifier to the states enum.
            2. add a pattern to the match expression, which returns a new object reference (see examples below)
        */
        public enum States
        {
            Grounded,
            Airborne,
            Crouch,
            Jump,
            Attack,
            Idle,
            Walk,
            Fly,
            Run,
            Dash,
            Fall,
            Jump2,
            LightAttack,
            HeavyAttack,
            RangedAttack,
            HighAttackState,
            Climbing
            // step 1: add new state reference here
        }
        public static PlayerState Build(States state, bool airControl = true) // this code is used to build a new instance of a given state
        {
            return state switch
            {
                States.Grounded => new GroundedState(States.Grounded),
                States.Airborne => new AirborneState(States.Airborne),
                States.Climbing => new ClimbingState(States.Climbing),
                //States.Crouch => new CrouchState(States.Crouch), //TODO!!!!!!
                States.Jump => new JumpState(States.Jump,airControl),
                States.Attack => new AttackState(States.Attack),
                States.Idle => new IdleState(States.Idle),
                States.Walk => new WalkState(States.Walk),
                States.Fly => new FlyState(States.Fly),
                States.Run => new RunState(States.Run),
                //States.Dash => new DashState(States.Dash), //TODO!!!!!!!
                States.Fall => new FallState(States.Fall ,airControl),
                States.Jump2 => new Jump2State(States.Jump2,airControl),
                //States.LightAttack => new LightAttackState(),
                //States.HeavyAttack => new HeavyAttackState(),
                States.RangedAttack => new RangedAttackState(States.RangedAttack),
                States.HighAttackState => new HighAttackState(States.HighAttackState),
                States.LightAttack => new LightAttackState(States.LightAttack),
                _ => throw new System.Exception("ESP Factory 'build' method does not support entity of type " + state + "\n\t * please check the States enum in the ESP"),
            };
        }
    }
}