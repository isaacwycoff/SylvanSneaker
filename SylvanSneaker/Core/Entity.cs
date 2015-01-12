using SylvanSneaker.Core;
using System;

namespace SylvanSneaker
{
    [Flags]
    public enum EntityCommand
    {
        MoveSouth = 1,
        MoveEast = 2,
        MoveNorth = 4,
        MoveWest = 8,
        AttackPrimary = 16,
    }

    public interface Entity
    {
        // float MapX { get; }
        // float MapY { get; }

        MapCoordinates MapCoordinates { get; }

        void Update(TimeSpan timeDelta);
        void SendCommand(EntityCommand command);
    }
}
