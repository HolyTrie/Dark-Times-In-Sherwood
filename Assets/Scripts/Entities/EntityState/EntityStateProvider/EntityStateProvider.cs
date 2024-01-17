using Unity.VisualScripting;

namespace DTIS
{
    /// <summary>
    /// Entity State Provider</br>
    /// This class provides a reference to an EntityState, by returning a reference to a static instance of a state
    /// or calling the appropriate factory method to return a new instance.
    /// </summary>
    public class ESP
    {
        /*  
            To add new states there are two steps:
            1. add an identifier to the states enum.
            2. add a static property that returns a static reference, or a new object reference (see examples below)
        */
        public enum States
        {
            Grounded,
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
            // step 1: add new state reference here
        }
        static readonly GroundedState _grounded;
        static readonly CrouchState _crouch;
        static readonly JumpState _jump;
        static readonly AttackState _attack;
        static readonly IdleState _idle;
        static readonly WalkState _walk;
        static readonly FlyState _fly;
        static readonly RunState _run;
        static readonly DashState _dash;
        static readonly FallState _fall;
        static readonly Jump2State _jump2;
        static readonly LightAttackState _lightAttack;
        static readonly HeavyAttackState _heavyAttack;
        static readonly RangedAttackState _rangedAttack;

        public EntityState Provide(States state) // this code decides whether to return a static class reference, or build a new class
        {
            return state switch
            {
                States.Grounded => _grounded, // returns static reference
                States.Crouch => _crouch,
                States.Jump => _jump,
                States.Attack => _attack,
                States.Idle => _idle,
                States.Walk => _walk,
                States.Fly => _fly,
                States.Run => _run,
                States.Dash => _dash,
                States.Fall => Build(States.Fall), //returns new instance reference
                States.Jump2 => _jump2,
                States.LightAttack => _lightAttack,
                States.HeavyAttack => _heavyAttack,
                States.RangedAttack => _rangedAttack,
                _ => _idle,
            };
        }

        private EntityState Build(States state) // this code is used to build a new instance of a given state
        {
            return state switch
            {
                States.Grounded => new GroundedState(),
                States.Crouch => new CrouchState(),
                States.Jump => new JumpState(),
                States.Attack => new AttackState(),
                States.Idle => new IdleState(),
                States.Walk => new WalkState(),
                States.Fly => new FlyState(),
                States.Run => new RunState(),
                States.Dash => new DashState(),
                States.Fall => new FallState(),
                States.Jump2 => new Jump2State(),
                States.LightAttack => new LightAttackState(),
                States.HeavyAttack => new HeavyAttackState(),
                States.RangedAttack => new RangedAttackState(),
                _ => throw new System.Exception("ESP Factory 'build' method does not support entity of type " + state + "\n\t * please check the States enum in the ESP"),
            };
        }
    }
}