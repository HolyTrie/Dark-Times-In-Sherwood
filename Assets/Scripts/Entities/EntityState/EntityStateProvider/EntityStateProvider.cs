using Unity.VisualScripting;

namespace DTIS
{
    /// <summary>
    /// Entity State Provider</br>
    /// This class provides a reference to an EntityState, by returning a reference to a static instance of a state
    /// or calling the appropriate factory method to return a new instance.
    /// </summary>
    public class ESP{
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
            Run,
            Dash,
            Fall,
            Jump2,
            LightAttack,
            HeavyAttack,
            RangedAttack,
            // step 1: add new state reference here
        }
        static GroundedState _grounded;
        static CrouchState  _crouch;
        static JumpState  _jump;
        static AttackState  _attack;
        static IdleState _idle;
        static WalkState  _walk;
        static RunState  _run;
        static DashState  _dash;
        static FallState  _fall;
        static Jump2State  _jump2;
        static LightAttackState _lightAttack;
        static HeavyAttackState _heavyAttack;
        static RangedAttackState _rangedAttack;

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
                States.Run => new RunState(),
                States.Dash => new DashState(),
                States.Fall => new FallState(),
                States.Jump2 => new Jump2State(),
                States.LightAttack => new LightAttackState(),
                States.HeavyAttack => new HeavyAttackState(),
                States.RangedAttack => new RangedAttackState(),
                _ => throw new System.Exception("ESP Factory 'build' method does not support entity of type " + state + "\n\t * check the States enum in the ESP"),
            };
        }
    }
}