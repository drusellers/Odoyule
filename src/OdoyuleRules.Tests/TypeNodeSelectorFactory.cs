namespace OdoyuleRules.Tests
{
    using System;

    public class TypeNodeSelectorFactory : 
        NodeSelectorFactory
    {
        readonly NodeSelectorFactory _nextFactory;

        public TypeNodeSelectorFactory(NodeSelectorFactory nextFactory)
        {
            _nextFactory = nextFactory;
        }

        public NodeSelector Create<T>() 
            where T : class
        {
            NodeSelector next = null;
            if (_nextFactory != null)
                next = _nextFactory.Create<T>();

            var type = typeof (TypeNodeSelector<>).MakeGenericType(typeof (T));
            var selector = (NodeSelector) Activator.CreateInstance(type, next);

            return selector;
        }    
    }
}