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

        /// <summary>
        /// Make sure Decorators only have 1 direct child node!
        /// Otherwise expect undefined behaviour...
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public DecoratorBuilder<BTreeBuilder,Node> Decorator(Node node)
        {
            var builder = new DecoratorBuilder<BTreeBuilder,Node>(this,root,node);
            if(root == null)
                root = builder.Root;
            else
                nodesToAdd.Add(builder.Root);
            return builder;
        }

        public Node End()
        {
            return root;
        }
    }
}