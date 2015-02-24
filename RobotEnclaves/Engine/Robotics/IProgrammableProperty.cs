namespace Engine.Robotics
{
    using Engine.Computer;

    public interface IProgrammableProperty
    {
        string Name { get; }
        bool IsReadOnly { get; }

        ComputerType Get();

        void Set(ComputerType value);
    }
}