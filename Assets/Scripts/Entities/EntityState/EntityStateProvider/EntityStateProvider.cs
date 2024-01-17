/// <summary>
/// Entity State Provide </br>
/// This class provides a reference to an EntityState, via returning a reference to a static instance of a state
/// or calling the appropriate factory method to return a new instance.
/// </summary>
public class ESP{
    /*  
        
        To add new states there are three steps:
        1. add an identifier to the states enum.
        2. add a static property that returns a static reference, or a new object reference (see examples below)
    */
    private EntityStateFactory _factory;
    public static enum states
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

    // All 'super' states
    public static GroundedState { get; }
    public static CrouchState  { get; };
    public static JumpState  { get; };
    public static AttackState  { get; };
    // All 'sub' states
    public static IdleState  { get; };
    public static WalkState  { get; };
    public static RunState  { get; };
    public static DashState  { get; };
    public static FallState  { get { return _factory.Build(states.Fall)} };
    public static Jump2State  { get; };
    public static LightAttackState { get; };
    static HeavyAttackState { get; };
    static RangedAttackState { get; };

}