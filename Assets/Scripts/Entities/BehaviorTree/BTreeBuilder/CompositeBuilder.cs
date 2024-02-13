using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class CompositeBuilder<K,T> where T : Node
    {
        private readonly K _prevBuilder;
        private Node _root;
        public Node Root => _root;
        public string name;
        public CompositeBuilder(K prevBuilder, Node parent, Node node, string _name = "Composite")
        {
            _prevBuilder = prevBuilder;
            _root = node;
            parent?.Attach(node);
            name = _name+" {"+node.Id+"}";
            //Debug.Log("start "+name);
        }

        public CompositeBuilder<CompositeBuilder<K,T>,T> Composite(Node node)
        {
            var builder = new CompositeBuilder<CompositeBuilder<K, T>, T>(this, Root, node);
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
        public DecoratorBuilder<CompositeBuilder<K,T>,T> Decorator(Node node)
        {
            var builder = new DecoratorBuilder<CompositeBuilder<K,T>,T>(this,Root, node);
            return builder;
        }

        public LeafBuilder<CompositeBuilder<K,T>,T> Leaf(Node node)
        {
            var builder = new LeafBuilder<CompositeBuilder<K,T>,T>(this,Root, node);
            return builder;
        }
        public K End
        {
            get
            {
            //Debug.Log("end "+name);
            return _prevBuilder;
            }
        }
    }
}