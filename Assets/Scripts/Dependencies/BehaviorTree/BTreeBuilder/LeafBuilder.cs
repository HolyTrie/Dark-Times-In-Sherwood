using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class LeafBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        private readonly Node _parent;
        private readonly IList<Node> leaves; // here Im doing a list just in case I need to track these nodes later, without spamming new LeafBuilders!
        public LeafBuilder(K prevBuilder, Node parent, Node node)
        {
            _prevBuilder = prevBuilder;
            leaves.Add(node);
            _parent = parent;
            leaves = new List<Node>();
        }

        public LeafBuilder<K,T> Leaf(Node node)
        {
            leaves.Add(node);
            return this;
        }
        public K End()
        {
            _parent.SetChildren(leaves);
            return _prevBuilder;
        }
    }
}