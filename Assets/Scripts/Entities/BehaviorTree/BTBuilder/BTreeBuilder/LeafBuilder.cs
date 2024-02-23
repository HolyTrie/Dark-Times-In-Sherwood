using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class LeafBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        public string name;
        private readonly Node _parent;
        private readonly IList<Node> leaves; // here Im doing a list just in case I need to track these nodes later, without spamming new LeafBuilders!
        public LeafBuilder(K prevBuilder, Node parent, Node node)
        {
            leaves = new List<Node>();
            _prevBuilder = prevBuilder;
            leaves.Add(node);
            _parent = parent;
            name = "Leaf {"+node.Id+"}";
            //Debug.Log("start "+name);
        }

        public LeafBuilder<K,T> Leaf(Node node)
        {
            leaves.Add(node);
            name += "\n"+"Leaf {"+node.Id+"}";
            return this;
        }

        public K End
        {
            get
            {
            _parent.SetChildren(leaves);
            //Debug.Log(name +"\n\t"+"end Leaves");
            return _prevBuilder;
            }
        }
    }
}