using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Storyline
{
    using Common;
    using Engine.Robotics;
    using Engine.Spaceship;
    using VectorMath;

    public class Story
    {
        private readonly List<StoryEvent> events = new List<StoryEvent>();

        public void AddEvents(IEnumerable<StoryEvent> evs)
        {
            foreach (var e in evs)
            {
                events.Add(e);
            }
        }

        public void AddEvent(StoryEvent e)
        {
            if(events.Any() && e.Time < events.Last().Time)
                throw new Exception("Attempt to insert event that happens before last event in queue, events must be added in cronological order.");

            events.Add(e);
        }

        public void Progress(GameTimer gameTimer)
        {
            while(events.Any())
            {
                if (events[0].Time > gameTimer.TotalSeconds)
                    break;

                events[0].Event();
                events.RemoveAt(0);
            }
        }

        public static Story TutorialStory(GameEngine gameEngine, float startTime)
        {
            var story = new Story();
            var t = startTime;
            
            // Boot Ai
            story.AddEvents(AiBootEvents(gameEngine, ref t));
            
            // Enable user input
            story.AddEvent(new StoryEvent(t, () => gameEngine.Ai.Booted = true));

            // Near-field sensor scan
            story.AddEvent(new StoryEvent(t, () => gameEngine.WriteConsole(new CommandResult(true, "\r\nNear-field sensor scan initiated..."))));
            story.AddEvent(new StoryEvent(t += 1.0f, gameEngine.ActivateSensors));
            story.AddEvent(new StoryEvent(t += 2.0f, () => gameEngine.WriteConsole(new CommandResult(true, "  Repair-Bot 'az15' detected at (-30.4, 27.2)."))));
            story.AddEvent(new StoryEvent(t, () =>
                                             {
                                                 var repairBotAz15 = new Robot("az15") { Position = new Vector2(-30.4f, 27.2f) };
                                                 gameEngine.AddRobot(repairBotAz15);
                                             }));
            story.AddEvent(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, "\r\nEstablished near-field communication link to 'az15'."))));
            story.AddEvent(new StoryEvent(t, () => gameEngine.WriteConsole(new CommandResult(true, "\r\nExecuting diagnostics routines..."))));
            story.AddEvent(new StoryEvent(t += 3.0f, () => gameEngine.WriteConsole(new CommandResult(true, "\r\nDiagnostics report:"))));
            story.AddEvent(new StoryEvent(t += 0.2f, () => gameEngine.WriteConsole(new CommandResult(true, "  CPU: OK (1.7 GHz)"))));
            story.AddEvent(new StoryEvent(t += 0.2f, () => gameEngine.WriteConsole(new CommandResult(true, "  RAM: <error: No memory core found>"))));
            story.AddEvent(new StoryEvent(t += 0.2f, () => gameEngine.WriteConsole(new CommandResult(true, "  Engine: OK"))));
            story.AddEvent(new StoryEvent(t += 0.2f, () => gameEngine.WriteConsole(new CommandResult(true, "  Crane: OK"))));
            story.AddEvent(new StoryEvent(t += 0.2f, () => gameEngine.WriteConsole(new CommandResult(true, "  Cargobay: OK (100.0 metric tons)"))));
            story.AddEvent(new StoryEvent(t += 0.2f, () => gameEngine.WriteConsole(new CommandResult(true, "  Sensors: <error: Hardware error>"))));

            return story;
        }

        public static IEnumerable<StoryEvent> AiBootEvents(GameEngine gameEngine, ref float t)
        {
            const string aiBootLine1 = "Hard-Core OS v" + Version.Text + " booting...";
            const string aiBootLine2 = "BIOS check... OK";
            const string aiBootLine3 = "RAM check... ERROR";
            const string aiBootLine4 = "Peripheral sensor calibration... ERROR";
            const string aiBootLine5 = "\r\nFATAL ERRORS ENCOUNTERED!";
            const string aiBootLine6 = "\r\nEntering recovery mode...";
            const string aiBootLine7 = "\r\nAvailable RAM: 0 MB";
            const string aiBootLine8 = "\r\nSystems online";

            var events = new List<StoryEvent>();
            events.Add(new StoryEvent(t += 1.0f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine1))));
            events.Add(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine2))));
            events.Add(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine3))));
            events.Add(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine4))));
            events.Add(new StoryEvent(t += 1.0f, () => gameEngine.WriteConsole(new CommandResult(false, aiBootLine5))));
            events.Add(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine6))));
            events.Add(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine7))));
            events.Add(new StoryEvent(t += 0.5f, () => gameEngine.WriteConsole(new CommandResult(true, aiBootLine8))));

            return events;
        }
    }
}
