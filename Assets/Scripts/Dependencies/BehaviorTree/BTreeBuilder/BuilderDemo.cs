using BehaviorTree;

public class BuilderDemo : BTree
{
    protected override Node SetupTree() //is called in Awake.
    {
        return new BTreeBuilder()
            .Composite<DemoNode>()
            .End()
        .End();
    }
}