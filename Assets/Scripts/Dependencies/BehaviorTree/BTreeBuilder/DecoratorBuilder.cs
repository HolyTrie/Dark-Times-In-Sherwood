using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BehaviorTree
{
    public class DecoratorBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        private List<Node> _nodesToAdd; //note : for decorators this length is at most 1!
        private readonly Node _root;
        public Node Root => _root;
        public DecoratorBuilder(K prevBuilder, Node parent, Node node)
        {
            _prevBuilder = prevBuilder;
            _nodesToAdd = new List<Node>();
            _root = node;
            parent.Attach(_root);

        }
        public CompositeBuilder<DecoratorBuilder<K,T>,T> Composite(Node node)
        {
            var builder = new CompositeBuilder<DecoratorBuilder<K,T>,T>(this,Root, node);
            _nodesToAdd.Add(builder.Root);
            return builder;
        }

        public DecoratorBuilder<DecoratorBuilder<K,T>,T> Decorator(Node node)
        {
            var builder = new DecoratorBuilder<DecoratorBuilder<K,T>,T>(this,Root, node);
            _nodesToAdd.Add(builder.Root);
            return builder;
        }

        public LeafBuilder<DecoratorBuilder<K,T>,T> Leaf(Node node)
        {
            var builder = new LeafBuilder<DecoratorBuilder<K,T>,T>(this,Root, node);
            _nodesToAdd.Add(builder.Root);
            return builder;
        }

        public K End()
        {
            Root.SetChildren(_nodesToAdd);
            return _prevBuilder;
        }
    }
}