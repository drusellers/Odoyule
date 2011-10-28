namespace OdoyuleRules.Tests
{
    using System;
    using System.Reflection;
    using Models.RuntimeModel;

    public class PropertyNodeSelectorFactory<TProperty> :
        NodeSelectorFactory
    {
        readonly NodeSelectorFactory _nextFactory;
        readonly PropertyInfo _propertyInfo;

        public PropertyNodeSelectorFactory(NodeSelectorFactory nextFactory, PropertyInfo propertyInfo)
        {
            _nextFactory = nextFactory;
            _propertyInfo = propertyInfo;
        }

        public NodeSelector Create<T>() 
            where T : class
        {
            NodeSelector next = null;
            if (_nextFactory != null)
                next = _nextFactory.Create<Token<T, TProperty>>();

            var type = typeof (PropertyNodeSelector<,>).MakeGenericType(typeof (T), typeof (TProperty));
            var selector = (NodeSelector) Activator.CreateInstance(type, next, _propertyInfo);

            return selector;
        }
    }
}