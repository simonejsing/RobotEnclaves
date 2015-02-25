namespace Engine
{
    using System.Collections.Generic;
    using Engine.Spaceship;

    public interface IGameConsole
    {
        IEnumerable<KeyValuePair<string, bool>> Lines { get; }

        void WriteResult(CommandResult result);
    }
}