namespace Engine.Storyline
{
    using System.Collections.Generic;

    public interface IStory
    {
        void AddEvents(IEnumerable<StoryEvent> evs);

        void AddEvent(StoryEvent e);

        void Progress(GameTimer gameTimer);
    }
}