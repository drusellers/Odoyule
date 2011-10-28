namespace OdoyuleRules.Tests
{
    using System;
    using Configuration.RulesEngineConfigurators;
    using Models.RuntimeModel;

    public interface NodeSelector
    {
//        void Select<T>(RuntimeConfigurator configurator, Action<Node<T>> nodeCallback) 
//            where T : class;

        NodeSelector Next { get; }
    }
}