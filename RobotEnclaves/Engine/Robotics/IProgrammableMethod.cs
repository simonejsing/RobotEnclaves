namespace Engine.Robotics
{
    using Engine.Computer;

    public interface IProgrammableMethod
    {
        string Name { get; }

        IComputerType Invoke(IComputerType arguments);
    }
}