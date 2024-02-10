using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTree : MonoBehaviour
    {
        protected Node _root = null;

        protected virtual void Awake()
        {
            Node.LastId = 0;
            _root = SetupTree();
        }

        private void Update()
        {
            _root?.Evaluate();
        }

        public Node Root => _root;
        protected abstract Node SetupTree();
    }
}
