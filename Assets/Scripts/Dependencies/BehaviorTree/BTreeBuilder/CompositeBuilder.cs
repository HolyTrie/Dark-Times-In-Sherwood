using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BehaviorTree
{
    public class CompositeBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        private readonly Node _parent;
        private IList<Func<T, object>> _callbacks;
        private IList<object> _parameters;
        private List<Node> nodesToAdd;
        private Node _root;
        public Node Root => _root;
        public CompositeBuilder(K prevBuilder, Node parent, Node node)
        {
            _parent = parent;
            _prevBuilder = prevBuilder;
            nodesToAdd = new List<Node>();
            _root = node;
        }

        public CompositeBuilder<CompositeBuilder<K,T>,T> Composite()
        {
            var builder = new CompositeBuilder<CompositeBuilder<K, T>, T>(this, Root);
            nodesToAdd.Add(builder.Root);
            return builder;
        }

        public DecoratorBuilder<CompositeBuilder<K,T>,T> Decorator()
        {
            var builder = new DecoratorBuilder<CompositeBuilder<K,T>,T>(this,Root);
            nodesToAdd.Add(builder.Root);
            return builder;
        }

        public LeafBuilder<CompositeBuilder<K,T>,T> Leaf()
        {
            var builder = new LeafBuilder<CompositeBuilder<K,T>,T>(this,Root);
            nodesToAdd.Add(builder.Root);
            return builder;
        } 

        //public CompositeBuilder<K,T> add

        public K End()
        {
            Root.SetChildren(nodesToAdd);
            _parent.Attach(Root);
            return _prevBuilder;
        }
    }
}