using BehaviorTree;

public class BuilderDemo : BTree
{
    protected override Node SetupTree() //is called in Awake.
    {
        Node root = new BTreeBuilder()
            .Composite(new Selector())
                .Composite(new Sequence())
                //  .Leaf(new GoToTarger())
                //  .Leaf(new AttackTarget())
                .End()
                .Decorator(new Inverter()) //like logical not, except it is ignored while node is still in running State.
                //  .Leaf(new HasTarget())
                .End()
              //.Leaf(new Patorl())
              //.End()
            .End()
        .End();
        root.PrintRecursively();
        return root;
    }
}