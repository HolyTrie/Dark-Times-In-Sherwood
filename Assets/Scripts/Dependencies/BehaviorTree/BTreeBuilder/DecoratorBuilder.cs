using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BehaviorTree
{
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
    public class DecoratorBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        private readonly Node _root;
        public Node Root => _root;
        public DecoratorBuilder(K prevBuilder, Node parent, Node node)
        {
            _prevBuilder = prevBuilder;
            _root = node;
            parent.Attach(_root);

        }
        public CompositeBuilder<DecoratorBuilder<K,T>,T> Composite(Node node)
        {
            var builder = new CompositeBuilder<DecoratorBuilder<K,T>,T>(this,Root, node);
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
        public DecoratorBuilder<DecoratorBuilder<K,T>,T> Decorator(Node node)
        {
            var builder = new DecoratorBuilder<DecoratorBuilder<K,T>,T>(this,Root, node);
            return builder;
        }

        public LeafBuilder<DecoratorBuilder<K,T>,T> Leaf(Node node)
        {
            var builder = new LeafBuilder<DecoratorBuilder<K,T>,T>(this,Root, node);
            return builder;
        }

        public K End()
        {
            return _prevBuilder;
        }
    }
}