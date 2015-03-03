namespace Engine.Computer
{
    public interface IComputerType
    {
        string TypeName { get; }

        string ToString();

        IComputerType Cast<T>() where T : IComputerType, new();
    }
}