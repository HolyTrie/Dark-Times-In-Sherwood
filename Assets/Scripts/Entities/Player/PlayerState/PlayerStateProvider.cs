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
        public static PlayerState Build(States state,bool airControl = true) // this code is used to build a new instance of a given state
        {
            return state switch
            {
                States.Grounded => new GroundedState(),
                States.Airborne => new AirborneState(),
                States.Climbing => new ClimbingState(),
                //States.Crouch => new CrouchState(),
                States.Jump => new JumpState(airControl),
                //States.Attack => new AttackState(),
                States.Idle => new IdleState(),
                States.Walk => new WalkState(),
                States.Fly => new FlyState(),
                States.Run => new RunState(),
                //States.Dash => new DashState(),
                States.Fall => new FallState(airControl),
                //States.Jump2 => new Jump2State(),
                //States.LightAttack => new LightAttackState(),
                //States.HeavyAttack => new HeavyAttackState(),
                States.RangedAttack => new RangedAttackState(),
                States.HighAttackState => new HighAttackState(),
                _ => throw new System.Exception("ESP Factory 'build' method does not support entity of type " + state + "\n\t * please check the States enum in the ESP"),
            };
        }
    }
}