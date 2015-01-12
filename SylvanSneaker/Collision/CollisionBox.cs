using SylvanSneaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Collision
{
    public class CollisionBox
    {
        public MapCoordinates MapCoordinates { get; set; }
        public float Radius { get; private set; }

        public CollisionBox(MapCoordinates mapCoordinates, float radius)
        {
            this.MapCoordinates = mapCoordinates;
            this.Radius = radius;
        }
    }
}
