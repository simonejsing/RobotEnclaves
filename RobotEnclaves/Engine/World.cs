using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using System.Linq;
    using Engine.Computer;
    using Engine.Items;
    using Engine.Robotics;
    using VectorMath;

    public class World
    {
        private readonly List<IComputer> computers; 
        private readonly List<Robot> robots;
        private readonly List<CollectableItem> items;
        private readonly List<IObject> objects;

        public IEnumerable<Robot> Robots
        {
            get
            {
                return robots;
            }
        }

        public IEnumerable<CollectableItem> Items
        {
            get
            {
                return items;
            }
        }

        public IEnumerable<IObject> Objects
        {
            get
            {
                return objects;
            }
        } 

        public World()
        {
            this.computers = new List<IComputer>();
            this.robots = new List<Robot>();
            this.items = new List<CollectableItem>();
            this.objects = new List<IObject>();
        }

        public void AddComputer(IComputer computer)
        {
            this.computers.Add(computer);
        }

        public void AddItem(CollectableItem item)
        {
            this.items.Add(item);
            AddObject(item);
            item.SetCurrentWorld(this);
        }

        public void AddRobot(Robot robot)
        {
            this.computers.Add(robot.Computer);
            this.robots.Add(robot);
            AddObject(robot.Object);
            robot.SetCurrentWorld(this);
        }

        public void AddObject(IObject obj)
        {
            this.objects.Add(obj);
        }

        public CollectableItem FindItemByName(string name)
        {
            return items.First(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IComputer FindComputerByName(string name)
        {
            return computers.First(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
