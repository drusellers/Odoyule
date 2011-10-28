namespace OdoyuleRules.Tests
{
    public interface NodeSelectorFactory
    {
        NodeSelector Create<T>()
            where T : class;
    }
}