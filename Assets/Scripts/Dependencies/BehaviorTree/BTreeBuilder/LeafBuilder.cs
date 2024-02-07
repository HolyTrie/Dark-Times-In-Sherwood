using System.Collections.Generic;

namespace BehaviorTree
{
    public class LeafBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        private readonly object [] _params;
        private readonly Node _root;
        public Node Root => _root;
        public LeafBuilder(K prevBuilder, Node parent, Node node)
        {
            _prevBuilder = prevBuilder;
            _root = node;
            parent.Attach(_root);
        }

        public K End()
        {
            return _prevBuilder;
        }
    }
}