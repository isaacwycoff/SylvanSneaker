using SylvanSneaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker.Slots
{
    public static class PhysicsSlot
    {
        private static ActionResolver Resolver;

        public static void Initialize(ActionResolver resolver)
        {
            Resolver = resolver;
        }

        public static void Shutdown()
        {

        }

        public static MapCoordinates AttemptToMove(MapCoordinates currentCoordinates, MapCoordinates difference)
        {
            return Resolver.AttemptToMove(currentCoordinates, difference);
        }
    }
}
