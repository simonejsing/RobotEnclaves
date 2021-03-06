﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Storyline
{
    using Common;
    using Engine.Items;
    using Engine.Robotics;
    using Engine.Spaceship;
    using VectorMath;

    public class Story : IStory
    {
        private readonly List<StoryEvent> timeline = new List<StoryEvent>();

        public void AddEvents(IEnumerable<StoryEvent> evs)
        {
            foreach (var e in evs)
            {
                this.timeline.Add(e);
            }
        }

        public void AddEvent(StoryEvent e)
        {
            if(this.timeline.Any() && e.Time < this.timeline.Last().Time)
                throw new Exception("Attempt to insert event that happens before last event in queue, events must be added in cronological order.");

            this.timeline.Add(e);
        }

        public void Progress(GameTimer gameTimer)
        {
            while(this.timeline.Any())
            {
                if (this.timeline[0].Time > gameTimer.TotalSeconds)
                    break;

                this.timeline[0].Event();
                this.timeline.RemoveAt(0);
            }
        }

        public void Clear()
        {
            timeline.Clear();
        }

        public static Story TutorialStory(GameEngine gameEngine, float startTime)
        {
            var story = new Story();
            var t = startTime;
            
            // Boot Ai
            t = gameEngine.Ai.Reboot(t);

            // AI near-field scan
            story.AddEvent(new StoryEvent(t += 1.0f, () => gameEngine.Ai.Console.WriteResult(new CommandResult(true, "\r\nNear-field sensor scan initiated..."))));
            story.AddEvent(new StoryEvent(t += 2.0f, () => gameEngine.Ai.Console.WriteResult(new CommandResult(true, "  Repair-Bot 'az15' detected at (-52.4, 27.2)."))));
            story.AddEvent(new StoryEvent(t += 0.5f, () => gameEngine.Ai.Console.WriteResult(new CommandResult(true, "\r\nEstablished near-field communication link to 'az15'."))));
            story.AddEvent(new StoryEvent(t, () => gameEngine.Ai.Console.WriteResult(new CommandResult(true, "\r\nExecuting diagnostics routines..."))));
            story.AddEvent(new StoryEvent(t += 0.3f, () => gameEngine.Ai.Console.WriteResult(new CommandResult(true, "> az15.diagnostics()"))));
            story.AddEvent(new StoryEvent(t += 0.1f, () => gameEngine.Ai.ExecuteCommand("az15.diagnostics()")));

            // AI discovers sensor upgrade
            story.AddEvent(new StoryEvent(t += 5.0f, () =>
                                                     {
                                                         var sensor = new SensorUpgrade(new RadarSensor(),"sensor","Damaged sensor array") { Position = new Vector2(340, -260) };
                                                         sensor.ObjectHealth.TakeDamage(83);
                                                         gameEngine.DiscoverItem(sensor);
                                                     }));
            return story;
        }

        public static IEnumerable<StoryEvent> AiBootEvents(Ai ai, ref float t)
        {
            var availableRam = ai.Computer.MemoryBank == null ? 0 : ai.Computer.MemoryBank.SizeMB;
            const string aiBootLine1 = "Hard-Core OS v" + Version.Text + " booting...";
            const string aiBootLine2 = "BIOS check... OK";
            const string aiBootLine3 = "RAM check... ERROR";
            string aiBootLine4 = string.Format("Peripheral sensor calibration... {0}", ai.Sensor.Errors.Any() ? "ERROR" : "OK");
            const string aiBootLine5 = "\r\nFATAL ERRORS ENCOUNTERED!";
            const string aiBootLine6 = "\r\nEntering recovery mode...";
            string aiBootLine7 = string.Format("\r\nAvailable RAM: {0} MB", availableRam);
            const string aiBootLine8 = "\r\nSystems online";

            var events = new List<StoryEvent>();
            events.Add(new StoryEvent(t += 1.0f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine1))));
            events.Add(new StoryEvent(t += 0.5f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine2))));
            events.Add(new StoryEvent(t += 0.5f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine3))));
            events.Add(new StoryEvent(t += 0.5f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine4))));
            events.Add(new StoryEvent(t += 1.0f, () => ai.Console.WriteResult(new CommandResult(false, aiBootLine5))));
            events.Add(new StoryEvent(t += 0.5f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine6))));
            events.Add(new StoryEvent(t += 0.5f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine7))));
            events.Add(new StoryEvent(t += 0.5f, () => ai.Console.WriteResult(new CommandResult(true, aiBootLine8))));

            // Enable user input
            events.Add(new StoryEvent(t, () => ai.Booted = true));

            // Long-range sensor scan (attempt)
            events.Add(new StoryEvent(t, () => ai.Console.WriteResult(new CommandResult(true, "\r\nLong-range sensor scan initiated..."))));
            if (ai.Sensor.Errors.Any())
            {
                events.Add(new StoryEvent(t += 2.5f, () => ai.Console.WriteResult(new CommandResult(false, "  <error: Hardware error>"))));
                events.Add(new StoryEvent(t += 2.5f, () => ai.Console.WriteResult(new CommandResult(true, "Long-range sensor status: DISABLED"))));
            }
            else
            {
                events.Add(new StoryEvent(t += 2.5f, () => ai.Console.WriteResult(new CommandResult(true, "Long-range sensor status: ENABLED"))));
                events.Add(new StoryEvent(t, () => ai.Sensor.Active = true));
            }

            events.Add(new StoryEvent(t, () => ai.Console.WriteResult(new CommandResult(true, "\r\nCore awaiting instructions from AI subsystem\r\n"))));

            return events;
        }
    }
}
