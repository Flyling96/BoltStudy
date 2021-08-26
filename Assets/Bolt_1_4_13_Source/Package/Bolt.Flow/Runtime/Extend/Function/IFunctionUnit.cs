namespace Bolt.Extend
{
    public interface IFunctionUnit
    {
        string functionName { get; }
    }

    public interface ISubFlowUnit
    {
        string variableName { get; }
    }
}
