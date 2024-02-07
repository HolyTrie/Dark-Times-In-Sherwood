using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class DemoNode : Node
    {
        private bool _bool;
        private float _float;
        private string _string;
        public DemoNode() : base() { }
        public DemoNode(List<Node> children) : base(children) { }

        public DemoNode(bool flag, float num, string str) : base() 
        {
            _bool = flag;
            _float = num;
            _string = str;
            Debug.Log(string.Format("{0} || {1} || {2}",_bool,_float,_string));
        }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        _state = NodeState.SUCCESS;
                        return _state;
                    case NodeState.RUNNING:
                        _state = NodeState.RUNNING;
                        return _state;
                    default:
                        continue;
                }
            }
            _state = NodeState.FAILURE;
            return _state;
        }
    }
}
