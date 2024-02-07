using System.Collections.Generic;

namespace BehaviorTree
{
    public class BTreeBuilder
    {
        private Node root;
        private readonly List<Node> nodesToAdd;
        public BTreeBuilder()
        {
            root = null;
        }
        public CompositeBuilder<BTreeBuilder,Node> Composite(Node node)
        {
            var builder = new CompositeBuilder<BTreeBuilder,Node>(this,root,node);
            if(root == null)
                root = builder.Root;
            else
                nodesToAdd.Add(builder.Root);
            return builder;
        }

        public DecoratorBuilder<BTreeBuilder,Node> Decorator(Node node)
        {
            var builder = new DecoratorBuilder<BTreeBuilder,Node>(this,root,node);
            if(root == null)
                root = builder.Root;
            else
                nodesToAdd.Add(builder.Root);
            return builder;
        }

        public LeafBuilder<BTreeBuilder,Node> Leaf(Node node)
        {
            var builder = new LeafBuilder<BTreeBuilder,Node>(this,root,node);
            if(root == null)
                root = builder.Root;
            else
                nodesToAdd.Add(builder.Root);
            return builder;
        }

        public Node End()
        {
            root.SetChildren(nodesToAdd,true); //sets this node as the root 
            return root;
        }
    }
}