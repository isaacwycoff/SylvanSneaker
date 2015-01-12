using SylvanSneaker.Core;

namespace SylvanSneaker
{
    public interface WorldElement: Element
    {
        MapCoordinates MapCoordinates { get; }
        // float MapX { get; }
        // float MapY { get; }
    }
}
