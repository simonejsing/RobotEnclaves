﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UnitTests.Stubs
{
    using VectorMath;

    class StubObject : IObject
    {
        public Vector2 Position { get; set; }
    }
}
