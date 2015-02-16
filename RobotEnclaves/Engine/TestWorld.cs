using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using VectorMath;

    public class TestWorld
    {
        public static World.World Generate()
        {
            var world = new World.World();
            world.InsertObject(new Headquarter() { Position = Vector2.Zero });
            world.InsertObject(new Robot() { Position = new Vector2(-30f, -30f) });
            world.InsertObject(new Robot() { Position = new Vector2(0f, -30f) });
            world.InsertObject(new Robot() { Position = new Vector2(30f, -30f) });

            return world;
        }
    }
}
