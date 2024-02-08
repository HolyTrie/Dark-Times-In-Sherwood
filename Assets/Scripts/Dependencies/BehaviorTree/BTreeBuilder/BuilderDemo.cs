using System.Linq.Expressions;
using BehaviorTree;

public class BuilderDemo : BTree
{
    protected override Node SetupTree() //is called in Awake.
    {
        Node root = new BTreeBuilder()
            .Composite(new Selector())
                .Composite(new Sequence())
                    .Leaf(new Node())
                    .Leaf(new Node())
                    .End()
                .End()
                .Decorator(new Inverter()) //like logical not, except it is ignored while node is still in running State.
                    .Leaf(new Node())
                    .End() 
                .End()
                .Decorator(new Inverter())
                    .Composite(new Sequence())
                        .Leaf(new Node())
                        .End()
                    .End()
                .End()
                .Leaf(new Node())
                .End()
            .End()
        .End();
        root.PrintRecursively();
        return root;
    }
}