using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SylvanSneaker
{
    public interface Camera
    {
        float Scale { get; }
        float MapX { get; }             // MapX and MapY in Tiles
        float MapY { get; }
        int Width { get; }            // width and height in pixels
        int Height { get; }

        int TileSize { get; }
        int ScreenRows { get; }
        int ScreenColumns { get; }
    }
}
