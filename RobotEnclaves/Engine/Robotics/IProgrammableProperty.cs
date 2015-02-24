namespace Engine.Robotics
{
    using Engine.Computer;

    public interface IProgrammableProperty
    {
        string Name { get; }
        bool IsReadOnly { get; }

        IComputerType Get();

        void Set(IComputerType value);
    }
}