using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SylvanSneaker.Sandbox;
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

        void DrawFrame(Texture2D texture, AnimationFrame frame, float mapX, float mapY, Color tint);
        void DrawTile(Texture2D texture, Rectangle sourceRect, int mapX, int mapY, Color tint);
    }
}
