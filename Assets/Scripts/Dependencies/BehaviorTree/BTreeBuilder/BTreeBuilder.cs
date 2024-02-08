using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// This is mostly syntactic sugar to help initialize the trees hierarchy without getting lost in context 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns> Reference to the same Builder</returns>
        public BTreeBuilder Leaf(Node node)
        {
            if(root == null)
                root = node;
            else
                nodesToAdd.Add(node);
            return this;
        }

        public Node End()
        {
            root.SetChildren(nodesToAdd,true); //sets this node as the root 
            return root;
        }
    }
}